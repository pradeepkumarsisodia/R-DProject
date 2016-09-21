using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Globalization;
using System.Configuration;

namespace MVCWeb.Controllers
{
    public class QueryBuilderController : Controller
    {
        // GET: QueryBuilder

        string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
        public ActionResult Index()
        {
            ViewBag.hdnpagename = "queryBuilder";
            return View();
        }

        public JsonResult ExecuteQuery(MyClass objClass)
        {
            var result = new OutputResult();
            try
            {

                var jsonData = string.Empty;
                var selectQuerys = string.Empty;
                var exectueNonQueryOutPut = string.Empty;
                var dataBaseName = objClass.databaseName;
                string[] querys = objClass.query.Replace("\n", "").Split(';');
                foreach (var query in querys)
                {
                    if (query.ToLower().StartsWith("select"))
                    {
                        selectQuerys += query + " ; ";
                    }
                    if (query.ToLower().StartsWith("insert") || query.ToLower().StartsWith("update") || query.ToLower().StartsWith("delete") || query.ToLower().StartsWith("create") || query.ToLower().StartsWith("drop") || query.ToLower().StartsWith("alter"))
                    {
                        exectueNonQueryOutPut += ExectueDMLOprations(query, dataBaseName);
                    }
                }
                if (!string.IsNullOrWhiteSpace(selectQuerys))
                {
                    jsonData = ForSelect(selectQuerys, dataBaseName);
                    result = new OutputResult() { type = "Table", status = "Success", result = jsonData };
                    //  return Json(jsonData, JsonRequestBehavior.AllowGet);
                }
                else if (!string.IsNullOrWhiteSpace(exectueNonQueryOutPut))
                {
                    result = new OutputResult() { type = "String", status = "Success", result = exectueNonQueryOutPut };
                    // return Json(exectueNonQueryOutPut, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = new OutputResult() { type = "String", status = "Error", errorMessage = "Invalid Query" };
                }


            }
            catch (Exception ex)
            {
                result = new OutputResult() { type = "Table", status = "Error", errorMessage= ex.Message, result = "" };
               // var jdata = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
               // result = jdata.Deserialize<OutputResult>(result);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string ForSelect(string Query, string database)
        {
            var jsonData = string.Empty;
            try
            {
                //string constr = "Data Source=.;Database=" + database + ";Integrated Security=SSPI;";
          
               // string constr = "Data Source=182.50.133.109;Database=RHOK; User id=rhok;Password=rhok@123;";
                SqlConnection con = new SqlConnection(connectionString);
                SqlDataAdapter da = new SqlDataAdapter(Query, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                jsonData = CreateJson(ds);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return jsonData;

        }


        public string ExectueDMLOprations(string Query, string database)
        {
            int rowAffected = 0;
           // string connectionString = "Data Source=.;Database=" + database + ";Integrated Security=SSPI;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(Query, connection);
                command.Connection.Open();
                rowAffected = command.ExecuteNonQuery();
            }

            return "Row Affected " + rowAffected + "; <Br />";
        }

        public string CreateJson(DataSet ds)
        {
            var jsonData = "[";
            if (ds != null && ds.Tables.Count > 0)
            {
                var i = 1;
                foreach (DataTable dt in ds.Tables)
                {

                    jsonData += "{";
                    jsonData += "\"tableName\": \"" + dt.TableName + "\",\"TotalRecords\": " + dt.Rows.Count + ",";
                    jsonData += "\"columns\":" + GetDataTableColumnsInJSON(dt);
                    jsonData += ",\"data\":" + GetDataTableDataInJSON(dt);
                    //jsonData += "\"Details\":" + DataTableToJSON(dt);
                    if (i < ds.Tables.Count)
                        jsonData += "},";
                    else
                        jsonData += "}";
                    i++;
                }
            }
            jsonData += "]";
            return jsonData;
        }
        public string GetDataTableColumnsInJSON(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, string>> parentRow = new List<Dictionary<string, string>>();

            foreach (DataColumn col in table.Columns)
            {
                var childRow = new Dictionary<string, string>();
                childRow.Add("title", col.ColumnName);
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }

        public string GetDataTableDataInJSON(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<object> parentRow = new List<object>();
            foreach (DataRow row in table.Rows)
            {
                var childRow = new List<object>();
                foreach (DataColumn col in table.Columns)
                {
                    if (col.DataType.Name == "DateTime")
                    {

                        childRow.Add(Convert.ToDateTime(row[col]).ToShortDateString());
                        // childRow.Add(row[col]);
                    }
                    else
                    {
                        childRow.Add(row[col]);
                    }
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }


    }

    public class MyClass
    {
        public string databaseName { get; set; }
        public string query { get; set; }
    }

    public class OutputResult
    {
        public string errorMessage { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string result { get; set; }
    }
}