using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VaporStore.Data.Models
{
    public class Tag
    {
    //    •	Id – integer, Primary Key
    //•	Name – text(required)
    //•	GameTags - collection of type GameTag

        public Tag()
        {
            this.GameTags = new HashSet<GameTag>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<GameTag> GameTags { get; set; }

    }
}