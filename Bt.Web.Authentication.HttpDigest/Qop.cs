using System;
using CSharpFunctionalExtensions;

namespace Bt.Web.Authentication.HttpDigest
{
    public class Qop : ValueObject<Qop>
    {
        public static readonly Qop Auth = new Qop("auth");
        public static readonly Qop AuthInt = new Qop("auth-int");
        
        public string Value { get; }

        private Qop(string val)
        {
            Value = val;
        }
        
        protected override bool EqualsCore(Qop other)
        {
            return this.Value == other.Value;
        }

        protected override int GetHashCodeCore()
        {
            return this.Value.GetHashCode();
        }

        public override string ToString()
        {
            return this.Value;
        }

        public static Qop Parse(string val)
        {
            if (val == Auth.Value)
                return Auth;
                    
            if (val == AuthInt.Value)
                return AuthInt;

            throw new ArgumentOutOfRangeException(nameof(val),
                $"Only following values are valid: {Auth}, {AuthInt}");
        }
    }
}