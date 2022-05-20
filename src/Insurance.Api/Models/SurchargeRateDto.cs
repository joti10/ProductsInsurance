namespace Insurance.Api.Models
{
    public class SurchargeRateDto
    {
        public int id { get; set; }
        public int productTypeId { get; set; }
        public decimal surcharge { get; set; }
    }
}
