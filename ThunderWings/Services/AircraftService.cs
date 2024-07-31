using System.IO;
using System.Text.Json;
using ThunderWings.Interfaces;
using ThunderWings.Model;

namespace ThunderWings.Services
{
    public class AircraftService : IAircraftService
    {
        private readonly List<Aircraft> _aircraftList;

        public AircraftService(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                _aircraftList = JsonSerializer.DeserializeAsync<List<Aircraft>>(stream).Result;
            }
        }

        public Task<List<Aircraft>> GetAircraftAsync() => Task.FromResult(_aircraftList);
        public async Task<bool> CheckAircraftExists(string name) => !string.IsNullOrEmpty(name) && _aircraftList.Any(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        public async Task<List<Aircraft>> GetFilteredAircraftAsync(string? name, string? country, string? role, string? manufacturer, int? topSpeed, long? price, int page, int pageSize)
        {
            var aircraftList = _aircraftList.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                aircraftList = aircraftList.Where(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(country))
                aircraftList = aircraftList.Where(a => a.Country.Equals(country, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(role))
                aircraftList = aircraftList.Where(a => a.Role.Equals(role, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrEmpty(manufacturer))
                aircraftList = aircraftList.Where(a => a.Manufacturer.Equals(manufacturer, StringComparison.OrdinalIgnoreCase));

            if (topSpeed.HasValue)
                aircraftList = aircraftList.Where(a => a.TopSpeed == topSpeed.Value);

            if (price.HasValue)
                aircraftList = aircraftList.Where(a => a.Price == price.Value);

            return aircraftList.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }        
    }
}