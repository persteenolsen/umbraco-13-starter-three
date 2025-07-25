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

public class DeleteProfileFormController : SurfaceController
{
     private readonly IMemberService _memberService;
    public DeleteProfileFormController(

        IMemberService memberService,

        IUmbracoContextAccessor umbracoContextAccessor,
        IUmbracoDatabaseFactory databaseFactory,
        ServiceContext services,
        AppCaches appCaches,
        IProfilingLogger profilingLogger,
        IPublishedUrlProvider publishedUrlProvider)
        : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
    {

        _memberService = memberService;

    }

    [HttpPost]
    
    public async Task<IActionResult> Submit(DeleteProfileModel model)
    {
        
        if (!ModelState.IsValid)
        {
            return CurrentUmbracoPage();
        }
      
        TempData["Email"] = model.Email;
        
        // Try to get the Umbraco Member by the Email submitted
        var member = _memberService.GetByEmail(model.Email);

        // Try to get the Umbraco Member by the Email submitted
        // var member = _memberService.GetById(model.Id);
        
        // If there is  Member ...
        if (member != null)
        {
           // TEST - Working - Deleting Hans with Id 1078
            var memberToDelete = _memberService.GetById(member.Id);
            _memberService.Delete(memberToDelete);

            // Force a Logout of the Deleted Member the hard way by deleting the Cookie
            // that Umbraco uses for keep a Member Logged In
            // The Deleted Member will be Redirected to the Login Page
            // Note: I am sure there must be a more "friendly" Umbraco way :-)
            HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");

            TempData["ProfileDeleted"] = "Your Profile was successfully deleted!";
        }
        else
        {
            TempData["ProfileDeleted"] = "Your Profile was not deleted!";
        }


        return RedirectToCurrentUmbracoPage();
    }
}