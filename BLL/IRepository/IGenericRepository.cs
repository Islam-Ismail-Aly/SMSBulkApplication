using BLL.Entities;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        Subscription GetByUserId(object id);
        Task<T> GetByIdAsync(Guid id);
        void Add(T entity);
        void Update(T entity);
        void Delete(object id);
        void AddRange(T entities);
        void AddRange(IEnumerable<SMSNumber> smsNumbers);
        public void Add(SMSNumber smsNumbers);
    }
}
