using Insurance.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Api.Repositories
{
    public interface ISurchargeRatesRepository
    {
        Task<SurchargeRateDto>GetSurchargeRate(int productTypeId);
        Task <SurchargeRateDto> UpdateSurchargeRates(List<SurchargeRateDto> surchargeRates);
    }
}
