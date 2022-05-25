namespace Fortnite_Plugins_Center.Shared.Exceptions.Users
{
    public class UserNotFoundException : BaseException
    {
        public UserNotFoundException(string item)
            : base(1003, "Sorry, the user {0} could not be found.", item)
        {
            StatusCode = 404;
        }
    }
}
