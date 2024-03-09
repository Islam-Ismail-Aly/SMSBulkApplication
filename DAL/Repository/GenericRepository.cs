using BLL.Entities;
using BLL.IRepository;
using DAL.Data;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> table = null;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }
        public void Delete(object id)
        {
            T existing = GetById(id);
            table.Remove(existing);
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public T GetById(object id)
        {
            return table.Find(id);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await table.FindAsync(id);
        }

        public void Add(T entity) => table.Add(entity);

        public void Update(T entity)
        {
            table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void AddRange(T entities) => _context.AddRange(entities);

        public void AddRange(IEnumerable<SMSNumber> smsNumbers)=> _context.AddRange(smsNumbers);
        public void Add(SMSNumber smsNumbers) => _context.AddRange(smsNumbers);

        public Subscription GetByUserId(object userId)
        {
            return _context.Subscriptions.SingleOrDefault(x => x.UserId == userId);
        }
    }
}
