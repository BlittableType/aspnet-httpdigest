using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace Bt.Web.Common
{
    [GeneratedCode("NSwag", "13.13.2.0 (NJsonSchema v10.5.2.0 (Newtonsoft.Json v12.0.0.0))")]
    public class ServiceResult
    {
        public ServiceResult(Result result, IReadOnlyDictionary<string, IEnumerable<string>> headers = default)
        {
            Result = result;
            Headers = headers;
            
            if (headers is null)
                Headers = new Dictionary<string, IEnumerable<string>>();
        }
        public Result Result { get; }
        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; }

        public static implicit operator Result(ServiceResult serviceResult) => serviceResult.Result;
        public static explicit operator ServiceResult(Result result) => new ServiceResult(result);

        public void Unwrap<TE>() where TE : ServiceException, new() => ThrowIfFailed<TE>();
        
        private Result ThrowIfFailed<TE>() where TE : ServiceException, new() => 
            Result.OnFailure(s => 
                    throw ((Exception)(Activator.CreateInstance(typeof(TE), new object[] {s})
                               ?? new NullReferenceException($"Failed to construct {typeof(TE).FullName} with argument \"{s}\""))));
    }

    [GeneratedCode("NSwag", "13.13.2.0 (NJsonSchema v10.5.2.0 (Newtonsoft.Json v12.0.0.0))")]
    public class ServiceResult<T>
    {
        public ServiceResult(Result<T> result, IReadOnlyDictionary<string, IEnumerable<string>> headers = default)
        {             
            Result = result;
            Headers = headers;
            
            if (headers is null)
                Headers = new Dictionary<string, IEnumerable<string>>();
        }

        public Result<T> Result { get; }
        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; }

        public static implicit operator Result<T>(ServiceResult<T> serviceResult) => serviceResult.Result;
        public static explicit operator ServiceResult<T>(Result<T> result) => new ServiceResult<T>(result);
        
        public T Value => Result.Value;
        public T ValueOrDefault => Result.IsSuccess ? Result.Value : default;
   
        public T Unwrap<TE>() where TE : ServiceException, new() => ThrowIfFailed<TE>().Value;
        
        private Result<T> ThrowIfFailed<TE>() where TE : ServiceException, new() => 
            ResultExtensions.OnFailure<T>(Result, s => 
                    throw ((Exception)(Activator.CreateInstance(typeof(TE), new object[] {s})
                               ?? new NullReferenceException($"Failed to construct {typeof(TE).FullName} with argument \"{s}\""))));

    }
}