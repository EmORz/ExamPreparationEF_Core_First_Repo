using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VaporStore.Data.Models.Enum;
using VaporStore.DataProcessor.Export;

namespace VaporStore.DataProcessor
{
	using System;
	using Data;

	public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
		    var result = context
		        .Genres
		        .Where(x => genreNames.Contains(x.Name))
		        .Select(x => new GenreDto()
		        {
                    Id = x.Id,
                    Genre = x.Name,
                    Games = x.Games.Select(y => new GameDtos
                    {
                        Id = y.Id,
                        Title = y.Name,
                        Developer = y.Developer.Name,
                        Tags = String.Join(", ", y.GameTags.Select(s => s.Tag.Name)),
                        Players = y.Purchases.Count

                    })
                        .OrderByDescending( p => p.Players)
                        .ThenBy(g => g.Id)
                        .ToArray(),
		            TotalPlayers = x.Games.Sum(t => t.Purchases.Count)
                })
		        .OrderByDescending(x => x.TotalPlayers)
		        .ThenBy(x => x.Id)
		        .ToArray();


		    var json = JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
		    return json;

            //Order the genres by total player count (descending), then by genre id (ascending)
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
		    var storeTypeValue = Enum.Parse<PurchaseType>(storeType);
		    var purchase = context
		        .Users
		        .Select(u => new UserDto
		        {
                    Username = u.Username,
                    Purchases = u.Cards
                        .SelectMany(s => s.Purchases)
                        .Where(a => a.Type == storeTypeValue)
                        .Select(p => new PurchaseDtos
                        {
                            Card = p.Card.Number,
                            Cvc =  p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new PurchaseGameDtos
                            {
                                Title = p.Game.Name,
                                Genre = p.Game.Genre.Name,
                                Price = p.Game.Price
                            }
                        })
                        .OrderBy(uu => uu.Date)
                        .ToArray(),
                    TotalSpent = u.Cards
                        .SelectMany( y => y.Purchases)
                        .Where(q => q.Type==storeTypeValue)
                        .Sum(d => d.Game.Price)
		        })
		        .Where( p => p.Purchases.Any())
		        .OrderByDescending(ts => ts.TotalSpent)
		        .ThenBy(un => un.Username)
		        .ToArray();

		    XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserDto[]), new XmlRootAttribute("Users"));

		    var sb = new StringBuilder();

		    var namespaces = new XmlSerializerNamespaces(new[]
		    {
		        new XmlQualifiedName("", ""),
		    });

		    xmlSerializer.Serialize(new StringWriter(sb), purchase, namespaces);


		    return sb.ToString().TrimEnd();



		}
	}
}