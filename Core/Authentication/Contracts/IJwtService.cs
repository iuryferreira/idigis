namespace Authentication.Contracts
{
    public interface IJwtService
    {
        string GenerateToken (string email, string id);
    }
}
