namespace Bt.Web.Authentication.HttpDigest.AspNetCore
{
    public interface INonceStorage
    {
        void Save(Nonce nonce);
        Nonce Get(ulong nonce);
        void Remove(Nonce nonce);
    }
}