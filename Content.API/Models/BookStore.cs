namespace Content.API.Models
{
    public class BookStore
    {
        public List<Book> Books = new List<Book>
        {
            new Book { Id = 1, Title = "Some one", Author = "Who Som", Price = 1047 },
            new Book { Id = 2, Title = "Some Other", Author = "Man Chee", Price = 847 }
        };

        public Dictionary<Guid, int[]> Orders = new Dictionary<Guid, int[]>
        {
            { Guid.Parse("2845A861-8679-469C-B97D-E931E1638BBF"), new int[] { 1 } },
            { Guid.Parse("070839D5-4774-44F5-843C-10370F575798"), new int[] { 1, 2 } }
        };
    }
}
