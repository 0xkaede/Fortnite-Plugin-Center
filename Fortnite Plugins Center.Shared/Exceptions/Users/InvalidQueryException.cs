namespace Fortnite_Plugins_Center.Shared.Exceptions.Users
{
    public class InvalidQueryException : BaseException
    {
        public InvalidQueryException(string reason = "")
            : base(1002, reason == "" ? "The provided query was invalid." : "The provided query was invalid. Reason: {0}", reason)
        {
            StatusCode = 400;
        }
    }
}
