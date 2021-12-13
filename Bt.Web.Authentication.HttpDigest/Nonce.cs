using System;
using System.Security.Cryptography;

namespace Bt.Web.Authentication.HttpDigest
{
    public class Nonce : IEquatable<Nonce>
    {
        public ulong Value { get; }
        public ulong Count { get; private set; } = 0U;
        public DateTimeOffset Created { get; } = DateTimeOffset.UtcNow;

        public Nonce()
        {
            var rngCsp = new RNGCryptoServiceProvider();
            var randomBytes = new byte[sizeof(ulong)];
            
            rngCsp.GetBytes(randomBytes);

            Value = BitConverter.ToUInt64(randomBytes, 0);
        }

        public Nonce(ulong value)
        {
            Value = value;
        }

        public ulong IncrementCount()
        {
            return ++Count;
        }

        public bool Equals(Nonce other)
        {
            if (ReferenceEquals(this, other)) 
                return true;
            
            return Value == other?.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) 
                return false;
            
            if (ReferenceEquals(this, obj)) 
                return true;

            return Equals((Nonce)obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        
        public static bool operator ==(Nonce left, Nonce right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right))
                return true;
            
            return !ReferenceEquals(null, left) 
                ? left.Equals(right) 
                : false;
        }

        public static bool operator !=(Nonce left, Nonce right)
        {
            return !(left == right);
        }
    }
}