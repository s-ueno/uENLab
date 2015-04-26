using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Web.Security;
using uEN.Extensions;
namespace uEN.Core
{
    /// <summary>Webサービス側で実装する任意のログイン処理です。</summary>
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(BizAuthentication))]
    [ExportMetadata(Repository.Priority, int.MaxValue)]
    public class BizAuthentication
    {
        public virtual BizAuthenticationnResponse Login(string userName, string password)
        {
            return new BizAuthenticationnResponse();
        }
    }
    public class BizAuthenticationnResponse
    {
        public BizAuthenticationnResponse()
        {
            AdditionalInfo = new Dictionary<string, string>();
        }
        public Dictionary<string, string> AdditionalInfo { get; private set; }

        public static implicit operator string(BizAuthenticationnResponse obj)
        {
            var ret = new List<string>();
            foreach (var each in obj.AdditionalInfo.Keys)
            {
                ret.Add(string.Format("{0}={1}", each, obj.AdditionalInfo[each]));
            }
            return string.Join("\t", ret);
        }
        public static implicit operator BizAuthenticationnResponse(string obj)
        {
            var ret = new BizAuthenticationnResponse();
            var arr = obj.Split('\t');
            foreach (var each in arr)
            {
                var kp = each.Split('=');
                if (kp.Length == 2)
                {
                    ret.AdditionalInfo[kp[0]] = kp[1];
                }
            }
            return ret;
        }
    }

    [Serializable]
    internal class AuthenticationRequest
    {
        public AuthenticationRequest(string userName, string password)
        {
            UserName = userName;
            Password = password.Encrypt();
        }
        public string UserName { get; private set; }
        public string Password { get; private set; }
    }
    [Serializable]
    internal class AuthenticationResponse
    {
        public string Ticket { get; set; }
        public string Response { get; set; }
    }

    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(AuthenticationService))]
    [ExportMetadata(Repository.Priority, int.MaxValue)]
    internal class AuthenticationService
        : BizServiceFacade<AuthenticationRequest, AuthenticationResponse>
    {
        public override AuthenticationResponse Execute(AuthenticationRequest request)
        {
            var username = request.UserName;
            var password = request.Password.Decrypt();

            var bizSvc = Repository.GetPriorityExport<BizAuthentication>();
            var ret = bizSvc.Login(username, password);

            var ticket = new FormsAuthenticationTicket(1,
                  username,
                  DateTime.Now,
                  DateTime.Now.AddMinutes(30),
                  true,
                  ret,
                  FormsAuthentication.FormsCookiePath);
            var encTicket = FormsAuthentication.Encrypt(ticket);

            //Web Formじゃないし、CookiesじゃなくてIsolatedStorageでいいんじゃないと思った。どうせレスポンス返すし。
            if (System.Web.HttpContext.Current != null)
            {
                System.Web.HttpContext.Current.Response.Cookies.Add(
                    new System.Web.HttpCookie(FormsAuthentication.FormsCookiePath, encTicket));
            }
            return new AuthenticationResponse() { Ticket = encTicket, Response = ret };
        }
    }
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(AuthenticationServiceProxy))]
    [ExportMetadata(Repository.Priority, int.MaxValue)]
    [BizWebServiceTypeName("uEN.Core.AuthenticationService, uEN")]
    internal class AuthenticationServiceProxy : BizWebServiceClientService<AuthenticationRequest, AuthenticationResponse> { }
}
