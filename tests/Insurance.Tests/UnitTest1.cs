using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Insurance.Api.Controllers;
using Insurance.Api.Models;
using Insurance.Api.Repositories;
using Insurance.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Xunit;
using Moq;

namespace Insurance.Tests
{
    public class InsuranceTests : IClassFixture<ControllerTestFixture>
    {
        private readonly ControllerTestFixture _fixture;
        private readonly ISurchargeRatesRepository surchargeRepository;
        private readonly IInsuranceService insuranceService;
        public InsuranceTests(ControllerTestFixture fixture)
        {
            _fixture = fixture;
            this.insuranceService = new InsuranceService();

        }

        [Fact]
        public async void CalculateInsurance_GivenSalesPriceBetween500And2000Euros_ShouldAddThousandEurosToInsuranceCost()
        {
            float expectedInsuranceValue = 1000;

            var insurancedto = new InsuranceDto
            {
                ProductId = 1,
            };
            var resDto = new InsuranceDto { InsuranceValue = 1000, };

            var mockInsuranceService = new Mock<IInsuranceService>();
            mockInsuranceService.Setup(p => p.CalculateInsuranceCharge(insurancedto)).ReturnsAsync(1000);

            var mockSurchargeRates = new Mock<ISurchargeRatesRepository>();

            InsuranceController ins = new InsuranceController(mockInsuranceService.Object, mockSurchargeRates.Object);
            var result = await ins.CalculateInsurance(insurancedto);
            Assert.Equal(expectedInsuranceValue, insurancedto.InsuranceValue);
        }


    }

    public class ControllerTestFixture : IDisposable
    {
        private readonly IHost _host;

        public ControllerTestFixture()
        {
            _host = new HostBuilder()
                   .ConfigureWebHostDefaults(
                        b => b.UseUrls("http://localhost:5002")
                              .UseStartup<ControllerTestStartup>()
                    )
                   .Build();

            _host.Start();
        }

        public void Dispose() => _host.Dispose();
    }

    public class ControllerTestStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(
                ep =>
                {
                    ep.MapGet(
                        "products/{id:int}",
                        context =>
                        {
                            int productId = int.Parse((string)context.Request.RouteValues["id"]);
                            var product = new
                            {
                                id = productId,
                                name = "Test Product",
                                productTypeId = 1,
                                salesPrice = 750
                            };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                        }
                    );
                    ep.MapGet(
                        "product_types",
                        context =>
                        {
                            var productTypes = new[]
                                               {
                                                   new
                                                   {
                                                       id = 1,
                                                       name = "Test type",
                                                       canBeInsured = true
                                                   },
                                                   new
                                                   {
                                                       id = 2,
                                                       name = "Laptops",
                                                       canBeInsured = true
                                                   },
                                               };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(productTypes));
                        }
                    );
                    ep.MapGet(
                               "product_withsurcharge/{id:int}",
                               context =>
                               {
                                   int productId = int.Parse((string)context.Request.RouteValues["id"]);
                                   var product = new
                                   {
                                       id = productId,
                                       name = "Dell Laptop",
                                       productTypeId = 2,
                                       salesPrice = 750,
                                       surcharge = 100
                                   };
                                   return context.Response.WriteAsync(JsonConvert.SerializeObject(product));
                               }
                                      );

                }
            );
        }
    }
}