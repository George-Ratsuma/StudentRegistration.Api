using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace StudentRegistration.Api.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendVerificationEmail(
            string recipientEmail,
            string firstName,
            string verificationLink)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(
                "Student Registration Portal",
                _configuration["Email:SmtpUsername"]));

            email.To.Add(
                MailboxAddress.Parse(recipientEmail));

            email.Subject = "Verify your Student Registration account";

            email.Body = new TextPart("html")
            {
                Text = $"""
                    <h2>Welcome to the Student Portal</h2>

                    <p>Hi {firstName},</p>

                    <p>
                        Thank you for registering.
                        Please click the link below to verify your email address.
                    </p>

                    <p>
                        <a href="{verificationLink}">
                            Verify Email
                        </a>
                    </p>

                    <p>
                        If you did not create this account,
                        you can ignore this email.
                    </p>
                    """
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                "smtp.gmail.com",
                587,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _configuration["Email:SmtpUsername"],
                _configuration["Email:SmtpPassword"]);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}