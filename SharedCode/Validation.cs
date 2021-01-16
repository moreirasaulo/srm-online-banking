using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharedCode
{
    //Login
    public partial class Login : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Username.Length < 5 || Username.Length > 20)
            {
                yield return new ValidationResult(
                        "Username must be made of 5 to 20 characters",
                        new[] { nameof(Username) });
            }
            if (Password.Length < 8 || Username.Length > 20)
            {
                yield return new ValidationResult(
                        "Password must be made of 8 to 20 characters",
                        new[] { nameof(Password) });
            }
        }
    }


    //User
    public partial class User : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName.Length < 1 || FirstName.Length > 20)
            {
                yield return new ValidationResult(
                        "First name must contain between 1 and 20 characters",
                        new[] { nameof(FirstName) });
            }
            if (MiddleName.Length > 20)
            {
                yield return new ValidationResult(
                        "Middle name must containt not more than 20 characters",
                        new[] { nameof(MiddleName) });
            }
            if (LastName.Length < 1 || LastName.Length > 20)
            {
                yield return new ValidationResult(
                       "Last name must containt between 1 and 20 characters",
                       new[] { nameof(LastName) });
            }
            if (Gender.ToLower() != "male" && Gender.ToLower() != "female" && Gender.ToLower() != "other")
            {
                yield return new ValidationResult(
                       "Please choose customer's gender",
                       new[] { nameof(Gender) });
            }
            if (NationalId.Length < 5 || NationalId.Length > 20)
            {
                yield return new ValidationResult(
                       "National Id/Company registration Id number must containt between 5 and 20 characters",
                       new[] { nameof(NationalId) });
            }
            if (DateOfBirth == null)
            {
                yield return new ValidationResult(
                      "Please select date of birth/date of company registration",
                      new[] { nameof(DateOfBirth) });
            }
            if (DateOfBirth > DateTime.Now)
            {
                yield return new ValidationResult(
                      "Date of birth/company registration date must be earlier than today's date",
                      new[] { nameof(DateOfBirth) });
            }
            if (!Regex.IsMatch(PhoneNo, @"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$"))
            {
                yield return new ValidationResult(
                        "Please enter valid phone number xxx-xxx-xxxx",
                        new[] { nameof(PhoneNo) });
            }
            if (Address.Length < 5 || Address.Length > 50)
            {
                yield return new ValidationResult(
                        "Address must contain between 5 and 50 caracters",
                        new[] { nameof(Address) });
            }
            if (City.Length < 2 || City.Length > 20)
            {
                yield return new ValidationResult(
                       "City must contain between 2 and 20 caracters",
                       new[] { nameof(City) });
            }
            if (ProvinceState.Length < 2 || ProvinceState.Length > 20)
            {
                yield return new ValidationResult(
                       "Province or State must be between 2 and 20 caracters",
                       new[] { nameof(ProvinceState) });
            }
            if (PostalCode.Length < 5 || PostalCode.Length > 10)
            {
                yield return new ValidationResult(
                       "Postal code must be made of 5 to 10 characters",
                       new[] { nameof(PostalCode) });
            }
            if (Country != "Canada" && Country != "USA")
            {
                yield return new ValidationResult(
                       "Country must be Canada or USA",
                       new[] { nameof(Country) });
            }
            if (Email != null && Email.Length > 60)
            {
                yield return new ValidationResult(
                        "E-mail must contain maximum 60 characters",
                        new[] { nameof(Email) });
            }
            if (CompanyName != null && CompanyName.Length > 70)
            {
                yield return new ValidationResult(
                        "Company name must not contain more than 70 characters",
                        new[] { nameof(CompanyName) });
            }
        }
    }

    // Account
    public partial class Account : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (OpenDate > DateTime.Today)
            {
                yield return new ValidationResult(
                        "Open date cannot be later than today's date",
                        new[] { nameof(OpenDate) });
            }
            if (AccountTypeId != 1 && AccountTypeId != 2 && AccountTypeId != 3 && AccountTypeId != 4)
            {
                yield return new ValidationResult(
                        "Account type can be only 'Checking', 'Savings', 'Investment' or 'Business'",
                        new[] { nameof(AccountTypeId) });
            }
            if (CloseDate != null && CloseDate < OpenDate)
            {
                yield return new ValidationResult(
                        "Account closing date cannot be earlier that account open date",
                        new[] { nameof(CloseDate), nameof(OpenDate) });
            }
            if ((AccountTypeId == 1 || AccountTypeId == 4) && MonthlyFee == null)
            {
                yield return new ValidationResult(
                        "Monthly fee must be set for Checking/Business account type",
                        new[] { nameof(AccountTypeId), nameof(MonthlyFee) });
            }
            if ((AccountTypeId == 1 || AccountTypeId == 4) && MonthlyFee != null && (MonthlyFee <= 4 || MonthlyFee > 50))
            {
                yield return new ValidationResult(
                        "Monthly fee must be between 4$ and  50 $",
                        new[] { nameof(AccountTypeId), nameof(MonthlyFee) });
            }
            if ((AccountTypeId == 2 || AccountTypeId == 3) && Interest == null)
            {
                yield return new ValidationResult(
                        "Interest must be set for Savings/Investment account type",
                        new[] { nameof(AccountTypeId), nameof(MonthlyFee) });
            }
            if ((AccountTypeId == 2 || AccountTypeId == 3) && Interest != null && (Interest <= (decimal)0.5 || Interest > 10))
            {
                yield return new ValidationResult(
                        "Interest must be between 0.5 % and 10 %",
                        new[] { nameof(AccountTypeId), nameof(Interest) });
            }
        }
    }

    // Transaction
    public partial class Transaction : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Date > DateTime.Today)
            {
                yield return new ValidationResult(
                        "Transaction date cannot be later than today's date",
                        new[] { nameof(Date) });
            }
            if (Amount <= 0)
            {
                yield return new ValidationResult(
                        "Transaction amount cannot be 0 or negative",
                        new[] { nameof(Amount) });
            }
            if ((Type == "Transfer" || Type == "Payment") && ToAccount == null)
            {
                yield return new ValidationResult(
                        "Destination account must not be null",
                        new[] { nameof(Type), nameof(ToAccount), nameof(AccountId) });
            }
            if ((Type == "Transfer" || Type == "Payment") && ToAccount != null && AccountId == ToAccount)
            {
                yield return new ValidationResult(
                        "Transfer of money withing the same account is prohibited",
                        new[] { nameof(Type), nameof(ToAccount), nameof(AccountId) });
            }
            if (Type == "Payment" && PaymentCategory == null)
            {
                yield return new ValidationResult(
                        "Payment category must be selected",
                        new[] { nameof(Type), nameof(PaymentCategory) });
            }
        }
    }
}
