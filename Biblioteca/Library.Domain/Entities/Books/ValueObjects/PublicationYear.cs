using Library.Domain.Exceptions;

namespace Library.Domain.Entities.Books.ValueObjects
{
    public sealed record PublicationYear
    {
        public int Value { get; private set; }

        private PublicationYear()
        {
        }

        public PublicationYear(int value)
        {
            ApplyValueRules(value);
            Value = value;
        }

        private static void ApplyValueRules(int value)
        {
            if (value < 1 || value > DateTime.UtcNow.Year)
            {
                throw new BussinesRuleException("El año de publicación debe estar entre 1 y el año actual.");
            }
        }
    }
}
