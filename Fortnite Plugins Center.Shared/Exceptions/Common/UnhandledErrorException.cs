namespace Fortnite_Plugins_Center.Shared.Exceptions.Common
{
    public class UnhandledErrorException : BaseException
    {
        public UnhandledErrorException(string id)
            : base(1005, "Uh-oh, something broke and we weren't able to solve it. (tracking id: [{0}])", id)
        {
            StatusCode = 500;
        }
    }
}
