using ThunderWings.Model;

namespace ThunderWings.Interfaces
{
    public interface IAircraftService
    {
        Task<List<Aircraft>> GetAircraftAsync();
        Task<List<Aircraft>> GetFilteredAircraftAsync(string? name, string? country, string? role, string? manufacturer, int? topSpeed, long? price, int page, int pageSize);
        Task<bool> CheckAircraftExists(string name);
    }
}
