using System.ComponentModel.DataAnnotations;

namespace StudentMaster.Models
{
    public class LOC_StateModel
    {
        public int? StateID { get; set; }

        [Required(ErrorMessage = "Plese Choose Country Name")]
        public int CountryID { get; set; }

        public List<LOC_CountryDropDownModel> llist { get; set; }

        [Required(ErrorMessage = "Plese Enter State Name")]
        public string StateName { get; set; }

        [Required(ErrorMessage = "Plese Enter State Code")]
        public string StateCode { get; set; }

        public string Created { get; set; }

        public string Modified { get; set;}

    }
    public class LOC_CountryDropDownModel
    {
        public int CountryID { get; set; }

        public string CountryName { get; set; }
    }
}
