using System.ComponentModel.DataAnnotations;

namespace StudentMaster.Models
{
    public class MST_StudentModel
    {
        public int? StudentID { get; set; }

        [Required(ErrorMessage = "Please Enter Branch Name")]
        public int BranchID { get; set; }

        [Required(ErrorMessage = "Please Enter City Name")]
        public int CityID { get; set; }

        [Required(ErrorMessage = "Please Enter Student Name")]
        public string StudentName { get; set;}
        [Required(ErrorMessage = "Please Enter MobileNoStudent")]

        public string MobileNoStudent { get; set;}
        [Required(ErrorMessage = "Please Enter Email")]

        public string Email { get; set;}
        [Required(ErrorMessage = "Please Enter MobileNoFather")]

        public string MobileNoFather { get; set;}
        [Required(ErrorMessage = "Please Enter Address")]

        public string Address { get; set;}
        [Required(ErrorMessage = "Please Enter BirthDate")]

        public DateTime BirthDate { get; set;}

        public int Age { get; set;}
        [Required(ErrorMessage = "Please Enter IsActive")]

        public string IsActive { get; set;}
        [Required(ErrorMessage = "Please Enter Gender")]

        public string Gender { get; set;}
        
        public string? Password { get; set;}

        public string? Created { get; set; }

        public string? Modified { get; set; }


    }
}
