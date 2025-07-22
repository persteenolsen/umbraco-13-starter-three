using Microsoft.AspNetCore.Mvc;
using MembersUmbraco.Models;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging; 
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Cms.Core.Security;

namespace MembersUmbraco.Controllers;

public class ResetPasswordFormController : SurfaceController
{
    private readonly IMemberService _memberService;
   private readonly IMemberManager _memberManager;

    public ResetPasswordFormController(

        IMemberService memberService,
        IMemberManager memberManager,

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

    }

    [HttpPost]
   // public IActionResult Submit(ResetPasswordModel model)
    public async Task<IActionResult> Submit(ResetPasswordModel model)
    {
       
        TempData["ResetStatus"] = null;

        if (!ModelState.IsValid)
        {
            TempData["ResetStatus"] += "Password is not valid + ";

            // return CurrentUmbracoPage();
            return Redirect("/forgotpassword");
        }

        TempData["MemberId"] = model.MemberId;
        TempData["Token"] = model.Token;
        TempData["NewPassword"] = model.NewPassword;
        TempData["ConfirmPassword"] = model.ConfirmPassword;

        var validPassword = await _memberManager.ValidatePasswordAsync(model.NewPassword);
        if (!validPassword.Succeeded)
        {
            //ModelState.AddModelError("NoPass", "Password is not valid");
            TempData["ResetStatus"] += "Password is not valid + ";
         }
        if (string.IsNullOrWhiteSpace(model.NewPassword))
          {
            // ModelState.AddModelError("NoPass", "You must enter a password");
            TempData["ResetStatus"] += "You must enter a password + "; 
           }
        if (model.NewPassword != model.ConfirmPassword)
        {
            // ModelState.AddModelError("NoMatch", "passwords do not match");
            TempData["ResetStatus"] += "Passwords do not match + ";
        }

         if (TempData["ResetStatus"] != null )
        {
            TempData["ResetStatus"] += "Password Reset - Input Error - try again !";

            return Redirect("/forgotpassword");
         }

        var identityUser = _memberManager.FindByIdAsync(model.MemberId).Result;
        var result =  _memberManager.ResetPasswordAsync(identityUser, model.Token, model.NewPassword).Result;

        if (!result.Succeeded)
        {
            TempData["ResetStatus"] += "Password Reset - Member Error - try again !";

            return Redirect("/forgotpassword");
         }


        // If the Password was changed, redirect to login page
        // 20-07-2025 - Could Redirect to the Protected Page as there may be a Automated Login after
        // a Successfull change of Password ?
        return Redirect("/login");
        
        // Just for testing the submitted is ok
        // return Redirect("/forgotpassword");
    }
}