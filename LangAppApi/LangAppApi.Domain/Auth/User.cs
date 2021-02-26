namespace LangAppApi.Domain.Auth
{
    public class User
    {
        public string FirstName { get; set; }
        public string UserId { get; set; }

        public User()
        {
        }

        public User(string firstName, string userId)
        {
            FirstName = firstName;
            UserId = userId;
        }
    }
}