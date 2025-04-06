using System.ComponentModel.DataAnnotations.Schema;

namespace ServerAPI.Entities
{
    [Table("Resources")]
    public class Resources
    {
        public int UserId { get; set; }
        public int Gold { get; set; }
        public int Gems { get; set; }

        public User User { get; set; }
    }
}
