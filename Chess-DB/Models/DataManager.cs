using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChessDB.Model
{
    public class DataManager
    {
        public List<Player> Players { get; set; } = new();
        public List<Competition> Competitions { get; set; } = new();

        private const string FilePath = "data.json";

        public void Save()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };

            string json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(FilePath, json);
        }

        public static DataManager Load()
        {
            if (!File.Exists(FilePath))
                return new DataManager();

            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<DataManager>(json)
                   ?? new DataManager();
        }

        public void UpdateEloAfterGame(Game game)
        {

        }
    }
}
