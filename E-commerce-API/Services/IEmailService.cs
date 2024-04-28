using E_commerce_API.DataModel;

namespace E_commerce_API.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO emailDto);
    }
}
