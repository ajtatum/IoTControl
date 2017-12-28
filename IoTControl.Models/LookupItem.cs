using System.ComponentModel.DataAnnotations;

namespace IoTControl.Models
{
    public class LookupItem
    {
        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Category { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Text { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Value { get; set; }
    }
}
