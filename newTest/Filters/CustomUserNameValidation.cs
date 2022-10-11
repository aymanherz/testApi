using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text;

namespace testApi.Filters
{
    public class CustomUserNameValidation : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// OnActionExecuting -- validate the username (email) is valid email format
        /// Author: AH
        /// Date Created: Oct 08, 2022
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string username = ((testApi.Models.BeerRate)filterContext.ActionArguments["beerrate"]).username;

            if (!string.IsNullOrEmpty(username))
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(username);

                if (!match.Success)
                {

                    filterContext.Result = new BadRequestObjectResult("Email is not the right format");
                }
            }
        }

    }
}

