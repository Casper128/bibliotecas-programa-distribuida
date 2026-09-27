using Library.Domain.Exceptions;

namespace Library.Domain.Entities.Books.ValueObjects
{
    public sealed record Isbn
    {
        public string Value { get; private set; } = null!;

        private Isbn()
        {
        }

        public Isbn(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new BussinesRuleException("El ISBN es requerido.");
            }

            string normalizedValue = value.Trim().Replace("-", "").Replace(" ", "").ToUpperInvariant();
            ApplyValueRules(normalizedValue);
            Value = normalizedValue;
        }

        private static void ApplyValueRules(string value)
        {
            if (value.Length != 10 && value.Length != 13)
            {
                throw new BussinesRuleException("El ISBN debe tener 10 o 13 caracteres, sin contar espacios ni guiones.");
            }

            for (int index = 0; index < value.Length; index++)
            {
                char character = value[index];
                bool isFinalX = value.Length == 10 && index == 9 && character == 'X';

                if (!isFinalX && (character < '0' || character > '9'))
                {
                    throw new BussinesRuleException("El ISBN solo admite dígitos y una X final para ISBN de 10 caracteres.");
                }
            }
        }
    }
}
