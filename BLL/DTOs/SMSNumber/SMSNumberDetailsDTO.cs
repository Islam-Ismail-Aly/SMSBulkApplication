using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.SMSNumber
{
    public class SMSNumberDetailsDTO
    {
        public int Id { get; set; }
        public string PhoneNumbers { get; set; } = string.Empty;
    }
}
