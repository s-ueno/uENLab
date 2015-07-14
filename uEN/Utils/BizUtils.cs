using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using uEN.Core;

namespace uEN
{
    public static class BizUtils
    {
        public static T AppSettings<T>(string key, T defaultValue = default(T))
        {
            var result = defaultValue;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                var value = appSettings.Get(key);
                if (!string.IsNullOrEmpty(value))
                {
                    result = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
                }
            }
            catch
            {
                //error free
            }
            return result;
        }

        private static readonly ILogService logger = Repository.GetPriorityExport<ILogService>();
        public static void TraceInformation(string message)
        {
            logger.TraceInformation(message);
        }
        public static void TraceInformation(string format, params object[] args)
        {
            logger.TraceInformation(format, args);
        }
        public static void TraceWarning(string message)
        {
            logger.TraceWarning(message);
        }
        public static void TraceWarning(string format, params object[] args)
        {
            logger.TraceWarning(format, args);
        }
        public static void TraceError(Exception ex)
        {
            logger.TraceError(ex);
        }


        public static string ServiceName
        {
            get
            {
                return AdditionalInfo.ContainsKey(ServiceNameTag) ?
                        AdditionalInfo[ServiceNameTag] : "unknown";
            }
            set
            {
                AdditionalInfo[ServiceNameTag] = value;
            }
        }
        public const string ServiceNameTag = "ServiceName";
        public static string AppName
        {
            get
            {
                return AdditionalInfo.ContainsKey(AppNameTag) ?
                        AdditionalInfo[AppNameTag] : "unknown";
            }
            set
            {
                AdditionalInfo[AppNameTag] = value;
            }
        }
        public const string AppNameTag = "AppName";
        public static string UserName
        {
            get
            {
                return AdditionalInfo.ContainsKey(UserNameTag) ?
                        AdditionalInfo[UserNameTag] : "unknown";
            }
            set
            {
                AdditionalInfo[UserNameTag] = value;
            }
        }
        public const string UserNameTag = "UserName";

        public static string Role
        {
            get
            {
                return AdditionalInfo.ContainsKey(RoleTag) ?
                        AdditionalInfo[RoleTag] : "unknown";
            }
            set
            {
                AdditionalInfo[RoleTag] = value;
            }
        }
        public const string RoleTag = "Role";

        public static string[] Roles
        {
            get
            {
                return Role.Split(',');
            }
        }

        public static Dictionary<string, string> AdditionalInfo
        {
            get
            {
                if (_additionalInfo == null)
                    _additionalInfo = new Dictionary<string, string>();
                return _additionalInfo;
            }
        }
        [ThreadStatic]
        private static Dictionary<string, string> _additionalInfo;

        public readonly static string[] AdditionalInfoKeys = new string[] 
        { 
            ServiceNameTag, AppNameTag, UserNameTag
        };

        public static void SignIn(string userName, string password)
        {
            UserName = userName;

            var request = new AuthenticationRequest(userName, password);
            var svc = Repository.GetPriorityExport<AuthenticationServiceProxy>();
            var ret = svc.Execute(request);
            BizAuthenticationnResponse response = ret.Response;
            foreach (var each in response.AdditionalInfo.Keys)
            {
                AdditionalInfo[each] = response.AdditionalInfo[each];
            }
            AdditionalInfo[FormsAuthentication.FormsCookiePath] = ret.Ticket;

            System.Threading.Thread.CurrentPrincipal =
                new GenericPrincipal(new GenericIdentity(userName), Roles);
        }


        public static T GetValueOrDefault<T>(object obj, T defaultValue)
        {
            var result = defaultValue;
            try
            {
                var value = Convert.ToString(obj);
                result = (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(value);
            }
            catch
            {
                //error free
            }
            return result;
        }

    }
}
