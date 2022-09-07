using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Portfolio.Core.Models.ViewModels;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Mail;
using Umbraco.Cms.Core.Models.Email;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace MyPortfolio.Controllers
{
    public class ContactSurfaceController : SurfaceController
    {
        private readonly ILogger<ContactSurfaceController> _logger;

        public ContactSurfaceController(IUmbracoContextAccessor umbracoContextAccessor, 
            IUmbracoDatabaseFactory databaseFactory, 
            ServiceContext services, 
            AppCaches appCaches, 
            IProfilingLogger profilingLogger, 
            IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Submit(ContactViewModel model)
        {
            if (!ModelState.IsValid) return CurrentUmbracoPage();

            TempData["Success"] = await SendEmail(model);

            return RedirectToCurrentUmbracoPage();
        }

        public async Task<bool> SendEmail(ContactViewModel model)
        {
            try
            {
                var client = new SmtpClient("smtp.mailtrap.io", 2525)
                {
                    Credentials = new NetworkCredential("a8217e65d54910", "0a6bd12838e06b"),
                    EnableSsl = true
                };
                client.Send("to@example.com", model.Email, model.Subject, model.Message);

                //_logger.LogInformation("Contact Form Submitted Successfully");
                return true;
            }
            catch (System.Exception ex)
            {
                //_logger.LogError(ex, "Error When Submitting Contact Form");
                return false;
            }
        }
    }
}