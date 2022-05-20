using Insurance.Api.Controllers;
using System.Collections.Generic;

namespace Insurance.Api.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public decimal orderInsuranceValue { get; set; }
        public List<InsuranceDto> Products { get; set; }

    }
}
