using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTControl.Models
{
    public class UserLifxFavorite
    {
        public int UserLifxFavoriteId { get; set; }
        public string UserId { get; set; }
        public string JsonValue { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        [ForeignKey("UserId")]
        public AspNetUser User { get; set; }
    }
}
