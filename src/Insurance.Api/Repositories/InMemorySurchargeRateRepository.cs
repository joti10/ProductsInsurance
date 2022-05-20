using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.Models;

namespace Insurance.Api.Repositories
{
    public class InMemorySurchargeRateRepository
    {
        private readonly Dictionary<int, SurchargeRateDto> surchargeRates;

        public InMemorySurchargeRateRepository()
        {
            surchargeRates = new Dictionary<int, SurchargeRateDto>();
        }

        public void UpdateSurchargeRates(List<SurchargeRateDto> surchargeRates)
        {
            foreach (var surchargeRate in surchargeRates)
            {
                this.surchargeRates[surchargeRate.id] = surchargeRate;
            }
        }

        public SurchargeRateDto GetSurchageRate(int productTypeId)
        {
            if (surchargeRates.ContainsKey(productTypeId))
            {
                return surchargeRates[productTypeId];
            }

            return new SurchargeRateDto
            {
                id = 0,
                productTypeId = productTypeId,
                surcharge = 0
            };
        }
    }
}
