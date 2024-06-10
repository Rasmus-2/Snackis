namespace Snackis.Models
{
    public class DM
    {
        public int Id { get; set; }

        //First Id wrote the message
        public List<string> UserIds { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}
