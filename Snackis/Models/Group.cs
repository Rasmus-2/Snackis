namespace Snackis.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        //First Id is host
        public List<string> UserIds { get; set; }
    }
}
