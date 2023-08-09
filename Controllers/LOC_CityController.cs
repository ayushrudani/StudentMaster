using Microsoft.AspNetCore.Mvc;
using StudentMaster.Models;
using System.Data.SqlClient;
using System.Data;

namespace StudentMaster.Controllers
{
    public class LOC_CityController : Controller
    {
        private IConfiguration configuration;
        public LOC_CityController(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }
        public IActionResult LOC_CityList()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            conn.Open();
            SqlCommand sqlCommand = conn.CreateCommand(); ;
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "SELECTALLOC_City";
            DataTable dt = new DataTable();
            SqlDataReader dr = sqlCommand.ExecuteReader();
            dt.Load(dr);
            if (dt.Rows.Count == 0)
            {
                ViewBag.City = "NULL";
            }
            return View(dt);
        }
        public void SetDropDownList()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            conn.Open();
            SqlCommand sqlCommand = conn.CreateCommand(); ;
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "LOC_StateList";
            DataTable dt = new DataTable();
            SqlDataReader dr = sqlCommand.ExecuteReader();
            dt.Load(dr);
            List<LOC_StateDropDownModel> stateDropDownModelList = new List<LOC_StateDropDownModel>();
            foreach (DataRow data in dt.Rows)
            {
                LOC_StateDropDownModel stateDropDownModel = new LOC_StateDropDownModel();
                stateDropDownModel.StateID = Convert.ToInt32(data["StateID"]);
                stateDropDownModel.StateCode = data["StateCode"].ToString();
                stateDropDownModelList.Add(stateDropDownModel);
            }
            ViewBag.stateDropDownModel = stateDropDownModelList;
            conn.Close();
        }
        public IActionResult LOC_CityAddEdit()
        {
            SetDropDownList();
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LOC_CityAddEdit(LOC_CityModel cityModel)
        {
            if (ModelState.IsValid)
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
                conn.Open();
                SqlCommand sqlCommand = conn.CreateCommand(); ;
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                if (cityModel.CityID == null)
                {
                    sqlCommand.CommandText = "InsertLOC_City";
                    TempData["message"] = "Record Inserted Successfully";
                }
                else
                {
                    sqlCommand.CommandText = "UPDATELOC_City";
                    sqlCommand.Parameters.AddWithValue("@ID", cityModel.CityID);
                    TempData["message"] = "Record Updated Successfully";
                }
                sqlCommand.Parameters.AddWithValue("@StateID", cityModel.StateID);
                sqlCommand.Parameters.AddWithValue("@CityName", cityModel.CityName);
                sqlCommand.Parameters.AddWithValue("@CityCode", cityModel.CityCode);
                DataTable dt = new DataTable();
                SqlDataReader dr = sqlCommand.ExecuteReader();
                dt.Load(dr);
                return RedirectToAction("LOC_CityList");
            }
            else
            {
                SetDropDownList();
                return View();
            }
        }
        [HttpGet]
        public IActionResult LOC_CityEdit(int id)
        {
            SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            con.Open();
            SqlCommand sqlCommand = con.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "SelectByPKLOC_City";
            sqlCommand.Parameters.AddWithValue("@ID", id);
            DataTable dt = new DataTable();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            dt.Load(sdr);
            LOC_CityModel cityModel = new LOC_CityModel();
            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    cityModel.CityID = Convert.ToInt32(dataRow[0]);
                    cityModel.StateID = Convert.ToInt32(dataRow[1]);
                    cityModel.CityName = dataRow[3].ToString();
                    cityModel.CityCode = dataRow[4].ToString();
                }
            }
            ViewBag.ID = id;
            con.Close();
            SetDropDownList();
            return View("LOC_CityAddEdit", cityModel);
        }
        public IActionResult LOC_CityDelete(int id)
        {

            try
            {
                SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
                con.Open();
                SqlCommand sqlCommand = con.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "DELETELOC_City";
                sqlCommand.Parameters.AddWithValue("@ID", id);
                DataTable dt = new DataTable();
                sqlCommand.ExecuteNonQuery();
                TempData["message"] = "Record Deleted Successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["message"] = ex.Message;
            }
            return RedirectToAction("LOC_CityList");
        }
    }
}
