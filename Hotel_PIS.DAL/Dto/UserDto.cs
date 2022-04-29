using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_PIS.DAL.Dto
{
    public class UserDto
    {
        public string Email { get; set; }
        public int? RoleId { get; set; }
        public string? Role{ get; set; }
        public string? FirstName{ get; set; }
        public string? SecondName{ get; set; }
        public string? JWT{ get; set; }
        public string? Password{ get; set; }
    }
}
