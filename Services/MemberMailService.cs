using System.Net.Mail;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace MembersUmbraco.Services;

public class MemberMailService : IMemberMailService
{
    private readonly string _fromEmail;
    private readonly HostingSettings _hostingSettings;
    private readonly ILogger _logger;
    private readonly IEmailSender _emailSender;
    private readonly IHostingEnvironment _hostingEnvironment;
    private readonly ILocalizationService _localisation;
    private readonly IMemberService _memberService;


    public MemberMailService(IHostingEnvironment hostingEnvironment, IOptions<GlobalSettings> globalSettings, IEmailSender emailSender,
        ILocalizationService localisation, ILogger<MailMessage> logger, IMemberService memberService, IOptions<ContentSettings> contentSettings,
        IOptions<HostingSettings> hostingSettings)
    {

        _logger = logger;
        _emailSender = emailSender;
        _hostingEnvironment = hostingEnvironment;
        _localisation = localisation;
        _memberService = memberService;
        _hostingSettings = hostingSettings.Value;

        _fromEmail = globalSettings.Value.Smtp?.From != null ? globalSettings.Value.Smtp.From : contentSettings.Value.Notifications.Email;
    }

    // Forgot Password - Reset Password
    public async Task SendResetPassword(string email, string token)
    {
        try
        {
            var member = _memberService.GetByEmail(email);

            if (member != null)
            {

                // string baseURL =  _hostingEnvironment.ApplicationMainUrl.AbsoluteUri;

                // Note: Developement url
                // string baseURL = "https://localhost:44319";

                // Note: Production url
                string baseURL = "https://umb.members.persteenolsen.com";

                var resetUrl = baseURL + "/resetpassword?id=" + member.Id + "&token=" + token;

                var messageBody = "<p>Reset your Password by the link below:</p>";
                messageBody += "<p>Please click <a href='" + resetUrl + "'>here</a> to Reset your Password</p>";

                // Just for testing ...
                // messageBody += "<p>" + resetUrl + "</p>";
                // messageBody += "<p>&nnbsp;</p>";

                messageBody += "<p>Kind regards, The Umbraco Member Website<br/>";

                try
                {
                    // smtp (assuming you've set all this up)
                    // await _emailSender.SendAsync(message, emailType: "Contact");
                    await _emailSender.SendEmailAsync(email, "Reset Password", messageBody);

                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Failed to send the email - probably because email isn't configured for this site\n {0}", ex.ToString());
                }
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error {0}", ex.Message);
        }
    }
        

        // Register Confirmation - Send Confirmation Email
        public async Task SendRegisterConfirmation(string email, string token)
        {
            try
            {
                var member = _memberService.GetByEmail(email);
               
                if (member != null)
                 {

                // string baseURL =  _hostingEnvironment.ApplicationMainUrl.AbsoluteUri;

                // Note: Developement url
                // string baseURL = "https://localhost:44319";

                // Note: Production url
                string baseURL = "https://umb.members.persteenolsen.com";

                var resetUrl = baseURL + "/registerconfirmation?id=" + member.Id + "&token=" + token;

                var messageBody = "<p>Confirm your Registration by the link below:</p>";
                messageBody += "<p>Please click <a href='" + resetUrl + "'>here</a> to Confirm your Registration</p>";
                
                // Just for testing ...
                // messageBody += "<p>" + resetUrl + "</p>";
                // messageBody += "<p>&nnbsp;</p>";

                messageBody += "<p>Kind regards, The Umbraco Member Website<br/>";

                try
                {
                    // smtp (assuming you've set all this up)
                    // await _emailSender.SendAsync(message, emailType: "Contact");
                    await _emailSender.SendEmailAsync(email, "Confirm Registration", messageBody);

                }
                catch (Exception ex)
                {
                    _logger.LogInformation("Failed to send the email - probably because email isn't configured for this site\n {0}", ex.ToString());
                }
            }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error {0}",ex.Message);
            }
        }
    }
