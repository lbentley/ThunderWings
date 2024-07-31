namespace ThunderWings.Model
{
    public class Basket
    {        
        public List<Entry> Entries { get; set; }
    }

    public class Entry
    {
        public string AircraftName { get; set; }
        public int Quantity { get; set; }
    }

}
