using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Xunit;
using Insurance.Api.Models;

namespace Insurance.Tests
{
    public class InsuranceAPiEndPointTests:IClassFixture<WebApplicationFactory<InsuranceAPiEndPointTests>>
    {
        private readonly HttpClient httpClient;
        private InsuranceDto GetSingleSamplePayload()
        {
            return new InsuranceDto
            {
                ProductId = 2,
                ProductTypeName = "Laptops",
                SalesPrice = 400,
                ProductTypeHasInsurance = true
            };
        }

        public InsuranceAPiEndPointTests(WebApplicationFactory<Api.Startup> factory)
        {
            httpClient = factory.CreateClient();
        }

        [Fact]
        public async void InsuranceController_WhenCalledWithCorrectPayload_GivesSuccessResult()
        {
            string REST_API_URL = "insurance/product";

            var request = new HttpRequestMessage(HttpMethod.Post, REST_API_URL)
            {
                Content = new StringContent(JsonSerializer.Serialize(GetSingleSamplePayload()), Encoding.UTF8, "application/json")
            };
            var response = await httpClient.SendAsync(request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

    }
}
