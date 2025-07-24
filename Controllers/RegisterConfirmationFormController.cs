using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging; 
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;

using Umbraco.Cms.Core.Models;

using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

using Umbraco.Cms.Core.Security;
using System.Web;

using MembersUmbraco.Services;
using MembersUmbraco.Models;

namespace MembersUmbraco.Controllers;

public class RegisterConfirmationFormController : SurfaceController
{
     private readonly IMemberService _memberService;

    private readonly IMemberManager _memberManager;

     private readonly IMemberMailService _mailService;

    public RegisterConfirmationFormController(

        IMemberService memberService,
        IMemberManager memberManager,
        IMemberMailService mailService,

        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider)
        : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {

        _memberService = memberService;
        _memberManager = memberManager;
        _mailService = mailService;

    }

    [HttpPost]
    
    public async Task<IActionResult> Submit(RegisterConfirmationModel model)
    {
        
         TempData["RegisterConfirmationSent"] = null;

        if (!ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }
      
        TempData["Email"] = model.Email;
        
        // Try to get the Umbraco Member by the Email submitted
        var member = _memberService.GetByEmail(model.Email);
        
        // If there is  Member ...
        if (member != null)
        {
            // Try to get the Identity of the Member
            // var memberIdentity = await _memberManager.FindByIdAsync("1115");
            var memberIdentity = await _memberManager.FindByIdAsync(member.Id.ToString());

            // Try to generate a token by the Member Identity ....
            // 24-07-2025 - This token value needs to be saved in Cookie with a lifetime of 24 hours
            // var token = await _memberManager.GeneratePasswordResetTokenAsync(memberIdentity);
            var token = await _memberManager.GenerateEmailConfirmationTokenAsync(memberIdentity);
            
            // Adding a cookie with the token as value and a lifetime of 60 minuttes
            HttpContext.Response.Cookies.Append("ConfirmationToken", token, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(60),
                HttpOnly = true, // Accessible only by the server
                IsEssential = true // Required for GDPR compliance
            });

            // Encode the Token for transportation by Email
            var encodedToken = !string.IsNullOrEmpty(token) ? HttpUtility.UrlEncode(token) : string.Empty;

            // Try to an email to the Umbraco Member ....
            // await _mailService.SendResetPassword(member.Email, encodedToken);
            await _mailService.SendRegisterConfirmation(member.Email, encodedToken);

            TempData["MemberId"] = memberIdentity.Id;
            TempData["Token"] = token;
            TempData["EncodedToken"] = encodedToken;

            TempData["RegisterConfirmationSent"] = "sent";
        }
        else
           {
            TempData["RegisterConfirmationSent"] = null;
            TempData["Email"] += " is not in the system !";
           }


            return RedirectToCurrentUmbracoPage();
    }
}