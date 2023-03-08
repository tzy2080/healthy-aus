using Microsoft.AspNetCore.Mvc;
using QRCoder;
using Microsoft.AspNetCore.Identity;
using System.Drawing;
using System;
using System.Security.Cryptography;
using HealthyAus.Models;
using Microsoft.AspNetCore.Authorization;

namespace HealthyAus.Controllers
{

    public class ContactTracingController : Controller
    {
        private readonly ILogger<ContactTracingController> _logger;
        private readonly STDContext _stdContext;

        //get the database context and logger instance 
        public ContactTracingController(ILogger<ContactTracingController> logger, STDContext sTDContext)
        {
            _logger = logger;
            _stdContext = sTDContext;
        }
        [Authorize]
        public IActionResult Index()
        {
            
            //get current user's username 
            var id = User.Identity.Name;

            //Symmetric encryption of the user name 
            string cipher = SymmetricEncrption(id);
            ViewBag.p = SymmetricDecrption(cipher);

            //generate qr code byte[]
            QRCodeGenerator qRCoder = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCoder.CreateQrCode(cipher, QRCodeGenerator.ECCLevel.Q);
            QRCode qRCode = new QRCode(qRCodeData);

            //convert qr code byte[] into bitmap
            using (MemoryStream ms = new MemoryStream())
            {
                using (Bitmap bmp = qRCode.GetGraphic(20))
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ViewBag.qrcode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    ViewBag.Id = id;
                    ViewBag.cipher = cipher;
                }
            }

            //get all records of current user
            var recrods = from username1 in _stdContext.CONTACTTRACING
                          where username1.username1 == id
                          select username1;
            List<CONTACTTRACING> ctls = new List<CONTACTTRACING>();

            //put the records into list to be passed to the View
            foreach (var record in recrods)
            {
                ctls.Add(record);
            }
            return View(ctls);
        }

        [Authorize]
        public IActionResult Create()
        {
        
            return View();
        }

        //HTTPPOST to create new record of contact tracing 
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("username1,username2,encounter_date,comment,notify_status,notification_msg")] CONTACTTRACING ct)
        {
            DateTime nowUnspecified = DateTime.UtcNow;
            
            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(nowUnspecified,
                TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time"));
            ModelState.Remove("comment");
            //check if the binding is valid 
            if (ModelState.IsValid)
            {
                if(ct.comment == null)
                {
                    ct.comment = "";
                }
                if(ct.encounter_date > now)
                {
                    ModelState.AddModelError("encounter_date", "Cannot select a future date and time!");
                    return View(ct);
                }
                //check if another user is included in the new entry
                if (ct.username2 != "d")
                {
                    string deUsername2 = SymmetricDecrption(ct.username2);
                    ct.username2 = deUsername2;
                    if (deUsername2 == "F")
                    {
                        ViewBag.validation = "invalid";
                        return View(ct);
                    }
                    CONTACTTRACING ct2 = new CONTACTTRACING()
                    {
                        username1 = deUsername2,
                        username2 = ct.username1,
                        encounter_date = ct.encounter_date,
                        comment = "",
                        notify_status = 0,
                        notification_msg = ""
                    };
                    _stdContext.Add(ct2);
                }
                ct.notification_msg = "";
                _stdContext.Add(ct);

                //update the database
                await _stdContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            
            return View(ct);
        }


        [Authorize]
        public IActionResult Edit(int id)
        {
            //get the requested record from database.
            CONTACTTRACING ct = _stdContext.CONTACTTRACING.Where(a=>a.id == id).FirstOrDefault();
            return View(ct);
        }

        //HTTPPOST to edit Comment of an entry
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string comment)
        {
            //get the corresponding record 
            var ct_tobe_updated = _stdContext.CONTACTTRACING.FirstOrDefault(c => c.id == id);
            //update the comment 
            ct_tobe_updated.comment = comment;
            //update the database 
            if (await TryUpdateModelAsync<CONTACTTRACING>(ct_tobe_updated))
                {
                try
                {
                    await _stdContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Unable to save changes", e.Message);
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> notification(string id,List<string> message, string date1, string date2)
        {
            String msg = "";
            foreach (var item in message)
            {
                msg += item + ",";
            }
            DateTime nowUnspecified = DateTime.UtcNow;

            DateTime now = TimeZoneInfo.ConvertTimeFromUtc(nowUnspecified,
                TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time"));

            msg +=  now.ToString("dd/MM/yyyy");
            DateTime date3 = DateTime.Parse(date1);
            DateTime date4 = DateTime.Parse(date2);
            var toBeUpdated = from ct in _stdContext.CONTACTTRACING
                              where ct.encounter_date >= date3 && ct.encounter_date <= date4
                              && ct.username2 == id
                              select ct;
            var ls = new List<CONTACTTRACING>();
            foreach (var ct in toBeUpdated)
            {
                ct.notify_status = 1;
                ct.notification_msg = msg;
                ls.Add(ct);

            }
            foreach (var ct in ls)
            {
                if (await TryUpdateModelAsync<CONTACTTRACING>(ct))
                {
                    try
                    {
                        //await _stdContext.SaveChangesAsync();
                    }
                    catch
                    {
                        return Json(false);
                    }
                }
            }
            await _stdContext.SaveChangesAsync();
            return Json(true);
        }

        //AES 128 CBC Ecryption
        private string SymmetricEncrption (string username)
        {
            byte[] cipher;
            // AES key, 128bit
            string hexk = "6135662b3d3e3f313063646b5b446a66";
            byte[] key =  Convert.FromHexString(hexk);
            // CBC IV, 128bit
            string hexiv = "31323334353637383938373435363332";
            byte[] iv = Convert.FromHexString(hexiv);
            //encryption 
            using (Aes aes = Aes.Create())
            {
                aes.IV = iv;
                aes.Key = key;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                //write the cipher into memory, and retrieve it as Byte Array.
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(username);
                        }

                        cipher = ms.ToArray();
                    }
                }
            }
            //Convert cipher Byte Array into Base64 string
            return Convert.ToBase64String(cipher);
        }

        private string SymmetricDecrption(string cipher)
        {
            byte[] cipherByte;
            byte[] key;
            byte[] iv;
            string hexiv = "31323334353637383938373435363332";
            string hexk = "6135662b3d3e3f313063646b5b446a66";
            try
            {
                cipherByte = Convert.FromBase64String(cipher);
            }
            catch(Exception e)
            {
                return "F";
            }
            key = Convert.FromHexString(hexk);
            iv = Convert.FromHexString(hexiv);
            string plainCipher;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.IV = iv;
                    aes.Key = key;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    //write the cipher into memory, and retrieve it as Byte Array.
                    using (MemoryStream msDecrpt = new MemoryStream(cipherByte))
                    {
                        using (CryptoStream Decs = new CryptoStream(msDecrpt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader Desr = new StreamReader(Decs))
                            {
                                plainCipher = Desr.ReadToEnd();
                            }
                        }
                    }
                }

                return plainCipher;
            }
            catch(Exception e)
            {
                return "F";
            }
        }

        
    }
}


//if (await TryUpdateModelAsync<CONTACTTRACING>(ct_tobe_updated, "",
  //              c => c.comment))