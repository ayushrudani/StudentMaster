using System.ComponentModel.DataAnnotations;

namespace StudentMaster.Models
{
    public class LOC_StateModel
    {
        public int? StateID { get; set; }

        

        [Required(ErrorMessage = "Please Enter State Name")]
        public string StateName { get; set; }

        [Required(ErrorMessage = "Please Enter State Code")]
        public string StateCode { get; set; }

        [Required(ErrorMessage = "Please Choose Country Name")]
        public int CountryID { get; set; }
        public string? Created { get; set; }

        public string? Modified { get; set;}

    }
    public class LOC_StateDropDownModel
    {
        public int StateID { get; set; }

        public string StateCode { get; set; }
    }

}
