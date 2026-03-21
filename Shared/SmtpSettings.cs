namespace Shared
{
    public class SmtpSettings
    {
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;  
        public string SenderEmail { get; set; } = string.Empty;
        public string SenderName { get; set; } = "TechHub"; 
    }
}
