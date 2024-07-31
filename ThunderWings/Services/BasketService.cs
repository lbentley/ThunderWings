using System.Runtime.CompilerServices;
using System.Text.Json;
using ThunderWings.Interfaces;
using ThunderWings.Model;

namespace ThunderWings.Services
{
    public class BasketService : IBasketService
    {
        readonly IAircraftService _aircraftService;
        private readonly string filePath = Path.Combine(AppContext.BaseDirectory, "data", "basket.json");

        public BasketService(IAircraftService aircraftService)
        {
            _aircraftService = aircraftService;
        }

        public async Task<Basket> GetBasketAsync()
        {     
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return await JsonSerializer.DeserializeAsync<Basket>(stream);
            }
        }

        public async Task<string> AddAircraftToBasketAsync(string aircraftName)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(aircraftName))
                {
                    var aircraftExists = await _aircraftService.CheckAircraftExists(aircraftName);

                    if (aircraftExists)
                    {
                        var basket = await GetBasketAsync();
                        if (basket == null || basket.Entries == null)
                        {
                            basket = new Basket { Entries = new List<Entry>() };
                        }

                        var existingEntry = basket.Entries.FirstOrDefault(e => e.AircraftName.Equals(aircraftName, StringComparison.OrdinalIgnoreCase));
                        if (existingEntry != null)
                        {
                            existingEntry.Quantity++;
                        }
                        else
                        {
                            basket.Entries.Add(new Entry { AircraftName = aircraftName, Quantity = 1 });
                        }

                        await SaveBasketAsync(basket);

                        return "Successfully added aircraft to basket";
                    }
                    else
                    {
                        return "Please select a valid aircraft";
                    }
                }
                else
                {
                    return "Please select an aircraft to add to the basket";
                }

            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding an item to the basket:" + ex.Message);
            }
        }

        public async Task<string> RemoveAircraftFromBasketAsync(string aircraftName)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(aircraftName))
                {
                    var basket = await GetBasketAsync();
                    if (basket == null || basket.Entries == null)
                    {
                        return "Basket is empty.";
                    }

                    var existingEntry = basket.Entries.FirstOrDefault(e => e.AircraftName.Equals(aircraftName, StringComparison.OrdinalIgnoreCase));
                    if (existingEntry != null)
                    {
                        if (existingEntry.Quantity > 1)
                        {
                            existingEntry.Quantity--;
                        }
                        else
                        {
                            basket.Entries.Remove(existingEntry);
                        }

                        await SaveBasketAsync(basket);
                        return "Successfully removed aircraft from basket";
                    }
                    else
                    {
                        return "Aircraft not found in the basket";
                    }
                }
                else
                {
                    return "Please select an aircraft to remove from the basket";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while removing an item from the basket: " + ex.Message);
            }
        }

        public async Task<Receipt> Checkout()
        {
            try
            {
                var aircraftList = await _aircraftService.GetAircraftAsync();
                var basket = await GetBasketAsync();
                long totalPrice = 0;

                if (basket != null && basket.Entries != null)
                {
                    foreach (var entry in basket.Entries)
                    {
                        var price = aircraftList.Where(s => s.Name == entry.AircraftName).Select(s => s.Price).FirstOrDefault();
                        totalPrice = totalPrice + (price * entry.Quantity);
                    }

                    var receipt = new Receipt();
                    receipt.TotalPrice = totalPrice;
                    receipt.Basket = basket;

                    return receipt;
                }
                else
                {
                    throw new Exception("Your basket is empty");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("There was an issue checking out: " + ex.Message);
            }
        }

        public async Task SaveBasketAsync(Basket basket)
        {
            try
            {     
                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await JsonSerializer.SerializeAsync(stream, basket);
                }
            }
            catch (Exception ex)
            {                
                throw new Exception("An error occurred while saving the basket: " + ex.Message);
            }
        }
        
    }
}
