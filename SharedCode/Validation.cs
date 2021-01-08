using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedCode
{

    public partial class UserType : IValidatableObject
    {
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (Description.Length < 1 || Description.Length > 20)
                {
                    yield return new ValidationResult(
                        "Description must be between 1 and 20 characters",
                        new[] {nameof(Description)});
                }
            }
        }
}
