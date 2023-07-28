
using System.ComponentModel;

namespace UserAuthCaller.Models
{
    public class UserData
    {
        [DisplayName("User Id")]
        public int id { get; set; }
        [DisplayName("User Name")]
        public string? userName { get; set; }
        [DisplayName("Name")]
        public string? name { get; set; }
        [DisplayName("Age")]
        public int age { get; set; }
        [DisplayName("Email")]
        public string? email { get; set; }
        [DisplayName("Contact")]
        public string? phonenumber { get; set; }
        [DisplayName("GUId")]
        public Guid guid { get; set; }
        [DisplayName("Password")]
        public string? password { get; set; }
        [DisplayName("Role")]
        public string? designation { get; set; }
        [DisplayName("Status")]
        public string? status { get; set; }
    }
}