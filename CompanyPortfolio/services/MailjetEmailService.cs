using System;
using System.Threading.Tasks;
using CompanyPortfolio.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace CompanyPortfolio.Services
{
    public class MailjetEmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MailjetEmailService> _logger;

        public MailjetEmailService(IConfiguration configuration, ILogger<MailjetEmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> SendContactEmailAsync(ContactFormModel model)
        {
            try
            {
                // Create a new message
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], 
                    _configuration["EmailSettings:SenderEmail"]));
                message.To.Add(new MailboxAddress(_configuration["EmailSettings:ReceiverName"], 
                    _configuration["EmailSettings:ReceiverEmail"]));
                message.Subject = $"New contact from {model.FirstName} {model.LastName}";
                
                // Set reply-to address to the sender's email
                message.ReplyTo.Add(new MailboxAddress($"{model.FirstName} {model.LastName}", model.Email));

                // Create the message body
                var builder = new BodyBuilder
                {
                    TextBody = $"Name: {model.FirstName} {model.LastName}\nEmail: {model.Email}\nMessage: {model.Message}",
                    HtmlBody = $@"
                        <h3>New Contact Message</h3>
                        <p><strong>Name:</strong> {model.FirstName} {model.LastName}</p>
                        <p><strong>Email:</strong> {model.Email}</p>
                        <p><strong>Message:</strong></p>
                        <p>{model.Message}</p>
                    "
                };
                
                message.Body = builder.ToMessageBody();

                // Send the email using Mailjet SMTP
                using (var client = new SmtpClient())
                {
                    // Connect to Mailjet SMTP server
                    await client.ConnectAsync("in-v3.mailjet.com", 587, SecureSocketOptions.StartTls);
                    
                    // Authenticate with Mailjet
                    await client.AuthenticateAsync(
                        _configuration["EmailSettings:MailjetApiKey"], 
                        _configuration["EmailSettings:MailjetApiSecret"]);
                    
                    // Send the message
                    await client.SendAsync(message);
                    
                    // Disconnect from the server
                    await client.DisconnectAsync(true);
                }

                _logger.LogInformation("Email sent successfully via Mailjet SMTP");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while sending email via Mailjet SMTP");
                return false;
            }
        }
    }
}