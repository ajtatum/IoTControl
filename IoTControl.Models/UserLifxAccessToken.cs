using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTControl.Models
{
    public class UserLifxAccessToken
    {
        public int UserLifxAccessTokenId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Access Token")]
        public string AccessToken { get; set; }

        [ForeignKey("UserId")]
        public virtual AspNetUser User { get; set; }
    }
}
