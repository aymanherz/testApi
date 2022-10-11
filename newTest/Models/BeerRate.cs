using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace testApi.Models
{
    public class BeerRate
    {
        //public void BeerRate1(string x1,string gx2, string x3)
        //{

        //}
        public long id { get; set; }
        public string? username { get; set; }
        public int rating { get; set; }
        public string? comments { get; set; }
    }
}
