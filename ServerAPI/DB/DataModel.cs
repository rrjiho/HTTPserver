using System.ComponentModel.DataAnnotations.Schema;

namespace ServerAPI.DB
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public int Level { get; set; } = 1;
        public int Experience { get; set; } = 0;
        public int Strength { get; set; } = 10;
        public int Agility { get; set; } = 10;
        public int Intelligence { get; set; } = 10;

        public Resources Resources { get; set; }

        public Ranking Ranking { get; set; }
    }

    [Table("Resources")]
    public class Resources
    {
        public int UserId { get; set; } 
        public int Gold { get; set; }
        public int Gems { get; set; }

        public User User { get; set; }
    }

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
