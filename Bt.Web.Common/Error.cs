using System;
using CSharpFunctionalExtensions;

namespace Bt.Web.Common
{
    public class Error : ValueObject<Error>
    {
        public string Code { get; }
        public string Message { get; }

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public Error(string code) : this(code, String.Empty)
        { }

        protected override bool EqualsCore(Error other)
        {
            return this.Code == other.Code;
        }

        protected override int GetHashCodeCore()
        {
            return Code.GetHashCode();
        }
    }
}