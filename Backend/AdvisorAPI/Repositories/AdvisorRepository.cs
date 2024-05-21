using AdvisorAPI.Data;
using AdvisorAPI.Models;
using AdvisorAPI.Services;

namespace AdvisorAPI.Repositories
{
    public interface IAdvisorRepository
    {
        Advisor Create(Advisor advisor);
        void Delete(int id);
        Advisor Get(int id);
        IEnumerable<Advisor> List();
        void Update(Advisor advisor);
    }
    public class AdvisorRepository : IAdvisorRepository
    {
        private readonly AdvisorContext _context;
        private readonly MRUCache<int, Advisor> _cache;

        public AdvisorRepository(AdvisorContext context)
        {
            _context = context;
            _cache = new MRUCache<int, Advisor>();
        }

        public Advisor Create(Advisor advisor)
        {
            _context.Advisors.Add(advisor);
            _context.SaveChanges();
            _cache.Put(advisor.Id, advisor);
            return advisor;
        }

        public void Delete(int id)
        {
            var advisor = _context.Advisors.Find(id);
            if (advisor != null)
            {
                _context.Advisors.Remove(advisor);
                _context.SaveChanges();
                _cache.Delete(id);
            }
        }

        public Advisor Get(int id)
        {
            var advisor = _cache.Get(id);
            if (advisor == null)
            {
                advisor = _context.Advisors.Find(id);
                if (advisor != null)
                {
                    _cache.Put(id, advisor);
                }
            }
            return advisor;
        }

        public IEnumerable<Advisor> List()
        {
            return _context.Advisors.ToList();
        }

        public void Update(Advisor advisor)
        {
            _context.Advisors.Update(advisor);
            _context.SaveChanges();
            _cache.Put(advisor.Id, advisor);
        }
    }
}
