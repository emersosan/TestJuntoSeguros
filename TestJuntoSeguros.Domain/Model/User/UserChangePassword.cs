using System.ComponentModel.DataAnnotations;

namespace TestJuntoSeguros.Domain.Model.User
{
  public class UserChangePassword
  {
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string NewPassword { get; set; }

    [Compare("Password")]
    public string ConfirmPassword { get; set; }

    public string OldPassword { get; set; }
  }
}
