using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bt.Web.Common
{
    [Serializable]
    public class ServiceException : Exception
    {
        public ServiceException()
        { }

        public ServiceException(string message) : base(message)
        { }

        public ServiceException(string message, Exception innerException) : base(message, innerException)
        { }

        protected ServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        public ServiceException(string message, int statusCode, string responseText,
            IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception innerException)
            : this($"{message} ({statusCode})\n{responseText}\n{headers}", innerException)
        { }
        
        public ServiceException(string message, ServiceResult result)
            : this($"{message} ({result.Result.Error})\n{result.Headers}")
        { }
    }
}