using System.ComponentModel.DataAnnotations;

namespace MembersUmbraco.Models;

public class RegisterConfirmationModel 
{
    
    [Required(ErrorMessage = "Please enter your email address")]  
    [DataType(DataType.EmailAddress)]  
    [Display(Name = "Email address")]  
    [MaxLength(50)]  
    [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]  
    public string Email { get; set; }
}