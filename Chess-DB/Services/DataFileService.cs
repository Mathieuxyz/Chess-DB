using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ChessDB.Model;

namespace Chess_DB.Services
{
    /// <summary>
    /// Handles reading and writing the application data to a JSON file.
    /// </summary>
    public static class DataFileService
    {
        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ChessDB",
            "data.json");

        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };

        public static async Task<DataManager> LoadAsync()
        {
            try
            {
                await using var fs = File.OpenRead(FilePath);
                var data = await JsonSerializer.DeserializeAsync<DataManager>(fs, Options);
                return data ?? new DataManager();
            }
            catch (FileNotFoundException)
            {
                return new DataManager();
            }
            catch (DirectoryNotFoundException)
            {
                return new DataManager();
            }
        }

        public static async Task SaveAsync(DataManager data)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            await using var fs = File.Create(FilePath);
            await JsonSerializer.SerializeAsync(fs, data, Options);
        }
    }
}
