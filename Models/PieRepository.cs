
using Microsoft.EntityFrameworkCore;

namespace BethanysPieShop.Models
{
    public class PieRepository:IPieRepository
    {
        private readonly BethanysPieShopDbContext _bethanysPieShopDbContext;

        //Press Alt+Enter to generate constructor
        public PieRepository(BethanysPieShopDbContext bethanysPieShopDbContext)
        {
            _bethanysPieShopDbContext = bethanysPieShopDbContext;
        }

        public IEnumerable<Pie> AllPies
        { get 
            {
                return _bethanysPieShopDbContext.pies.Include(c => c.Category);
            } 
        }

        public IEnumerable<Pie> PiesOfTheWeek
        {
            get 
            { 
            return _bethanysPieShopDbContext.pies.Include(c=> c.Category).Where(p=>p.IsPieOfTheWeek);
            }
        }

        public Pie? GetPieById(int PieId)
        {
            return  _bethanysPieShopDbContext.pies.FirstOrDefault(p => p.PieId == PieId);
        }

        public IEnumerable<Pie> SearchPies(string searchQuery)
        {
            throw new NotImplementedException();
        }
    }
}
