namespace Insurance.Api.Models
{
    public class Product
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal salesPrice { get; set; }
        public int productTypeid { get; set; }

    }
}
