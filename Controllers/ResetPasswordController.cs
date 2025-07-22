
using MembersUmbraco.Models;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;


using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Web;

namespace MembersUmbraco.Controllers
{
   // public class EmployeeController : Controller
    public class ResetPasswordController : RenderController
   
    {
        public ResetPasswordController(ILogger<ResetPasswordController> logger, ICompositeViewEngine compositeViewEngine, IUmbracoContextAccessor umbracoContextAccessor)
        : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            
        }



        // [Route("/reset-password")]
        public IActionResult ResetPassword()
        //public override IActionResult Index()
        {
            int id = 0;

            // 18-07-2025: Note - take id + token param from this url 
            // https://localhost:44317/validate-password?id=5&token=xyz12eeell
            // There MUST be created a Document Type + Template with the names ResetPassword equal the 
            // name of the Action ResetPassword(). The Content created using the Document Type + Template
            string idS = HttpContext.Request.Query["id"].ToString();

            // 20-07-2025 - This Token value needs to be compared with the generted Token from
            // ForgotPassword saved in a Cookie with 24 hours lifetime !
            string tokenS = HttpContext.Request.Query["token"].ToString();

            Boolean res = false;
            int a;
            res = int.TryParse(idS, out a);
            if (res)
                id = int.Parse(idS);
            
            // Get the token saved in the Cookie to compare with the token from the Email
            string tokenFromCookieS = HttpContext.Request.Cookies["ResetToken"];

            // Decode the Token
            // Not needed
            // var decodedTokenS = !string.IsNullOrEmpty(tokenS) ? HttpUtility.UrlDecode(tokenS) : string.Empty;

            // The Id of the Member and the token can be values of hidden input in the Reset Password Form
            TempData["ResetPasswordId"] = idS;
            TempData["ResetPasswordToken"] = tokenS;

            // Check if the token from the Email match the token saved in the Cookie
            if (tokenS.Equals(tokenFromCookieS))
            {
                // Just for TEST
                // TempData["ResetPasswordTokenMatch"] = "You can reset your Password now ...";

                // Deleting a cookie - after testing the system
                HttpContext.Response.Cookies.Delete("ResetToken");

                return View("ResetPasswordForm");
            }
            else
            {
                TempData["ResetPasswordTokenMatch"] = "The token is not valid, try again ... ";
                
                // 20-07-2025 - The matching content node in Umbraco is needed !
                return Redirect("/forgotpassword");

                //return View("ForgotPasswordForm");
            }


        } 

    }
}
