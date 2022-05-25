using Newtonsoft.Json;
using System;
using J = Newtonsoft.Json.JsonPropertyAttribute;

namespace Fortnite_Plugins_Center.Shared.Exceptions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BaseException : Exception
    {
        [J("errorType")] public string ErrorType => GetType().FullName.ToLower().Replace(".shared.exceptions", "").Replace("fortnite_plugins_center.", "");

        [J("errorMessage")] public string ErrorMessage => base.Message;

        [J("errorCode")] public int ErrorCode { get; set; }

        [J("errorVars")] public string[] ErrorVars { get; set; }

        public int StatusCode = 400;

        public BaseException(int code, string message, params string[] vars)
            : base(string.Format(message, vars))
        {
            ErrorCode = code;
            ErrorVars = vars;
        }
    }
}
