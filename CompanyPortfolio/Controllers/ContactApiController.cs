using System.Threading.Tasks;
using CompanyPortfolio.Models;
using CompanyPortfolio.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CompanyPortfolio.Controllers
{
    [Route("api/contact")]
    [ApiController]
    [EnableCors("AllowedOrigins")]
    public class ContactApiController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<ContactApiController> _logger;

        public ContactApiController(IEmailService emailService, ILogger<ContactApiController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SendContactForm([FromBody] ContactFormModel model)
        {
            // Log that we received a contact form submission
            _logger.LogInformation("Received contact form submission from: {Email}", model.Email);

            // Validate the model - make sure required fields are provided
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for contact form");
                return BadRequest(new { success = false, message = "Please fill all required fields" });
            }

            // The EmailService will use the settings from appsettings.json to send the email
            // The user's message and contact details will be included in the email body
            var result = await _emailService.SendContactEmailAsync(model);

            if (result)
            {
                _logger.LogInformation("Successfully sent email for contact form from: {Email}", model.Email);
                return Ok(new { success = true, message = "Your message has been sent successfully!" });
            }
            else
            {
                _logger.LogError("Failed to send email for contact form from: {Email}", model.Email);
                return StatusCode(500, new { success = false, message = "Failed to send message. Please try again later." });
            }
        }
    }
}