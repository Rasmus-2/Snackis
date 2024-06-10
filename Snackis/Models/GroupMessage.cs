namespace Snackis.Models
{
    public class GroupMessage
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        
        public string UserId { get; set; }
        public int GroupId { get; set; }
    }
}
