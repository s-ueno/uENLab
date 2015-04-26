using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using uEN.Extensions;
namespace uEN.Core
{
    public class BizWebService : WebService
    {
        public BizWebService()
        {
            foreach (var each in BizUtils.AdditionalInfoKeys)
            {
                BizUtils.AdditionalInfo[each] = GetCookie(each);
            }
        }


        [WebMethod]
        public virtual byte[] Execute(string typeName, byte[] request)
        {
            var facade = Repository.GetPriorityExport(typeName) as IBizServiceFacade;
            if (facade == null)
                throw new InvalidOperationException("汎用WebサービスはIBizServiceFacadeおよびExportMetadataを実装する必要があります。");

            var deserializedRequest = request.FromByteDeserialize();
            var respons = facade.Execute(deserializedRequest);
            var serializedRespons = respons.ToByteSerialize();

            return serializedRespons;
        }

        public string GetCookie(string key)
        {
            if (Context.Request.Cookies[key] != null)
            {
                return HttpUtility.UrlDecode(Context.Request.Cookies[key].Value);
            }
            return null;
        }
    }

    [Export(typeof(BizWebServiceClientProtocol))]
    [ExportMetadata(Repository.Priority, int.MaxValue)]
    [WebServiceBindingAttribute(Name = "XmlBizWebService", Namespace = "http://tempuri.org/")]
    public class BizWebServiceClientProtocol : SoapHttpClientProtocol
    {
        private readonly string DefaultUrl = BizUtils.AppSettings("BizWebServiceClient.Url", "");
        private readonly int DefaultTimeOut = BizUtils.AppSettings("BizWebServiceClient.TimeOut", 1000 * 6);

        public BizWebServiceClientProtocol()
        {
            if (!string.IsNullOrWhiteSpace(DefaultUrl))
                this.Url = DefaultUrl;

            this.CookieContainer = new CookieContainer(
                    CookieContainer.DefaultCookieLimit * 10,
                    CookieContainer.DefaultPerDomainCookieLimit * 10,
                    CookieContainer.DefaultCookieLengthLimit * 10);

            this.Timeout = DefaultTimeOut;


            foreach (var each in BizUtils.AdditionalInfo.Keys)
            {
                SetCookie(each, BizUtils.AdditionalInfo[each]);
            }
        }

        [SoapDocumentMethodAttribute("http://tempuri.org/Execute",
            RequestNamespace = "http://tempuri.org/",
            ResponseNamespace = "http://tempuri.org/",
            Use = SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public byte[] Execute(string typeName, byte[] request)
        {
            object[] results = this.Invoke("Execute", new object[] { typeName, request });
            return ((byte[])(results[0]));
        }

        public void ExecuteAsync(string commandTypeName, byte[] request, SendOrPostCallback callback, object userState = null)
        {
            this.InvokeAsync("Execute", new object[] { commandTypeName, request }, callback, userState);
        }

        public void SetCookie(string key, string value)
        {
            CookieContainer.Add(new Uri(this.Url), new Cookie(key, HttpUtility.UrlEncode(value)));
        }

    }

    public class BizWebServiceTypeNameAttribute : Attribute
    {
        public BizWebServiceTypeNameAttribute(string typeName)
        {
            this.TypeName = typeName;
        }
        public string TypeName { get; private set; }
    }

    public class BizWebServiceClientService<TRequest, TResponse>
    {
        private static readonly CookieContainer _cookieContainer = new CookieContainer(
                    CookieContainer.DefaultCookieLimit * 10,
                    CookieContainer.DefaultPerDomainCookieLimit * 10,
                    CookieContainer.DefaultCookieLengthLimit * 10);

        public BizWebServiceClientService()
        {
            var att = this.GetType().GetCustomAttributes(typeof(BizWebServiceTypeNameAttribute), true)
                                         .OfType<BizWebServiceTypeNameAttribute>()
                                         .FirstOrDefault();
            if (att == null)
                throw new InvalidOperationException("BizWebServiceTypeNameを指定してください。");
            TypeName = att.TypeName;
        }

        public String TypeName { get; private set; }


        public TResponse Execute(TRequest request)
        {
            var clientProtocol = Repository.GetPriorityExport<BizWebServiceClientProtocol>();

            var serializedRequest = request.ToByteSerialize();
            var result = clientProtocol.Execute(TypeName, serializedRequest);

            return (TResponse)result.FromByteDeserialize();
        }


        public void ExecuteAsync(TRequest request, Action<TResponse> callback = null)
        {
            var clientProtocol = Repository.GetPriorityExport<BizWebServiceClientProtocol>();

            var serializedRequest = request.ToByteSerialize();

            clientProtocol.ExecuteAsync(TypeName, serializedRequest, OnSendOrPostCallback, callback);

        }

        private void OnSendOrPostCallback(object arg)
        {
            var e = (InvokeCompletedEventArgs)arg;
            if (e.Error != null)
            {
                Trace.TraceError(e.Error.ToString());
                throw e.Error;
            }

            var result = e.Results[0] as Byte[];
            var deserializedResult = result.FromByteDeserialize();

            var callBack = e.UserState as Action<TResponse>;
            callBack((TResponse)deserializedResult);
        }


    }


}
