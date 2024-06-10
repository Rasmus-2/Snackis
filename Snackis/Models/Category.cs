namespace Snackis.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        //If parentId is null, then it is a Top-Category
        public int? ParentId { get; set; }

        public string Name { get; set; }
    }
}
