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
            return View();
        }
        public IActionResult LOC_StateAddEdit()
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
            foreach(DataRow data in dt.Rows)
            {
                LOC_CountryDropDownModel countryDropDownModel = new LOC_CountryDropDownModel();
                countryDropDownModel.CountryID = Convert.ToInt32(data["CountryID"]);
                countryDropDownModel.CountryName = data["CountryName"].ToString();
                countryDropDownModelList.Add(countryDropDownModel);
            }
            
            ViewBag.countryDropDownModel = countryDropDownModelList;
            conn.Close();
            return View();
            
        }
        [HttpPost]
        public IActionResult LOC_StateAddEdit(LOC_StateModel stateModel, LOC_CountryDropDownModel dropDownModel)
        {
            Console.WriteLine(dropDownModel.CountryID);
            if (ModelState.IsValid)
            {
                SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
                conn.Open();
                SqlCommand sqlCommand = conn.CreateCommand(); ;
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "InsertLOC_State";
                sqlCommand.Parameters.AddWithValue("@ID", dropDownModel.CountryID);

                DataTable dt = new DataTable();
                //SqlDataReader dr = sqlCommand.ExecuteReader();
                //dt.Load(dr);
                return RedirectToAction("LOC_StateList");
            }
            else
            {
                return View();
            }
        }
        
    }
}
