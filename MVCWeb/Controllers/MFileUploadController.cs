using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWeb.Controllers
{
    public class MFileUploadController : Controller
    {
        // GET: MFileUpload
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Multiple(IEnumerable<HttpPostedFileBase> files)
        {
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var a = Path.Combine(Server.MapPath("/uploads"), Guid.NewGuid() + Path.GetExtension(file.FileName));
                    file.SaveAs(a);
                   
                }
            }
          //  return Json("File Upload Successfully", JsonRequestBehavior.AllowGet);
            return RedirectToAction("Index","MFileUpload");
        }


        [HttpPost]
        public ActionResult UploadFiles()
        {
            var message = String.Empty;
            string images = string.Empty;
            // Checking no of files injected in Request object  
            if (Request.Files.Count > 0)
            {
                
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                  
                    for (int i = 0; i < files.Count; i++)
                    {                    
                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                       // fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        string newName = Guid.NewGuid() + Path.GetExtension(fname);
                        fname = Path.Combine(Server.MapPath("/uploads"), newName);
                        images += newName;
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    //return Json( "File Uploaded Successfully!");
                    
                }
                catch (Exception ex)
                {
                    message = "Error occurred. Error details: " + ex.Message;                    
                }
               
            }
            else
            {
                message = "No files selected";
            }
            return Json(new { status = message, data = images });
        }  
    }
}