namespace Snackis.Models
{
    public class FlagPost
    {
        public int Id { get; set; }
        public int ForumPostId { get; set; }
        public List<string> FlaggedByUserIds { get; set; }
    }
}
