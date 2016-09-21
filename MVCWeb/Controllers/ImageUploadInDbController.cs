using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWeb.Controllers
{
    public class ImageUploadInDbController : Controller
    {
        // GET: ImageUploadInDb
        string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            var message = String.Empty;
            string images = string.Empty;
            string Query = string.Empty;
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
                        images += newName + ",";
                        file.SaveAs(fname);


                    }
                    Query += "Insert into Tbl_ImageCollection(Image_Path) values('" + images + "');";
                    SaveImagesDb(Query);
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

        public void SaveImagesDb(string Query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(Query, connection);
                command.Connection.Open();
                var rowAffected = command.ExecuteNonQuery();
            }
        }

    }
}