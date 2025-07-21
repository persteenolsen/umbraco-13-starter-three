
using Umbraco.Cms.Core.Composing;

namespace MembersUmbraco.Services;

    public class MailServiceComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
          //  builder.ManifestFilters().Append<EmailValidationManifestFilter>();

            builder.Services.AddScoped<IMemberMailService, MemberMailService>();

        }
    }