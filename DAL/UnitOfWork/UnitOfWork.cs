using BLL.IRepository;
using DAL.Data;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.UnitOfWork
{
    public class UnitOfWork<T> : IUnitOfWork<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private IGenericRepository<T> _entity;

        public ICampaignRepository _campaignRepository { get; }
        public ISMSNumberRepository _smsNumberRepository { get; }

        public UnitOfWork(ApplicationDbContext context, ICampaignRepository campaignRepository, ISMSNumberRepository smsNumberRepository)
        {
            _context = context;
            _campaignRepository = campaignRepository;
            _smsNumberRepository = smsNumberRepository;
        }
        public IGenericRepository<T> Entity
        {
            get
            {
                return _entity ?? (_entity = new GenericRepository<T>(_context));
            }
        }


        public int Save()
        {
           return _context.SaveChanges();
        }
    }
}
