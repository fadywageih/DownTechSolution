
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Services.Specifications
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SmtpSettings> smtpSettings, ILogger<EmailService> logger)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailDto email)
        {
            try
            {
                if (string.IsNullOrEmpty(email.To))
                {
                    return;
                }

                if (string.IsNullOrEmpty(_smtpSettings.SenderEmail))
                {
                    return;
                }
                if (string.IsNullOrEmpty(_smtpSettings.Password))
                {
                    return;
                }

                var message = new MimeMessage();
                var senderName = string.IsNullOrEmpty(_smtpSettings.SenderName) ? "TechHub" : _smtpSettings.SenderName;
                message.From.Add(new MailboxAddress(senderName, _smtpSettings.SenderEmail));
                message.To.Add(new MailboxAddress("", email.To));
                message.Subject = email.Subject ?? "No Subject";
                message.Body = new TextPart("html")
                {
                    Text = email.Body ?? ""
                };
                using var client = new SmtpClient();
                _logger.LogInformation($"Connecting to SMTP server {_smtpSettings.Host}:{_smtpSettings.Port}");
                await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);
                _logger.LogInformation($"Authenticating as {_smtpSettings.UserName}");
                await client.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
                _logger.LogInformation($"Sending email to {email.To}");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                _logger.LogInformation("Email sent successfully to {Recipient}", email.To);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Recipient}", email.To);
            }
        }
    }
}