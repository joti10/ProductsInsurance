using Insurance.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Insurance.Api.Services
{
    public interface IInsuranceService
    {
        Task<decimal> CalculateInsuranceCharge(InsuranceDto insured);
        Task<decimal> CalculateOrderInsurance(OrderDto order);
    }
}
