using Microsoft.EntityFrameworkCore;
using Payment.API.ServiceInterfaces;

namespace Payment.API.Helpers
{
    public class ServiceRepository<T> : IServiceRepository<T> where T : class
    {
        private  AppDbContext _context = null;
        private DbSet<T> dbSet = null;      
        
        public ServiceRepository(AppDbContext _context)
        {
            this._context = _context;
            dbSet = _context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }
        //This method will return the specified record from the table
        //based on the ID which it received as an argument
        public T GetById(object id)
        {
            return dbSet.Find(id);
        }
        public void Insert(T obj)
        {
            dbSet.Add(obj);
        }
        public void Update(T obj)
        {
            dbSet.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
        public void Delete(object id)
        {
            T existing = dbSet.Find(id);
            dbSet.Remove(existing);
        }        
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
