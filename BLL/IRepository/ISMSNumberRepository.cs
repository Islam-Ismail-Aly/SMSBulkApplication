using BLL.DTOs.SMSNumber;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IRepository
{
    public interface ISMSNumberRepository : IGenericRepository<SMSNumber>
    {
        public IEnumerable<SMSNumber> GetAllNumbers();
        public IEnumerable<SMSNumber> GetAllNumbersById(int id);
    }
}
