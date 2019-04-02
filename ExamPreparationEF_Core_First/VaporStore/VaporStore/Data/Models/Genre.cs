using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class Genre
    {
    //    •	Id – integer, Primary Key
    //•	Name – text(required)
    //•	Games - collection of type Game

        public Genre(string name)
        {
            this.Name = name;
        }
        public Genre()
        {
            this.Games = new HashSet<Game>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Game> Games { get; set; }

    }
}