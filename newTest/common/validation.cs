using System;
using System.Net;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;

namespace testApi.common
{
    /// <summary>
    /// Class Validation : to collect all validation methods 
    /// Author: AH
    /// Date Created: Oct 08, 2022
    /// 
    /// </summary>
    public static class validation
    {
        const string punkapiUrl = "https://api.punkapi.com/v2/beers/";
        
        /// <summary>
        /// isIdValid is a method to query the id against PunkAPI and if not found the validation fails
        /// Author: AH
        /// Date Created: Oct 08, 2022
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<bool> isIdValid(long id)
        {
            HttpClient client = new HttpClient();
            //  call punkApi and query the ID
            var response = await client.GetAsync(punkapiUrl + id);
            // if ID not found in PunkApi the response from the api return 404
            if (response.StatusCode == HttpStatusCode.NotFound)
              return false;
            return true;
         
        }

        /// <summary>
        /// isRateValid is a method to validate that the rate is between 1 to 5 
        /// Author: AH
        /// Date Created: Oct 08, 2022
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>
        public static Boolean isRateValid(int rating)
        {
            if (rating > 0 & rating <= 5) return true;

            return false;
        }
    }
}

