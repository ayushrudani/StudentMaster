using Microsoft.AspNetCore.Mvc;
using StudentMaster.Models;
using System.Data;
using System.Data.SqlClient;

namespace StudentMaster.Controllers
{
    public class LOC_StateController : Controller
    {
        private IConfiguration configuration;
        public LOC_StateController(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }
        public IActionResult LOC_StateList()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            conn.Open();
            SqlCommand sqlCommand = conn.CreateCommand(); ;
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "SELECTALLOC_State";
            DataTable dt = new DataTable();
            SqlDataReader dr = sqlCommand.ExecuteReader();
            dt.Load(dr);
            if (dt.Rows.Count == 0)
            {
                ViewBag.State = "NULL";
            }
            return View(dt);
        }
        public void SetDropDownList()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            conn.Open();
            SqlCommand sqlCommand = conn.CreateCommand(); ;
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "LOC_CountryList";
            DataTable dt = new DataTable();
            SqlDataReader dr = sqlCommand.ExecuteReader();
            dt.Load(dr);
            List<LOC_CountryDropDownModel> countryDropDownModelList = new List<LOC_CountryDropDownModel>();
            foreach (DataRow data in dt.Rows)
            {
                LOC_CountryDropDownModel countryDropDownModel = new LOC_CountryDropDownModel();
                countryDropDownModel.CountryID = Convert.ToInt32(data["CountryID"]);
                countryDropDownModel.CountryCode = data["CountryCode"].ToString();
                countryDropDownModelList.Add(countryDropDownModel);
            }

            ViewBag.countryDropDownModel = countryDropDownModelList;
            conn.Close();
        }
        public IActionResult LOC_StateAddEdit()
        {
            SetDropDownList();
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LOC_StateAddEdit(LOC_StateModel stateModel)
        {
            if (ModelState.IsValid)
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
                conn.Open();
                SqlCommand sqlCommand = conn.CreateCommand(); ;
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                if (stateModel.StateID == null)
                {
                    sqlCommand.CommandText = "InsertLOC_State";
                    TempData["message"] = "Record Inserted Successfully";
                }
                else
                {
                    sqlCommand.CommandText = "UPDATELOC_State";
                    sqlCommand.Parameters.AddWithValue("@ID", stateModel.StateID);
                    TempData["message"] = "Record Updated Successfully";
                }
                sqlCommand.Parameters.AddWithValue("@CountryID", stateModel.CountryID);
                sqlCommand.Parameters.AddWithValue("@StateName", stateModel.StateName);
                sqlCommand.Parameters.AddWithValue("@StateCode", stateModel.StateCode);
                DataTable dt = new DataTable();
                SqlDataReader dr = sqlCommand.ExecuteReader();
                dt.Load(dr);
                return RedirectToAction("LOC_StateList");
            }
            else
            {
                SetDropDownList();
                return View();
            }
        }
        [HttpGet]
        public IActionResult LOC_StateEdit(int id)
        {
            SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            con.Open();
            SqlCommand sqlCommand = con.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "SelectByPKLOC_State";
            sqlCommand.Parameters.AddWithValue("@ID", id);
            DataTable dt = new DataTable();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            dt.Load(sdr);
            LOC_StateModel stateModel = new LOC_StateModel();
            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    stateModel.StateID = Convert.ToInt32(dataRow[0]);
                    stateModel.CountryID = Convert.ToInt32(dataRow[1]);
                    stateModel.StateName = dataRow[2].ToString();
                    stateModel.StateCode = dataRow[3].ToString();
                }
            }
            ViewBag.ID = id;
            con.Close();
            SetDropDownList();
            return View("LOC_StateAddEdit", stateModel);
        }
        public IActionResult LOC_StateDelete(int id)
        {
            SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            con.Open();
            SqlCommand sqlCommand = con.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "DELETELOC_State";
            sqlCommand.Parameters.AddWithValue("@ID", id);
            DataTable dt = new DataTable();
            sqlCommand.ExecuteNonQuery();
            TempData["message"] = "Record Deleted Successfully";
            return RedirectToAction("LOC_StateList");
        }
    }
}
