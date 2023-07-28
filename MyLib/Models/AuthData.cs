using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLib.Models
{
    [Table("AuthData")]
    public class AuthData
    {
        [Key]
        public int userId { get; set; }
        public string? password { get; set; }
        
    }
}
