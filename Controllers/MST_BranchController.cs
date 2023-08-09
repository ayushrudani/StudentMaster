using Microsoft.AspNetCore.Mvc;
using StudentMaster.Models;
using System.Data.SqlClient;
using System.Data;

namespace StudentMaster.Controllers
{
    public class MST_BranchController : Controller
    {
        private IConfiguration configuration;

        public MST_BranchController(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }
        public IActionResult MST_BranchList()
        {

            SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SELECTALMST_Branch";
            DataTable dt = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            con.Close();
            if (dt.Rows.Count == 0)
            {
                ViewBag.Branch = "NULL";
            }
            return View(dt);
        }

        public IActionResult MST_BranchAddEdit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MST_BranchAddEdit(MST_BranchModel branchModel)
        {

            if (ModelState.IsValid)
            {
                SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
                con.Open();
                SqlCommand sqlCommand = con.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                if (branchModel.BranchID == null)
                {
                    sqlCommand.CommandText = "InsertMST_Branch";
                    TempData["message"] = "Record Inserted Successfully";
                }
                else
                {
                    sqlCommand.CommandText = "UPDATEMST_Branch";
                    sqlCommand.Parameters.AddWithValue("@ID", branchModel.BranchID);
                    TempData["message"] = "Record Updated Successfully";
                }
                sqlCommand.Parameters.AddWithValue("@BranchName", branchModel.BranchName);
                sqlCommand.Parameters.AddWithValue("@BranchCode", branchModel.BranchCode);
                sqlCommand.ExecuteNonQuery();
                return RedirectToAction("MST_BranchList");
            }
            else
            {
                return View("MST_BranchAddEdit");
            }
        }




        [HttpGet]
        public IActionResult MST_BranchEdit(int id)
        {
            SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            con.Open();
            SqlCommand sqlCommand = con.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "SelectByPKMST_Branch";
            sqlCommand.Parameters.AddWithValue("@ID", id);
            DataTable dt = new DataTable();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            dt.Load(sdr);
            MST_BranchModel branchModel = new MST_BranchModel();
            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    branchModel.BranchID = Convert.ToInt32(dataRow[0]);
                    branchModel.BranchName = dataRow[1].ToString();
                    branchModel.BranchCode = dataRow[2].ToString();
                }
            }
            ViewBag.ID = id;
            con.Close();
            return View("MST_BranchAddEdit", branchModel);
        }

        public IActionResult MST_BranchDelete(int id)
        {
            try
            {
                SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
                con.Open();
                SqlCommand sqlCommand = con.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "DELETEMST_Branch";
                sqlCommand.Parameters.AddWithValue("@id", id);
                DataTable dt = new DataTable();
                sqlCommand.ExecuteNonQuery();
                TempData["message"] = "Record Deleted Successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["message"] = ex.Message;
            }
            return RedirectToAction("MST_BranchList");
        }
    }
}
