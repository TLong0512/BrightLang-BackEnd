namespace Infrastructure.Configurations;

public class EmailSettings
{
    public string SenderName { get; set; } = string.Empty; // must config at appsettings.json or Program.cs
    public string SenderEmail { get; set; } = string.Empty; // must config at appsettings.json or Program.cs
    public string SmtpHost { get; set; } = "smtp.gmail.com"; // Default SMTP host for Gmail
    public int SmtpPort { get; set; } = 587; // Default SMTP port for TLS
    //public bool UseSsl { get; set; } = true; // Use SSL for secure connection
    public string SmtpUser { get; set; } = string.Empty; // must config at appsettings.json or Program.cs
    public string SmtpPass { get; set; } = string.Empty; // must config at appsettings.json or Program.cs
}
