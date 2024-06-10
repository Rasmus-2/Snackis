namespace Snackis.Models
{
    public class MyInfoPage
    {
        public int Id { get; set; }
        public DateTime LastChanged { get; set; }
        public string? Header { get; set; }
        public string? Text { get; set; }
        public string? Image { get; set; }

        public string UserId { get; set; }
    }
}
