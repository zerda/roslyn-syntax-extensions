using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.Sample.Models
{
    [Table("Eating")]
    public class Food
    {
        [Required]
        [MaxLength(100)]
        public string Name
        {
            get;
            set;
        }

        public int? Calorie
        {
            get;
            set;
        }
    }
}