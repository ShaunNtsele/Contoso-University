using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ContosoUniversity.Infrastructure
{
    public class CustomUserValidator : UserValidator<IdentityUser>
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager,
            IdentityUser user)
        {
            IdentityResult result = await base.ValidateAsync(manager, user);
            List<IdentityError> errors = result.Succeeded ? new List<IdentityError>() :
                result.Errors.ToList();

            if (!string.IsNullOrEmpty(user.Email) && !user.Email.ToLower().EndsWith("@contoso.com"))
            {
                errors.Add(new IdentityError
                {
                    Code = "EmailDomainError",
                    Description = "Only contoso.com email addresses are allowed"
                });
            }
            return (errors.Count == 0 ?
              IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}
