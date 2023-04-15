using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;

namespace ProyectoIdentity.Services
{
    public class MailJetEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public OptionsMailJet _optionsMailJet;
        public MailJetEmailSender(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _optionsMailJet = _configuration.GetSection("MailJet").Get<OptionsMailJet>();

            MailjetClient client = new MailjetClient(_optionsMailJet.Apikey, _optionsMailJet.Secretkey);

            var myEmail = new TransactionalEmailBuilder()
                .WithFrom(new SendContact("julianlondono@outlook.com"))
                .WithSubject(subject)
                .WithHtmlPart(htmlMessage)
                .WithTo(new SendContact(email))
                .Build();

            var response = await client.SendTransactionalEmailAsync(myEmail); ;
        }
    }
}
