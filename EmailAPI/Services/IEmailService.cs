using EmailAPI.Models;

namespace EmailAPI.Services {
    public interface IEmailService {
        Task<IResult> SendEmailAsync(EmailDTO request);
    }
}
