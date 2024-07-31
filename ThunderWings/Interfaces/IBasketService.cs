using ThunderWings.Model;

namespace ThunderWings.Interfaces
{
    public interface IBasketService
    {
        Task<Basket> GetBasketAsync();        
        Task SaveBasketAsync(Basket basket);
        Task<string> AddAircraftToBasketAsync(string aircraftName);
        Task<string> RemoveAircraftFromBasketAsync(string aircraftName);
        Task<Receipt> Checkout();
    }
}
