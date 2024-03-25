namespace BethanysPieShop.Models
{
    public interface IPieRepository
    {
        IEnumerable <Pie> AllPies { get; }

        IEnumerable <Pie> PiesOfTheWeek { get; }

        Pie? GetPieById (int PieId);

        IEnumerable<Pie> SearchPies(string searchQuery);
    }
}
