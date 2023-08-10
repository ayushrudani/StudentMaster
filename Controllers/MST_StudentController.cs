using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using StudentMaster.Models;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace StudentMaster.Controllers
{
    public class MST_StudentController : Controller
    {
        private IConfiguration configuration;

        public MST_StudentController(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }
        public IActionResult MST_StudentList()
        {

            SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "SELECTALMST_Student";
            DataTable dt = new DataTable();
            SqlDataReader dr = cmd.ExecuteReader();
            dt.Load(dr);
            con.Close();
            if (dt.Rows.Count == 0)
            {
                ViewBag.Country = "NULL";
            }
            return View(dt);
        }
        public IActionResult MST_StudentAddEdit()
        {
            SetBranchDropDownList();
            SetCityDropDownList();
            return View();
        }

        string dateFormate(String input)
        {
            string[] ans = new string[3];
            ans = input.Split(' ');
            return ans[0];
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MST_StudentAddEdit(MST_StudentModel studentModel)
        {
            
            if (ModelState.IsValid)
            {
                SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
                con.Open();
                SqlCommand sqlCommand = con.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                if (studentModel.StudentID == null)
                {
                    sqlCommand.CommandText = "InsertMST_Student";
                    sqlCommand.Parameters.AddWithValue("@Password", studentModel.Password);
                    TempData["message"] = "Record Inserted Successfully";
                }
                else
                {
                    sqlCommand.CommandText = "UPDATEMST_Student";
                    sqlCommand.Parameters.AddWithValue("@ID", studentModel.StudentID);
                    TempData["message"] = "Record Updated Successfully";
                }
                sqlCommand.Parameters.AddWithValue("@BranchID", studentModel.BranchID);
                sqlCommand.Parameters.AddWithValue("@CityID", studentModel.CityID);
                sqlCommand.Parameters.AddWithValue("@StudentName", studentModel.StudentName);
                sqlCommand.Parameters.AddWithValue("@MobileNoStudent", studentModel.MobileNoStudent);
                sqlCommand.Parameters.AddWithValue("@Email", studentModel.Email);
                sqlCommand.Parameters.AddWithValue("@MobileNoFather", studentModel.MobileNoFather);
                sqlCommand.Parameters.AddWithValue("@Address", studentModel.Address);
                sqlCommand.Parameters.AddWithValue("@BirthDate", dateFormate(studentModel.BirthDate.ToString()));
                if(studentModel.IsActive == "Yes")
                {
                sqlCommand.Parameters.AddWithValue("@IsActive", 1);
                }
                else
                {
                    sqlCommand.Parameters.AddWithValue("@IsActive", 0);
                }
                sqlCommand.Parameters.AddWithValue("@Gender", studentModel.Gender);
                
                sqlCommand.ExecuteNonQuery();
                if(studentModel.StudentID == null)
                {
                    return RedirectToAction("MST_StudentList");
                }
                else
                {
                    return RedirectToAction("MST_StudentProfile", new { id = studentModel.StudentID });
                }
            }
            else
            {
                SetBranchDropDownList();
                SetCityDropDownList();
                return View("MST_StudentAddEdit");
            }
        }
        public void SetCityDropDownList()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            conn.Open();
            SqlCommand sqlCommand = conn.CreateCommand(); ;
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "LOC_CityList";
            DataTable dt = new DataTable();
            SqlDataReader dr = sqlCommand.ExecuteReader();
            dt.Load(dr);
            List<LOC_CityDropDown> cityDropDownModelList = new List<LOC_CityDropDown>();
            foreach (DataRow data in dt.Rows)
            {
                LOC_CityDropDown cityDropDownModel = new LOC_CityDropDown();
                cityDropDownModel.CityID = Convert.ToInt32(data["CityID"]);
                cityDropDownModel.CityCode = data["CityCode"].ToString();
                cityDropDownModelList.Add(cityDropDownModel);
            }

            ViewBag.cityDropDownModel = cityDropDownModelList;
            conn.Close();
        }
        public void SetBranchDropDownList()
        {
            SqlConnection conn = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            conn.Open();
            SqlCommand sqlCommand = conn.CreateCommand(); ;
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "MST_BranchList";
            DataTable dt = new DataTable();
            SqlDataReader dr = sqlCommand.ExecuteReader();
            dt.Load(dr);
            List<LOC_BranchDropDownModel> branchDropDownModelList = new List<LOC_BranchDropDownModel>();
            foreach (DataRow data in dt.Rows)
            {
                LOC_BranchDropDownModel branchDropDownModel = new LOC_BranchDropDownModel();
                branchDropDownModel.BranchID = Convert.ToInt32(data["BranchID"]);
                branchDropDownModel.BranchCode = data["BranchCode"].ToString();
                branchDropDownModelList.Add(branchDropDownModel);
            }

            ViewBag.branchDropDownModel = branchDropDownModelList;
            conn.Close();
        }
        public IActionResult MST_StudentProfile(int id)
        {
            SetBranchDropDownList();
            SetCityDropDownList();
            SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            con.Open();
            SqlCommand sqlCommand = con.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "SelectByPKMST_Student";
            sqlCommand.Parameters.AddWithValue("@ID", id);
            DataTable dt = new DataTable();
            SqlDataReader sdr = sqlCommand.ExecuteReader();
            dt.Load(sdr);
            MST_StudentModel studentModel = new MST_StudentModel();
            if (dt.Rows.Count == 1)
            {
                foreach (DataRow dataRow in dt.Rows)
                {
                    studentModel.StudentID = Convert.ToInt32(dataRow[0]);
                    studentModel.BranchID = Convert.ToInt32(dataRow[1]);
                    studentModel.CityID = Convert.ToInt32(dataRow[2]);
                    studentModel.StudentName = dataRow[3].ToString();
                    studentModel.MobileNoStudent = dataRow[4].ToString();
                    studentModel.Email = dataRow[5].ToString();
                    studentModel.MobileNoFather = dataRow[6].ToString();
                    studentModel.Address = dataRow[7].ToString();
                    studentModel.BirthDate = Convert.ToDateTime(dataRow[8].ToString());
                    studentModel.Age = Convert.ToInt32(dataRow[9].ToString());
                    studentModel.IsActive = dataRow[10].ToString();
                    studentModel.Gender = dataRow[11].ToString();
                    studentModel.Password = dataRow[12].ToString();
                    studentModel.Created = dataRow[13].ToString();
                    studentModel.Modified = dataRow[14].ToString();
                    ViewBag.BranchName = dataRow[15].ToString();
                    ViewBag.CityName = dataRow[16].ToString();
                }
            }
            ViewBag.ID = id;
            con.Close();
            return View(studentModel);
            
        }
        public IActionResult MST_StudentDelete(int id)
        {

            try
            {
                SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
                con.Open();
                SqlCommand sqlCommand = con.CreateCommand();
                sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCommand.CommandText = "DELETEMST_Student";
                sqlCommand.Parameters.AddWithValue("@ID", id);
                DataTable dt = new DataTable();
                sqlCommand.ExecuteNonQuery();
                TempData["message"] = "Record Deleted Successfully";
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
            }
            return RedirectToAction("MST_StudentList");
        }
        public IActionResult SearchStudentName(MST_StudentModel studentModel)
        {
            SqlConnection con = new SqlConnection(this.configuration.GetConnectionString("myConnectionString"));
            con.Open();
            SqlCommand sqlCommand = con.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.Text;
            sqlCommand.CommandText = "select ";
            sqlCommand.Parameters.AddWithValue("@StudentName", studentModel.StudentNameSearch);
            DataTable dt = new DataTable();
            sqlCommand.ExecuteNonQuery();
            if(dt.Rows.Count == 0)
            {
                return RedirectToAction("MST_StudentList", dt);
            }
            else
            {
                return RedirectToAction("MST_StudentList", dt);
            }
        }
    }
}
