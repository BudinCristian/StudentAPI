using System.ComponentModel.DataAnnotations.Schema;

namespace StudentApi.Entities
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        [ForeignKey("GroupId")]
        public ICollection<Student> Students { get; set; }
    }
}
