using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;

namespace MVCWeb.Controllers
{
    public class QueryBuilderController : Controller
    {
        // GET: QueryBuilder
        public ActionResult Index()
        {
            return View();
        }

        //public JsonResult ExecuteQuery(MyClass objClass)
        //{
        //    string constr = "Data Source=.;Database=" + objClass.databaseName + ";Integrated Security=SSPI;";
        //    SqlConnection con = new SqlConnection(constr);
        //    SqlDataAdapter da = new SqlDataAdapter(objClass.query, con);
        //    DataSet ds = new DataSet();
        //    da.Fill(ds);

        //    var jsonData = "[";
        //    if (ds != null && ds.Tables.Count > 0)
        //    {
        //        var i = 1;
        //        foreach (DataTable dt in ds.Tables)
        //        {
        //            jsonData += "{";
        //            jsonData += "\"tableName\": \"" + dt.TableName + "\",\"TotalRecords\": " + dt.Rows.Count + ",\"Details\":";
        //            jsonData += DataTableToJSON(dt);
        //            if (i < ds.Tables.Count)
        //                jsonData += "},";
        //            else
        //                jsonData += "}";
        //            i++;
        //        }
        //    }
        //    jsonData += "]";
        //    return Json(jsonData, JsonRequestBehavior.AllowGet);
        //}

        //public string DataTableToJSON(DataTable table)
        //{
        //    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
        //    List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> childRow;
        //    foreach (DataRow row in table.Rows)
        //    {
        //        childRow = new Dictionary<string, object>();
        //        foreach (DataColumn col in table.Columns)
        //        {
        //            childRow.Add(col.ColumnName, row[col]);
        //        }
        //        parentRow.Add(childRow);
        //    }
        //    return jsSerializer.Serialize(parentRow);
        //}

        public JsonResult ExecuteQuery(MyClass objClass)
        {
            var jsonData = "[";
            try
            {
                string constr = "Data Source=.;Database=" + objClass.databaseName + ";Integrated Security=SSPI;";
                SqlConnection con = new SqlConnection(constr);
                SqlDataAdapter da = new SqlDataAdapter(objClass.query, con);
                DataSet ds = new DataSet();
                da.Fill(ds);

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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Json(jsonData, JsonRequestBehavior.AllowGet);
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
                      
                        childRow.Add( Convert.ToDateTime(row[col]).ToShortDateString());
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
}