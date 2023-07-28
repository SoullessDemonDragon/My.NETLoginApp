using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLib.Models
{
    [Table("UserTable")]
    public class UserData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        
        public string? userName { get; set; }

        public string? name { get; set; }
        
        public int age { get; set; }
        
        public string? email { get; set; }

        public string? phonenumber { get; set; }

        public Guid guid { get; set; }

        public string? password { get; set; }

        public string? designation { get; set; }
        public string? status { get; set; }

    }
}
