using System.ComponentModel.DataAnnotations;
using VaporStore.Data.Models.Enum;

namespace VaporStore.DataProcessor.Import
{
    public class UserDto
    {
        //    "FullName": "Invalid Invalidman",
        //    "Username": "invalid",
        //    "Email": "invalid@invalid.com",
        //    "Age": 104,
        //    "Cards": [
        //{
        //    "Number": "1111 1111 1111 1111",
        //    "CVC": "111",
        //    "Type": "Debit"
        //}
        //]


        [Required]
        [RegularExpression("([A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+)")] //todo add create reqular expresion
        public string FullName { get; set; }

        [Required]
        [MinLength(3), MaxLength(20)]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Range(typeof(int), "3", "103")]
        public int Age { get; set; }

        [MinLength(1)]
        public CardDto[] Cards { get; set; }

    }

    public class CardDto
    {
        [Required]
        [RegularExpression("([0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4})")] //
        public string Number { get; set; }

        [Required]
        [RegularExpression("([0-9]{3})")] // 
        public string CVC { get; set; }

        [Required]
        public CardType Type { get; set; }
    }
}