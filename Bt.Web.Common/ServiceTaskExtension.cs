using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace Bt.Web.Common
{
    public static class ServiceTaskExtension
    {
        public static async Task<Result> ToResultTask(this Task<ServiceResult> task) =>
            await task.ConfigureAwait(false);
        
        public static async Task<Result<T>> ToResultTask<T>(this Task<ServiceResult<T>> task) =>
            await task.ConfigureAwait(false);
        
        public static async Task UnwrapToResult<TE>(this Task<ServiceResult> task) where TE : ServiceException, new() => 
            (await task.ConfigureAwait(false)).Unwrap<TE>();

        public static async Task<T> UnwrapToResult<T, TE>(this Task<ServiceResult<T>> task) where TE : ServiceException, new() => 
            (await task.ConfigureAwait(false)).Unwrap<TE>();
    }
}