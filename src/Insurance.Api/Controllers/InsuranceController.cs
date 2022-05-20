using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Insurance.Api.Models;
using Insurance.Api.Repositories;
using Insurance.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Insurance.Api.Controllers
{
    public class InsuranceController : Controller
    {
        private readonly ILogger<InsuranceController> logger;
        private readonly ISurchargeRatesRepository surchargeRepository;
        private readonly IInsuranceService insuranceService;

        public InsuranceController(IInsuranceService insuranceService, ISurchargeRatesRepository surchargeRepository)
        {
            this.insuranceService = insuranceService;
            this.surchargeRepository = surchargeRepository;
        }

        [HttpPost]
        [Route("api/insurance/product")]
        public async Task<InsuranceDto> CalculateInsurance([FromBody] InsuranceDto toInsure)
        {
            try
            {
                var insuranceCharge = await insuranceService.CalculateInsuranceCharge(toInsure);

                toInsure.InsuranceValue = (float)insuranceCharge;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return toInsure;
        }
        [HttpPost]
        [Route("api/insurance/order")]
        public async Task<OrderDto> GetOrderInsurance([FromBody] OrderDto order)
        {
            try
            {
                var insuranceCharge = await insuranceService.CalculateOrderInsurance(order);

                order.orderInsuranceValue = insuranceCharge;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return order;
        }

        [HttpPost]
        [Route("api/insurance/uploadsurchargerates")]
        public HttpResponseMessage UploadSurchargeRates([FromBody] List<SurchargeRateDto> surchargeRates)
        {
            try
            {
                this.surchargeRepository.UpdateSurchargeRates(surchargeRates);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }

}