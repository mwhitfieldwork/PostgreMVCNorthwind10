using System;
using System.Collections.Generic;
using NWCodeFirstMVC.Domain.Contracts;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;
using sib_api_v3_sdk.Client;
using Microsoft.Extensions.Configuration;
using Task = System.Threading.Tasks.Task;



namespace NWCodeFirstMVC.Infrastructure.Services
{
    public class EmailService: IEmailService
    {

        private readonly string _apiKey;
        private readonly string _sender;

        public EmailService(IConfiguration config)
        {
            _apiKey = config["BREVO_API_KEY"];
            Console.WriteLine($"BREVO_API_KEY loaded: {!string.IsNullOrEmpty(_apiKey)}");
            _sender = config["BREVO_SENDER"];
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            Configuration.Default.ApiKey["api-key"] = _apiKey;

            var apiInstance = new TransactionalEmailsApi();

            var sendSmtpEmail = new SendSmtpEmail(
                sender: new SendSmtpEmailSender(email: _sender),
                to: new List<SendSmtpEmailTo> { new SendSmtpEmailTo(to) },
                subject: subject,
                htmlContent: body
            );

            await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
        }

        public async Task SendEmailAsync(string to, string subject, string body, byte[] attachmentBytes, string attachmentName)
        {
            Configuration.Default.ApiKey["api-key"] = _apiKey;

            var apiInstance = new TransactionalEmailsApi();

            var attachments = new List<sib_api_v3_sdk.Model.SendSmtpEmailAttachment>();


            if (attachmentBytes != null)
            {
            attachments.Add(new sib_api_v3_sdk.Model.SendSmtpEmailAttachment(
                content: attachmentBytes,
                name: attachmentName
            ));

            }

            var sendSmtpEmail = new sib_api_v3_sdk.Model.SendSmtpEmail(
               sender: new sib_api_v3_sdk.Model.SendSmtpEmailSender(email: _sender),
               to: new List<sib_api_v3_sdk.Model.SendSmtpEmailTo> { new sib_api_v3_sdk.Model.SendSmtpEmailTo(to) },
               subject: subject,
               htmlContent: body,
               attachment: attachments
           );

            await apiInstance.SendTransacEmailAsync(sendSmtpEmail);

        }

    }
}
