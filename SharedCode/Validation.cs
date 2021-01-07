using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode
{

    public partial class Login : IValidatableObject
    {
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (Username.Length < 1 || Username.Length > 20)
                {
                    yield return new ValidationResult(
                        "Username must be between 1 and 20 characters",
                        new[] {nameof(Username)});
                }
            }
        }
}
