using Xunit;
using testApi.Models;
using System.Security.Cryptography.Xml;
using System.Numerics;
using Xunit;
using Moq;
using testApi.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using testApi.common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace TestProject1
{
    public class UnitTest1
    {
        private readonly Mock<IOptions<BeerRateContext>> _dbContext;
        

        public UnitTest1()
        {
            this._dbContext = new Mock<IOptions<BeerRateContext>>();
            
        }

        [Fact]
        public async void Test1()
        {            
            BeerRate beerRate = new BeerRate() {
                id = 190,
                username = "user@user.com",
                rating=5,
            comments= "comments"
            };
                       
            BeerRatesController beerRatesController = new BeerRatesController(this._dbContext.Object);

            //Mock validation//

            // I should have created this AddToDatabasejson logic in BL layer and mock it here but I will keep it like this for now
            //Mock<BeerRatesController> _mocked = new Mock<BeerRatesController>();
            //_mocked.Setup(fun => fun.AddToDatabasejson(beerRate));
            //_mocked.Verify(x => x.AddToDatabasejson(beerRate), Times.Once());            

            IActionResult acr = await beerRatesController.PutBeerRate(4, beerRate);
            Assert.IsNotType<OkObjectResult>(acr);            

        }
    }
}