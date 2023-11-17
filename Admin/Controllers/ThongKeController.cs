using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doanphanmem.Admin.Controllers
{
    public class ThongKeController : Controller
    {
        // GET: ThongKe
         public ActionResult chart()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CharData"].ConnectionString;

            List<string> labels = new List<string>();
            List<decimal> data = new List<decimal>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Các thao tác với cơ sở dữ liệu ở đây
                connection.Open();
                string sql = "SELECT MONTH(NgayDH) AS Thang, SUM(Trigia) AS DoanhThu FROM DONDATHANG GROUP BY MONTH(NgayDH)";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            labels.Add(reader["Thang"].ToString());
                            data.Add((decimal)reader["DoanhThu"]);
                        }
                    }
                }
            }
            ViewBag.Labels = labels;
            ViewBag.Data = data;

            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}