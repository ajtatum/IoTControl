using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IoTControl.Models
{
    public class UserLifxFavorite
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a name for this favorite.")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a the selector type.")]
        [Display(Name = "Select By")]
        public string SelectorType { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter a the selector value.")]
        [Display(Name = "Select Value")]
        public string SelectorValue { get; set; }

        [Required(ErrorMessage = "Please check if you want to choose a random light.")]
        [Display(Name = "Choose Random Light")]
        public bool PickRandom { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the JSON this favorite.")]
        public string JsonValue { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public AspNetUser User { get; set; }
    }
}
