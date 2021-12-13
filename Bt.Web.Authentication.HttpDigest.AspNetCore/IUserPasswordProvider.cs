namespace Bt.Web.Authentication.HttpDigest.AspNetCore
{
    public interface IUserPasswordProvider
    {
        string GetPassword(string userName, string realm = null);
    }
}