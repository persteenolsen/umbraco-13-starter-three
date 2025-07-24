
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
using Umbraco.Cms.Core.Services;
// using Umbraco.Cms.Core.Security;

namespace MembersUmbraco.Controllers
{
   // public class EmployeeController : Controller
    public class RegisterConfirmationController : RenderController
       {
        
        private readonly IMemberService _memberService;

       // private readonly IMemberManager _memberManager;

        public RegisterConfirmationController(

            IMemberService memberService,
           // IMemberManager memberManager,
            ILogger<RegisterConfirmationController> logger,
            ICompositeViewEngine compositeViewEngine,
            IUmbracoContextAccessor umbracoContextAccessor

            )
        : base(logger, compositeViewEngine, umbracoContextAccessor)
        {
            _memberService = memberService;
           // _memberManager = memberManager;
        }



        // [Route("/register-confirmation")]
        // public IActionResult RegisterConfirmation()
        public async Task<IActionResult> RegisterConfirmation()
        //public override IActionResult Index()
        {
            int id = 0;

            // 18-07-2025: Note - take id + token param from this url 
            // https://localhost:44317/register-confirmation?id=5&token=xyz12eeell
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
            string tokenFromCookieS = HttpContext.Request.Cookies["ConfirmationToken"];

            // Decode the Token
            // Not needed
            // var decodedTokenS = !string.IsNullOrEmpty(tokenS) ? HttpUtility.UrlDecode(tokenS) : string.Empty;

            // The Id of the Member and the token can be values of hidden input in the Reset Password Form
            TempData["RegisterConfirmationId"] = idS;
            TempData["RegisterConfirmationToken"] = tokenS;

            // Check if the token from the Email match the token saved in the Cookie
            if (tokenS.Equals(tokenFromCookieS))
            {
                // Just for TEST
                // TempData["RegisterConfirmationTokenMatch"] = "You can Confirm your Registration now ...";

                // Deleting a cookie - after testing the system
                HttpContext.Response.Cookies.Delete("ConfirmationToken");

                 // Try to get the Umbraco Member by the Email submitted
                var member = _memberService.GetById(id);
                                           
                // If there is  Member ...
                if (member != null)
                {
                    
                    // Aprove the Member
                    member.IsApproved = true;
                   
                   // Add the Member to the Preminum - which needs to be Create in the Umbraco Backend
                   _memberService.AssignRole(member.Id, "Premium");

                   // Save the Member settings
                   _memberService.Save(member);

                 
                }

                // return View("RegisterConfirmationForm");
                return Redirect("/login");
            }
            else
            {
                TempData["RegisterConfirmationTokenMatch"] = "The token is not valid, try again ... ";

                // 24-07-2025 - The matching content node in Umbraco is needed !
                 return Redirect("/register-confirmation-form");
                
            }


        } 

    }
}
