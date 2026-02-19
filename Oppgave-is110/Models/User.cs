using System;

namespace UniversitySystem.Models
{
    public abstract class User
    {
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;

        protected User(string name, string email)
        {
            SetName(name);
            SetEmail(email);
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");

            Name = name.Trim();
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be empty.");

            email = email.Trim();

            if (!email.Contains("@") || email.StartsWith("@") || email.EndsWith("@"))
                throw new ArgumentException("Email format is not valid.");

            Email = email;
        }

        public override string ToString()
        {
            return $"{Name} ({Email})";
        }
    }
}
