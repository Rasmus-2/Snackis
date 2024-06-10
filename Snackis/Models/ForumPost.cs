namespace Snackis.Models
{
    public class ForumPost
    {
        public int Id { get; set; }

        //If parentId is null, then it is a Top-level-post
        public int? ParentId { get; set; }

        public DateTime Date { get; set; }
        public string? Header { get; set; }
        public string? Text { get; set; }
        public string? Image { get; set; }
        public List<string> UserIdLikes { get; set; }
        
        public string UserId { get; set; }
        public int CategoryId { get; set; }
    }
}
