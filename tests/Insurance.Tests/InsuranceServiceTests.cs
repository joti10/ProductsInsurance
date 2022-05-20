using Insurance.Api.Controllers;
using Insurance.Api.Models;
using Insurance.Api.Repositories;
using Insurance.Api.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace Insurance.Tests
{
    public class InsuranceServiceTests
    {
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
        private InsuranceDto GetLessSalesPriceSamplePayload()
        {
            return new InsuranceDto
            {
                ProductId = 3,
                ProductTypeName = "SLR cameras",
                SalesPrice = 300,
                ProductTypeHasInsurance = true
            };
        }

        private List<InsuranceDto> productList()
        {
            var prods = new List<InsuranceDto>();
            var prod = new InsuranceDto()
            {
                ProductId = 1,
                ProductTypeName = "SLR cameras",
                SalesPrice = 300,
                ProductTypeHasInsurance = true
            };
            var prod1 = new InsuranceDto()
            {
                ProductId = 2,
                ProductTypeName = "Laptops",
                SalesPrice = 300,
                ProductTypeHasInsurance = true
            };


            prods.Add(prod);
            prods.Add(prod1);
            return prods;
        }


        private List<InsuranceDto> productsWithCameraList()
        {
            var prods = new List<InsuranceDto>();
            var prod = new InsuranceDto()
            {
                ProductId = 1,
                ProductTypeName = "SLR cameras",
                SalesPrice = 300,
                ProductTypeHasInsurance = true
            };
            var prod1 = new InsuranceDto()
            {
                ProductId = 2,
                ProductTypeName = "Laptops",
                SalesPrice = 300,
                ProductTypeHasInsurance = true
            };

            var prod2 = new InsuranceDto()
            {
                ProductId = 3,
                ProductTypeName = "Digital Cameras",
                SalesPrice = 600,
                ProductTypeHasInsurance = true
            };

            prods.Add(prod);
            prods.Add(prod1);
            prods.Add(prod2);
            return prods;
        }
        private OrderDto orderSample()
        {
            return new OrderDto
            {
                Id = 1,
                Products = productList()

            };
        }

        private OrderDto orderSampleWithDigitalCamera()
        {
            return new OrderDto
            {
                Id = 1,
                Products = productsWithCameraList()

            };
        }
        [Fact]
        public async void CalculateInsuranceCharge_GivenSalesPriceLessThan500AndIsNotLaptopOrSmartPhone()
        {
            decimal expectedInsuranceValue = 0;

            InsuranceDto insurancedto = GetLessSalesPriceSamplePayload();

            var mockInsuranceService = new Mock<IInsuranceService>();
            mockInsuranceService.Setup(p => p.CalculateInsuranceCharge(insurancedto)).ReturnsAsync(0);

            var mockSurchargeRates = new Mock<ISurchargeRatesRepository>();

            InsuranceController insurancecontroller = new InsuranceController(mockInsuranceService.Object, mockSurchargeRates.Object);
            var result = await insurancecontroller.CalculateInsurance(insurancedto);
            Assert.Equal(
                expected: expectedInsuranceValue,
                actual: (decimal)result.InsuranceValue
            );
        }
        [Fact]
        public async void CalculateInsuranceCharge_GivenSalesPriceLessThan500AndProductIsLaptopOrSmartPhone()
        {
            decimal expectedInsuranceValue = 500;

            InsuranceDto insurancedto = GetSingleSamplePayload();

            var mockInsuranceService = new Mock<IInsuranceService>();
            mockInsuranceService.Setup(p => p.CalculateInsuranceCharge(insurancedto)).ReturnsAsync(500);

            var mockSurchargeRates = new Mock<ISurchargeRatesRepository>();

            InsuranceController insurancecontroller = new InsuranceController(mockInsuranceService.Object, mockSurchargeRates.Object);
            var result = await insurancecontroller.CalculateInsurance(insurancedto);

            Assert.Equal(expectedInsuranceValue, (decimal)result.InsuranceValue);


        }



        [Fact]
        public async void CalculateOrderInsurance_GivenSalesPriceLessThan500AndHasLaptops()
        {
            decimal expectedInsuranceValue = 500;

            OrderDto orderdto = orderSample();

            var mockInsuranceService = new Mock<IInsuranceService>();
            mockInsuranceService.Setup(p => p.CalculateOrderInsurance(orderdto)).ReturnsAsync(500);

            var mockSurchargeRates = new Mock<ISurchargeRatesRepository>();

            InsuranceController insurancecontroller = new InsuranceController(mockInsuranceService.Object, mockSurchargeRates.Object);
            var result = await insurancecontroller.GetOrderInsurance(orderdto);

            Assert.Equal(expectedInsuranceValue, result.orderInsuranceValue);

        }

        [Fact]
        public async void CalculateOrderInsurance_GivenAtLeastOneDigitalCamera()
        {
            decimal expectedInsuranceValue = 2000;

            OrderDto orderdto = orderSampleWithDigitalCamera();

            var mockInsuranceService = new Mock<IInsuranceService>();
            mockInsuranceService.Setup(p => p.CalculateOrderInsurance(orderdto)).ReturnsAsync(500);

            var mockSurchargeRates = new Mock<ISurchargeRatesRepository>();

            InsuranceController insurancecontroller = new InsuranceController(mockInsuranceService.Object, mockSurchargeRates.Object);
            var result = await insurancecontroller.GetOrderInsurance(orderdto);

            Assert.Equal(expectedInsuranceValue, result.orderInsuranceValue);
        }

        private InsuranceDto GetSamplePayload_ProductWithSurcharge()
        {
            return new InsuranceDto
            {
                ProductId = 1
            };
        }
        [Fact]
        public async void CalculateInsuranceCharge_GivenProductWithSurchargeISLaptop()
        {
            decimal expectedInsuranceValue = 1600;
            var insurancedto = new InsuranceDto
            {
                ProductId = 1,
                ProductTypeName = "Laptops",
                ProductTypeHasInsurance = true,
                Surcharge=100
            };
            
            var mockInsuranceService = new Mock<IInsuranceService>();
            mockInsuranceService.Setup(p => p.CalculateInsuranceCharge(insurancedto)).ReturnsAsync(1600);

            var mockSurchargeRates = new Mock<ISurchargeRatesRepository>();

            InsuranceController insurancecontroller = new InsuranceController(mockInsuranceService.Object, mockSurchargeRates.Object);
            var result = await insurancecontroller.CalculateInsurance(insurancedto);

            Assert.Equal(expectedInsuranceValue, (decimal)result.InsuranceValue);
        }
    }
}
