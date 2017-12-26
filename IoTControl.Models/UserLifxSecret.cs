using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IoTControl.Models
{
    public class UserLifxSecret
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string LifxSecret { get; set; }

        public AspNetUser User { get; set; }
    }
}
