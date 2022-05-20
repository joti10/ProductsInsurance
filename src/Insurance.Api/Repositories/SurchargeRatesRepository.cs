using Insurance.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Insurance.Api.Controllers;

namespace Insurance.Api.Repositories
{
    public class SurchargeRatesRepository : ISurchargeRatesRepository
    {
        private readonly ILogger<InsuranceController> logger;
        private const string baseAddress = "http://localhost:5002";
        private readonly HttpClient client;

        public SurchargeRatesRepository()
        {
            this.client = new HttpClient { BaseAddress = new Uri(baseAddress) }; ;

        }
        public async Task<SurchargeRateDto> UpdateSurchargeRates(List<SurchargeRateDto> surchargeRates)
        {
            var json = "";
            try
            {
                json = await client.GetAsync(string.Format("/surcharge/")).Result.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            var surchargerate = JsonConvert.DeserializeObject<SurchargeRateDto>(json);

            return surchargerate;

        }

        public async Task<SurchargeRateDto> GetSurchargeRate(int productTypeId)
        {
            var json = "";
            try
            {
                json = await client.GetAsync(string.Format("/surcharge/{0:G}", productTypeId)).Result.Content.ReadAsStringAsync();

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            var surchargerate = JsonConvert.DeserializeObject<SurchargeRateDto>(json);

            return surchargerate;

        }
    }
}

