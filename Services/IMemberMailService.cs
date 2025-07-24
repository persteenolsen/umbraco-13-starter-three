

namespace MembersUmbraco.Services;

public interface IMemberMailService
{
    Task SendResetPassword(string email, string token);
        
    Task SendRegisterConfirmation(string email, string token);
    }

