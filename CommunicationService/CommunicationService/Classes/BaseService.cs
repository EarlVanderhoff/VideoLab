using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace CustomService
{
    /// <summary>
    /// Base class for a web service implementation.
    /// </summary>
    public abstract class BaseService
    {
        #region "Declarations"
        protected Uri _baseHttpAddress;
        protected ServiceHost _serviceHost;
        protected Type _serviceType;
        protected Type _contractType;
        #endregion

        #region "Properties"
        /// <summary>
        /// Gets the current state of the service.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public CommunicationState State
        {
            get
            {
                if (_serviceHost == null)
                    return CommunicationState.Closed;
                return _serviceHost.State;
            }
        }

        /// <summary>
        /// Get/Set the service timeout when opening a connection.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public TimeSpan OpenTimeOut { get; set; }

        /// <summary>
        /// Get/Set the service timeout when closing a connection.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public TimeSpan CloseTimeOut { get; set; }

        /// <summary>
        /// Get/Set the service timeout when sending.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public TimeSpan SendTimeOut { get; set; }

        /// <summary>
        /// Get/Set the service timeout when receiving.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public TimeSpan ReceiveTimeOut { get; set; }
        #endregion

        #region "Public Methods"
        public BaseService()
        {
            // The service could not be loaded unless it has a default (parameter-less) constructor.
        }

        public BaseService(string httpBaseAddress, Type serviceType, Type contractType)
        {
            _baseHttpAddress = new Uri(httpBaseAddress);
            _serviceType = serviceType;
            _contractType = contractType;
        }

        /// <summary>
        /// Opens the service.
        /// </summary>
        public virtual void Open()
        {
            if (_serviceHost != null)
                Close();

            try
            {
                BasicHttpBinding binding = new BasicHttpBinding
                {
                    OpenTimeout = this.OpenTimeOut,
                    CloseTimeout = this.CloseTimeOut,
                    SendTimeout = this.SendTimeOut,
                    ReceiveTimeout = this.ReceiveTimeOut
                };

                _serviceHost = new ServiceHost(_serviceType, _baseHttpAddress);
                _serviceHost.AddServiceEndpoint(_contractType, binding, string.Empty);

                // Uncomment code block below if we need to specify other endpoints besides the default endpoints already provided by WCF
                //.AddServiceEndpoint(GetType(IVideoCapture), New WSHttpBinding, [Address])
                //.AddServiceEndpoint(GetType(IVideoCapture), New NetTcpBinding, String.Empty)
                //.AddServiceEndpoint(GetType(IVideoCapture), New NetNamedPipeBinding, "net.pipe://localhost:18090/VideoCaptureService")

                
                // Enable metadata exchange.
                _serviceHost.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });

                // Enable service debugging.
                ServiceDebugBehavior sb = _serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();
                sb.IncludeExceptionDetailInFaults = false;

                _serviceHost.Open();
            }
            catch (CommunicationException ex)
            {
                Trace.WriteLine(ex.Message);
                throw;
            }
            finally
            {
                if (_serviceHost.State == CommunicationState.Faulted)
                    _serviceHost.Abort();
            }
        }

        /// <summary>
        /// Closes the service.
        /// </summary>
        public virtual void Close()
        {
            if (_serviceHost == null || _serviceHost.State == CommunicationState.Closed)
                return;

            try
            {
                if (_serviceHost.State == CommunicationState.Faulted)
                    _serviceHost.Abort();

                _serviceHost.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Aborts the service.
        /// </summary>
        public virtual void Abort()
        {
            if (_serviceHost == null)
                return;

            try
            {
                _serviceHost.Abort();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                throw;
            }
        }
        #endregion
    }
}