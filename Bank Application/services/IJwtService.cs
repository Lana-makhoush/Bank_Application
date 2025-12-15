namespace Bank_Application.services
{
        public interface IJwtService
        {
            string GenerateToken( int id, string role, out DateTime expiry);
        }
    }


