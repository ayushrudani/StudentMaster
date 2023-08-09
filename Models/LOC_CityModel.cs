using System.ComponentModel.DataAnnotations;

namespace StudentMaster.Models
{
    public class LOC_CityModel
    {
        public int? CityID { get; set; }

        [Required(ErrorMessage = "Please Enter City Name")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "Please Enter City Code")]
        public string CityCode { get; set; }

        [Required(ErrorMessage = "Please Choose State Name")]
        public int StateID { get; set; }
        public string? Created { get; set; }

        public string? Modified { get; set; }
    }
}
