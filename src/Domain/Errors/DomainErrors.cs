using SharedKernel.FunctionalTypes;

namespace Domain.Errors;

/// <summary>
    /// Predefined Domain Errors
    /// </summary>
    public static class DomainErrors
    {
        /// <summary>
        /// Domain Errors related to UserName Value Object
        /// </summary>
        public readonly struct UserName
        {
            public static readonly Error Empty = Error.Validation("UserName.Empty", "UserName is empty");

            public static readonly Error TooLong = Error.Validation("UserName.TooLong", "UserName is too long");

            public static readonly Error TooShort = Error.Validation("UserName.TooShort", "UserName is too short");
        }

        /// <summary>
        /// Domain Errors related to Email Value Object
        /// </summary>
        public readonly struct Email
        {
            public static readonly Error Empty = Error.Validation("Email.Empty", "Email is empty");

            public static readonly Error InvalidFormat = Error.Validation("Email.InvalidFormat", "Email format is invalid");

            public static readonly Error TooLong = Error.Validation("Email.TooLong", "Email is too long");
        }

        /// <summary>
        /// Error related to User Entity
        /// </summary>
        public readonly struct User
        {
            public static readonly Error UserNameOrEmailAlreadyInUse = Error.Conflict("User.UserNameOrEmailAlreadyInUse", "The specified UserName or Email is already in use.");

            public static readonly Error UserNotFound = Error.NotFound("User.UserNotFound", "The requested user doesn't Exists.");
        }
    }