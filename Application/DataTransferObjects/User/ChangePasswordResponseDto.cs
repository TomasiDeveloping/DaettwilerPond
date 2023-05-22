using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataTransferObjects.User
{
    public class ChangePasswordResponseDto
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
    }
}
