using System.ComponentModel.DataAnnotations;

namespace IoTControl.Models
{
    public class UserLifxAccessToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Access Token")]
        public string AccessToken { get; set; }

        public AspNetUser User { get; set; }
    }
}
