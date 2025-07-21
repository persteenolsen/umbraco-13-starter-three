namespace MembersUmbraco.Models;

public class ResetPasswordModel
{
    public string MemberId { get; set; }

    public string Token { get; set; }

    public string NewPassword { get; set; }

    public string ConfirmPassword { get; set; }
    

}