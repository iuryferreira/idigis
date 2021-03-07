namespace Shared.Notifications
{
    public static class Messages
    {
        private const string Base = "Este campo";
        public const string NotEmpty = Base + " deve ser informado(a).";
        public const string Email = "Forneça um email válido.";

        public static string Minimum (int size, string type = "string")
        {
            var message = $"{Base} deve ser maior que {size}";
            return type is "string" ? $"{message} caracteres." : $"{size}.";
        }
    }
}
