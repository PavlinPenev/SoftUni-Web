namespace Store_Ge.Services.Models
{
    public class PagedList<T> 
        where T : class
    {
        public PagedList()
        {
            Items = new List<T>();
        }

        public PagedList(ICollection<T> items)
        {
            Items = items.ToList();
        }

        public PagedList(ICollection<T> items, int totalItemsCount)
        {
            Items = items.ToList();
            TotalItemsCount = totalItemsCount;
        }

        public List<T> Items { get; set; }

        public int TotalItemsCount { get; set; }
    }
}
