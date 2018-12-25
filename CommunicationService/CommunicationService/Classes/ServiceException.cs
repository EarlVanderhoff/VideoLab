using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace CustomService
{
    /// <summary>
    /// A custom generic exception that handles service operation exceptions.
    /// </summary>
    [DataContract()]
    public class ServiceException
    {
        #region "Properties"
        [DataMember()] public string Message { get; set; }
        [DataMember()] public string InnerExceptionMessage { get; set; }
        #endregion

        #region "Public Methods"
        /// <summary>
        /// Throws an exception of type FaultException(Of CustomServiceException)
        /// </summary>
        /// <param name="Ex">The ex.</param>
        public static void ThrowException(Exception Ex)
        {
            ServiceException serviceEX = new ServiceException { Message = Ex.Message };

            if (Ex.InnerException != null)
                serviceEX.InnerExceptionMessage = Ex.InnerException.Message;

            throw new FaultException<ServiceException>(serviceEX);
        }
        #endregion
    }
}