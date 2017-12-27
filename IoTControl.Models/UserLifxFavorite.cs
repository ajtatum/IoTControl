using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTControl.Models
{
    public class UserLifxFavorite
    {
        public int UserLifxFavoriteId { get; set; }
        public string UserId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a name for this favorite.")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a the selector.")]
        public string Selector { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the JSON this favorite.")]
        public string JsonValue { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        [ForeignKey("UserId")]
        public AspNetUser User { get; set; }
    }
}
