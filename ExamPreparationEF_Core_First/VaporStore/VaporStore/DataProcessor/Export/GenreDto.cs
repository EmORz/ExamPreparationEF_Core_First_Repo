namespace VaporStore.DataProcessor.Export
{
    public class GenreDto
    {
        public int Id { get; set; }

        public string Genre { get; set; }

        public GameDtos[] Games { get; set; }

        public int TotalPlayers { get; set; }
    //    "Id": 4,
    //    "Genre": "Violent",
    //    "Games": [
    //{
    //    "Id": 49,
    //    "Title": "Warframe",
    //    "Developer": "Digital Extremes",
    //    "Tags": "Single-player, In-App Purchases, Steam Trading Cards, Co-op, Multi-player, Partial Controller Support",
    //    "Players": 6

    }

    public class GameDtos   
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Developer { get; set; }

        public string Tags { get; set; }

        public int Players { get; set; }

    }
}