using System.ComponentModel.DataAnnotations;

namespace StudentMaster.Models
{
    public class MST_BranchModel
    {
        public int? BranchID { get; set; }

        [Required(ErrorMessage = "Please Enter Branch Name")]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "Please Enter Branch Name")]
        public string BranchCode { get; set; }

        public string? Created { get; set; }

        public string? Modified { get; set; }
    }
}
