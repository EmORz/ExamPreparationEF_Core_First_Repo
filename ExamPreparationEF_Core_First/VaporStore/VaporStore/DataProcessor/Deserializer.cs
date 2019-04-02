using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using VaporStore.Data.Models;
using VaporStore.DataProcessor.Import;

namespace VaporStore.DataProcessor
{
	using System;
	using Data;

	public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
		    var sb = new StringBuilder();

		    var deserializeGames = JsonConvert.DeserializeObject<GameDto[]>(jsonString);
            // Games, Developers, Genres and Tags

            var validGames = new List<Game>();
            var validDevelopers = new List<Developer>();
            var validGenres = new List<Genre>();
            var validTags = new List<Tag>();

		    foreach (var gameDto in deserializeGames)
		    {
		        if (!IsValid(gameDto) || !gameDto.Tags.All(IsValid))
		        {
		            sb.AppendLine("Invalid Data");
                    continue;
		        }

		        var developers = validDevelopers.FirstOrDefault(x => x.Name == gameDto.Developer);
		        if (developers == null)
		        {
		            developers = new Developer(gameDto.Developer);
                    validDevelopers.Add(developers);
		        }

		        var genre = validGenres.FirstOrDefault(x => x.Name == gameDto.Genre);
		        if (genre == null)
		        {
		            genre = new Genre(gameDto.Genre);
		            validGenres.Add(genre);
		        }

                var gameTags = new List<Tag>();
		        foreach (var tagName in gameDto.Tags)
		        {
		            var tag = validTags.FirstOrDefault(x => x.Name == tagName);
		            if (tag == null)
		            {
		                tag = new Tag
		                {
                            Name = tagName
		                };
                        validTags.Add(tag);
		            }
                    gameTags.Add(tag);
		        }

		        var game = new Game
		        {
                    Name = gameDto.Name,
                    Price =  gameDto.Price,
                    ReleaseDate = gameDto.ReleaseDate,
                    Developer = developers,
                    Genre = genre,
                    GameTags = gameTags.Select(x => new GameTag{ Tag = x}).ToArray()
		        };

                validGames.Add(game);

		        sb.AppendLine($"Added {gameDto.Name} ({gameDto.Genre}) with {gameDto.Tags.Length} tags");

            }

		    context.Games.AddRange(validGames);

		    context.SaveChanges();

		    return sb.ToString().TrimEnd();


		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
			throw new NotImplementedException();
		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
			throw new NotImplementedException();
		}

	    public static bool IsValid(object obj)
	    {
	        var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
	        var result = new List<ValidationResult>();

	        return Validator.TryValidateObject(obj, validationContext, result, true);
	    }
    }
}