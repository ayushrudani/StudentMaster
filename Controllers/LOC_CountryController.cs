using Microsoft.AspNetCore.Mvc;
using System.Data;
using StudentMaster.Models;
using System.Data.SqlClient;


namespace StudentMaster.Controllers
{
    public class LOC_CountryController : Controller
    {
        private IConfiguration configuration;

        public LOC_CountryController(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }
        public IActionResult LOC_CountryList()
        {
            
            SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SELECTALLOC_Country";
            DataTable dt = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            con.Close();
            if(dt.Rows.Count == 0)
            {
                ViewBag.Country = "NULL";
            }
            return View(dt);
        }
        
        public IActionResult LOC_CountryAddEdit() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LOC_countryAddEdit(LOC_CountryModel countryModel) {
           
            if (ModelState.IsValid)
            {
                SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
                con.Open();
                SqlCommand sqlCommand = con.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                if(countryModel.CountryID == null)
                {
                    sqlCommand.CommandText = "InsertLOC_Country";
                    TempData["message"] = "Record Inserted Successfully";
                }
                else
                {
                    sqlCommand.CommandText = "UPDATELOC_Country";
                    sqlCommand.Parameters.AddWithValue("@ID", countryModel.CountryID);
                    TempData["message"] = "Record Updated Successfully";
                }
                sqlCommand.Parameters.AddWithValue("@CountryName", countryModel.CountryName);
                sqlCommand.Parameters.AddWithValue("@CountryCode", countryModel.CountryCode);
                sqlCommand.ExecuteNonQuery();
                return RedirectToAction("LOC_CountryList");
            }
            else
            {
                return View("LOC_CountryAddEdit");
            }
        }




        [HttpGet]
        public IActionResult LOC_CountryEdit(int id)
        {
            SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            con.Open();
            SqlCommand sqlCommand = con.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "SelectByPKLOC_Country";
            sqlCommand.Parameters.AddWithValue("@id", id);
            DataTable dt = new DataTable();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            dt.Load(sdr);
            LOC_CountryModel countryModel = new LOC_CountryModel();
            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    countryModel.CountryID = Convert.ToInt32(dataRow[0]);
                    countryModel.CountryName = dataRow[1].ToString();
                    countryModel.CountryCode = dataRow[2].ToString();
                }
            }
            ViewBag.ID = id;
            con.Close();
            return View("LOC_CountryAddEdit", countryModel);
        }

        public IActionResult LOC_CountryDelete(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
                con.Open();
                SqlCommand sqlCommand = con.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "DELETELOC_Country";
                sqlCommand.Parameters.AddWithValue("@id", id);
                DataTable dt = new DataTable();
                sqlCommand.ExecuteNonQuery();
                TempData["message"] = "Record Deleted Successfully";
                return RedirectToAction("LOC_CountryList");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["message"] = ex.Message;
                TempData["messageType"] = "Error";
                return RedirectToAction("LOC_CountryList");
            }
        }

    }
}
