using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cryptocurrency.Application.Dtos
{
    public class UserDto
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
    }
}
