using System.Collections.Generic;
using Bt.Web.Authentication.HttpDigest;
using Bt.Web.Authentication.HttpDigest.AspNetCore;

namespace Bt.Web.Service.Auth
{
    public class NonceStorage : INonceStorage
    {
        private readonly HashSet<Nonce> _storage = new HashSet<Nonce>(10);

        public void Save(Nonce nonce)
        {
            _storage.Add(nonce);
        }

        public Nonce Get(ulong nonce)
        {
            return _storage.TryGetValue(new Nonce(nonce), out Nonce actual) 
                ? actual 
                : null;
        }

        public void Remove(Nonce nonce)
        {
            _storage.Remove(nonce);
        }
    }
}