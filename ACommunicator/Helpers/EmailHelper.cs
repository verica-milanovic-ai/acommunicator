using System.Net.Mail;
using ACommunicator.Properties;

namespace ACommunicator.Helpers
{
    public static class EmailHelper
    {
        private static readonly SmtpClient EmailClient = new SmtpClient();

        public static void SendWelcomeMail(string emailRecipient)
        {
            if (string.IsNullOrEmpty(emailRecipient))
                return;

            var message = new MailMessage
            {
                From = new MailAddress(Resources.SenderEmailAddress),
                Subject = Resources.WelcomeMailSubject,
                Body = Resources.WelcomeMailBody
            };
            message.To.Add(emailRecipient);

            EmailClient.Send(message);
        }
    }
}