using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using testApi.Filters;
using testApi.Models;

namespace testApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
        public class BeerRatesController : ControllerBase
    {
        private string FilePath = Directory.GetCurrentDirectory() + @"/Database/database.json";
        private List<BeerRate> BeerCollection = new List<BeerRate>();
        private readonly BeerRateContext _context;
        private string error;
        public BeerRatesController(IOptions<BeerRateContext> context)
        {
            // _context = context;
        }

        /// <summary>
        /// This method reads the contents of the database.json under database folder.
        /// </summary>
        private void readDataFromDatabaseJson()
        {
            // Read existing json data and De-serialize to object or create new list
            BeerCollection = JsonConvert.DeserializeObject<List<BeerRate>>(System.IO.File.ReadAllText(FilePath))
                                      ?? new List<BeerRate>();
        }

        
        /// <summary>
        /// Add Request body to the database.json file under database folder.
        /// </summary>
        /// <param name="beerrate"></param>        
        public  void AddToDatabasejson(BeerRate beerrate)
        {
            BeerCollection.Clear();
            try
            {

                readDataFromDatabaseJson();

                BeerCollection.Add(beerrate);

                var jsonData = JsonConvert.SerializeObject(BeerCollection);

                // add body request to Database.json file as requested in task 1
                System.IO.File.WriteAllText(FilePath, jsonData);
            }
            catch (Exception ex)
            {
                // something went wrong we can can handle exception here
                throw new Exception("Error  : " + ex.Message);
            }

        }


        [Route("GetBeers"), HttpGet]
        public async Task<ActionResult<IEnumerable<BeerRate>>> GetBeers()
        {
            return await _context.Beers.ToListAsync();
        }


        [Route("GetBeerRate/{id}"), HttpGet]
        public async Task<ActionResult<BeerRate>> GetBeerRate(long id)
        {
            var beerRate = await _context.Beers.FindAsync(id);

            if (beerRate == null)
            {
                return NotFound();
            }

            return beerRate;
        }
       

        public class userRating
        {
            public string? username { get; set; }
            public int rating { get; set; }
            public string? comment { get; set; }
        }

        public class BeerRate1
        {
            public long id { get; set; }
            public string? name { get; set; }
            public string? description { get; set; }
            public List<userRating>? userRatings { get; set; }


            }

            private List<BeerRate> updateuserRating(long id)
        {
            var result = BeerCollection.Where(x => x.id == id).ToList();

            var result1 = (from pro in result
                          select new  {  username = pro.username , rating = pro.rating, comment = pro.comments});


            return (List<BeerRate>)result1;
        }


        [Route("GetBeersByName/{BearName}"), HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> GetBeersByName(string BearName)
        {
            HttpClient client = new HttpClient();
            var searchResult = await client.GetStringAsync("https://api.punkapi.com/v2/beers?beer_name=" + BearName);

            BeerCollection.Clear();
            readDataFromDatabaseJson(); // this will read the json objects from database.json and load the data into the beerCollection list which is consider as in-memory data 

            var searchResultCollection = JsonConvert.DeserializeObject<List<dynamic>>(searchResult);


            JObject QueryToJSON = JObject.FromObject(new
            {
                // linq
                query = (from item in searchResultCollection
                         select new BeerRate1()
                         {
                             id = item.id,
                             name = item.name,
                             description = item.description,
                             userRatings = BeerCollection.Where(p => p.id.ToString() == (String)item.id).Select(st => new userRating { rating = st.rating, username = st.username, comment = st.comments }).ToList()
                         }).
                             Where(x => BeerCollection.Any(xx => xx.id == x.id))
            });

            string output = JsonConvert.SerializeObject(QueryToJSON);
            return Ok(output); //(IActionResult)QueryToJSON;
        }

        [CustomUserNameValidation]
        [Route("PutBeerRate/{id}"), HttpPut]
        public async Task<IActionResult> PutBeerRate(long id,[FromBody]BeerRate beerrate)  
        {
            beerrate.id = id;
            List<Error> errorList = new List<Error>();  // array to store the validation error messages if one of the validation fail


            if (!await common.validation.isIdValid(beerrate.id))  // validate ID
            {
                errorList.Add(new Error { error = "No ID Found" });
                errorList.Add(new Error { error = "This is an invalid ID"});
            }
         
            if (!common.validation.isRateValid(beerrate.rating)) // validate rating
               errorList.Add(new Error { error = "Rating must be between 1 and 5" });
            

            if (errorList.Count > 0) // if error list has one or more errors, then return all applicable erros to the user.. 
                return new BadRequestObjectResult(errorList); 

                // since it is valid request. Append the JSON from the request body  requirment 
                AddToDatabasejson(beerrate); 

                return Ok();
        }

     
    }
}
