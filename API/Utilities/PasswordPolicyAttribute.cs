using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace API.Utilities;

public class PasswordPolicyAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var password = (string)value;
        if (password is null)
        {
            return false;
        }

        var hasMinimumChars = new Regex(@".{6,}");
        var hasNumber = new Regex(@"[0-9]+");
        var hasSymbols = new Regex(@"[!@#$%^&*()_+=-\[{\]};:<>|./?,~]");
        var hasUpperCase = new Regex(@"[A-Z]+");
        var hasLowerCase = new Regex(@"[a-z]+");

        var isValidated = hasMinimumChars.IsMatch(password)&&
            hasNumber.IsMatch(password)&&
            hasSymbols.IsMatch(password)&&
            hasUpperCase.IsMatch(password)&&
            hasLowerCase.IsMatch(password);

        ErrorMessage = "Password must contain atleast 6 character, "
            + "1 Number, " + "1 Symbol, " + "1 Uppercase & Lowercase";

        return isValidated;

    }
}
