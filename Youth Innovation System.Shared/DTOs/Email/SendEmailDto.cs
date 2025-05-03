using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youth_Innovation_System.Shared.DTOs.Email
{
    public class SendEmailDto
    {
        public string to { get; set; }
        public string body { get; set; }
        public string subject { get; set; }
    }
}
