using System.ComponentModel.DataAnnotations;

namespace MembersUmbraco.Models;

public class ResetPasswordModel
{
    public string MemberId { get; set; }

    public string Token { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(15, ErrorMessage = "Must be between 10 and 15 characters", MinimumLength = 10)]
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [StringLength(15, ErrorMessage = "Must be between 10 and 15 characters", MinimumLength = 10)]
    [Compare("NewPassword")]
    public string ConfirmPassword { get; set; }
    

}