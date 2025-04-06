using System.ComponentModel.DataAnnotations.Schema;

namespace ServerAPI.Entities
{
    [Table("Rankings")]
    public class Ranking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int Level { get; set; }
        public int Rank { get; set; }
        public DateTime LastUpdated { get; set; }

        public User User { get; set; }
    }
}
