using ErrorOr;

namespace ClientApplications.Domain;

public static class Errors
{
    public static class General
    {
        public const string Not_Found_Code = "General.NotFound";
        public const string Required_Code = "General.Required";
        public const string Max_Length_Code = "General.MaxLength";
        public const string Min_Length_Code = "General.MinLength";
        public const string Duplicate_Code = "General.Duplicate";
        public const string Regular_Expression_Code = "General.RegularExpression";
        public const string Invalid_Code = "General.InvalidCode";

        public static Error NotFound
            (string name, string code = Not_Found_Code)
        {
            var errorMessage = string.Format
                (Resources.Messages.Validations.NotFound, GetResource(name));

            return Error.NotFound(code, errorMessage);
        }

        public static Error Required
            (string name, string code = Required_Code)
        {
            var errorMessage = string.Format
                (Resources.Messages.Validations.Required, GetResource(name));

            return Error.Validation(code, errorMessage);
        }

        public static Error MaxLength
            (string name, int maxLength, string code = Max_Length_Code)
        {
            var errorMessage = string.Format
                (Resources.Messages.Validations.MaxLength, GetResource(name), maxLength);

            return Error.Validation(code, errorMessage);
        }

        public static Error MinLength
            (string name, int minLength, string code = Min_Length_Code)
        {
            var errorMessage = string.Format
                (Resources.Messages.Validations.MinLength, GetResource(name), minLength);

            return Error.Validation(code, errorMessage);
        }

        public static Error Duplicate
            (string name, string code = Duplicate_Code)
        {
            var errorMessage = string.Format
                (Resources.Messages.Validations.Duplicate, GetResource(name));

            return Error.Validation(code, errorMessage);
        }

        public static Error RegularExpression
            (string name, string code = Regular_Expression_Code)
        {
            var errorMessage = string.Format
                (Resources.Messages.Validations.InvalidRegularExpression, GetResource(name));

            return Error.Validation(code, errorMessage);
        }

        public static Error InvalidCode
            (string name, string code = Invalid_Code)
        {
            var errorMessage = string.Format
                (Resources.Messages.Validations.InvalidCode, GetResource(name));

            return Error.Validation(code, errorMessage);
        }
    }

    private static string GetResource(string key)
    {
        key = char.ToUpper(key[0]) + key[1..];

        var value =
            Resources.DataDictionary.ResourceManager.GetString(key);

        return value ?? key;
    }

}
