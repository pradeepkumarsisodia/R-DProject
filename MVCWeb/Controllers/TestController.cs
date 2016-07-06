using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWeb.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            try
            {
                //int a = 0;
                //var b = 4;
                //var c = b / a;
                return View();
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

        public ActionResult MyTest()
        {

            return View();
        }
    }
}