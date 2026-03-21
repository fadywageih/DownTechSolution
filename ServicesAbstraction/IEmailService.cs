using Shared.Dtos.Email;

namespace ServicesAbstraction
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailDto email);
    }
}
