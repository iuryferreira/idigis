namespace Idigis.Core.Domain.Helpers
{
    public static class Messages
    {
        private const string Base = "Este campo";
        public const string NotEmpty = Base + " deve ser informado(a).";
        public const string Email = "Forneça um email válido.";

        public static string Minimum (int size, bool isString = true)
        {
            var message = $"{Base} deve ser maior que {size}";
            return isString ? $"{message} caracteres." : $"{message}.";
        }
    }
}
