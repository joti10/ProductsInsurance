using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Insurance.Api.Controllers;
using Newtonsoft.Json;
using Insurance.Api.Models;
using Microsoft.Extensions.Logging;
using Insurance.Api.Repositories;
using NLog.Web;

namespace Insurance.Api.Services
{
    public class InsuranceService : IInsuranceService
    {

        private readonly ILogger<InsuranceController> logger;
        private readonly IProductRepository productRepository;
        private readonly ISurchargeRatesRepository surchargeRepository;

        public InsuranceService()
        {
            this.productRepository = new ProductRepository();
            this.surchargeRepository = new SurchargeRatesRepository();
        }

        public async Task<decimal> CalculateInsuranceCharge(InsuranceDto item)
        {
            logger.LogInformation("Get Product by Id : {0}", item.ProductId);

            var product = await productRepository.GetProduct(item.ProductId);
            var productType = await productRepository.GetProductTypes();


            logger.LogInformation("Initializing insurance value");
            decimal insurance = 0;

            if (product != null)
            {
                var salesPrice = product.salesPrice;
                var prodInsuredType = productType.Find(p => p.canBeInsured == true && p.id == product.id);

                var hasInsurance = prodInsuredType.canBeInsured;
                var productTypeName = prodInsuredType.name;
                logger.LogInformation("Found Product Name:{0},Sales Price: {1},ProductType: {2},Has Insurance:{3}", product.name, salesPrice, prodInsuredType.name, hasInsurance);

                if (!hasInsurance)
                    return insurance;

                logger.LogInformation("Setting insurance value depending on product type");
                if (salesPrice < 500)
                {
                    insurance = 0;
                    if (productTypeName == "Laptops" || productTypeName == "Smartphones")
                        insurance += 500;
                }
                else
                {
                    if (salesPrice > 500 && salesPrice < 2000)
                        insurance += 1000;
                    if (salesPrice >= 2000)
                        insurance += 2000;
                }

                logger.LogInformation("Summing surcharge with insurance value");
                var surcharge = await surchargeRepository.GetSurchargeRate(prodInsuredType.id);
                insurance += surcharge.surcharge;
            }

            logger.LogInformation("Total Insurance Value:{0}", insurance);
            return insurance;
        }


        public async Task<decimal> CalculateOrderInsurance(OrderDto order)
        {
            decimal orderInsurance = 0;
            decimal digiCamInsurance = 500;
            logger.LogInformation("Total Number of Products in Order : {0} Product(s) Found", order.Products.Count);

            if (order.Products.Count > 0)
            {
                var digiCamsList = order.Products.FindAll(x => x.ProductTypeName == "Digital Cameras");
                logger.LogInformation("Checking for digital camera in order : {0} Found", digiCamsList.Count);

                if (digiCamsList.Count > 0)
                    orderInsurance = digiCamInsurance;

                foreach (var prod in order.Products)
                {
                    var product = order.Products.Find(x => x.ProductId == prod.ProductId);
                    orderInsurance += await CalculateInsuranceCharge(product);
                }
            }
            logger.LogInformation("Total Order Insurance : {0}", orderInsurance);

            return orderInsurance;
        }



    }
}