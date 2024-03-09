using BLL.DTOs.SMSNumber;
using BLL.IRepository;
using BLL.IServices;
using DAL.Data;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public class SMSNumberRepository : GenericRepository<SMSNumber>, ISMSNumberRepository
    {
        private readonly ApplicationDbContext _context;
        public SMSNumberRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SMSNumber> GetAllNumbers()
        {
            return _context.SMSNumbers
                .Include(c => c.Campaign).ToList();
        }
        
        public IEnumerable<SMSNumber> GetAllNumbersById(int id)
        {
            return _context.SMSNumbers
                .Include(c => c.Campaign).Where(c => c.Id == id).ToList();
        }
    }
}
