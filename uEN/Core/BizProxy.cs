using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Services;
using System.Security.Permissions;
using System.Text;
using uEN.Core.Data;

namespace uEN.Core
{
    //https://msdn.microsoft.com/ja-jp/library/system.runtime.remoting.proxies.realproxy.aspx
    //http://pro.art55.jp/?eid=1304152
    public class BizProxy : RealProxy
    {

        private static readonly bool TraceArgs = BizUtils.AppSettings("BizProxy.TraceArgs", false);

        MarshalByRefObject _target;
        public BizProxy(MarshalByRefObject target)
            : base(RemotingServices.GetRealProxy(target).GetProxiedType())
        {
            _target = target;
        }

        public override IMessage Invoke(IMessage msg)
        {
            var callMessage = (IMethodCallMessage)msg;
            var ctor = callMessage as IConstructionCallMessage;
            if (ctor != null)
            {
                RealProxy realProxy = RemotingServices.GetRealProxy(_target);
                realProxy.InitializeServerObject(ctor);
                var transparentProxy = (MarshalByRefObject)GetTransparentProxy();
                return EnterpriseServicesHelper.CreateConstructionReturnMessage(ctor, transparentProxy);
            }

            TraceMethodStart(callMessage);
            var myReturnMessage = RemotingServices.ExecuteMessage(_target, callMessage);
            if (myReturnMessage.Exception != null)
            {
                TraceMethodError(myReturnMessage, myReturnMessage.Exception);
            }
            else
            {
                TraceMethodEnd(myReturnMessage);
            }
            return myReturnMessage;
        }
        protected virtual void TraceMethodStart(IMethodCallMessage callMessage)
        {
            var facadeProxy = _target as BizServiceFacadeProxy;
            if (facadeProxy != null)
            {
                Trace.TraceInformation("--- {0}.Execute Start --- ", facadeProxy.OwnerTypeFullName);
            }
            else
            {
                Trace.TraceInformation("--- {0} Start --- ", MethodName(callMessage));
            }

            if (TraceArgs)
                Trace.TraceInformation("ReturnValue:{0}", ObjectDumper.ToString(callMessage.Args));
        }
        protected virtual void TraceMethodEnd(IMethodReturnMessage msg)
        {
            if (TraceArgs)
                Trace.TraceInformation("ReturnValue:{0}", ObjectDumper.ToString(msg.ReturnValue));


            var facadeProxy = _target as BizServiceFacadeProxy;
            if (facadeProxy != null)
            {
                Trace.TraceInformation("--- {0}.Execute Finish --- ", facadeProxy.OwnerTypeFullName);
            }
            else
            {
                Trace.TraceInformation("--- {0} Finish --- ", MethodName(msg));
            }
        }
        protected virtual void TraceMethodError(IMethodReturnMessage msg, Exception ex)
        {
            Trace.TraceError(ex.ToString());

            var facadeProxy = _target as BizServiceFacadeProxy;
            if (facadeProxy != null)
            {
                Trace.TraceInformation("--- {0}.Execute Error --- ", facadeProxy.OwnerTypeFullName);
            }
            else
            {
                Trace.TraceInformation("--- {0} Error --- ", MethodName(msg));
            }
        }
        protected virtual string MethodName(IMethodMessage msg)
        {
            var result = msg.MethodName;
            try
            {
                result = msg.TypeName.Split(',').First() + "." + msg.MethodName;
            }
            catch
            {
            }
            return result;
        }
    }

    public class BizProxyAttribute : ProxyAttribute
    {
        // Create an instance of ServicedComponentProxy
        public override MarshalByRefObject CreateInstance(Type serverType)
        {
            var instance = base.CreateInstance(serverType);
            var factory = Repository.GetPriorityExport<BizProxyFactory>();
            var proxy = factory.CreateProxy(instance);
            return proxy.GetTransparentProxy() as MarshalByRefObject;
        }
    }

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [BizProxy]
    public abstract class BizService : ContextBoundObject { }

    //MEFでの安全なコンストラクタ パターン
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Export(typeof(BizProxyFactory))]
    [ExportMetadata(Repository.Priority, int.MaxValue)]
    public class BizProxyFactory
    {
        public virtual RealProxy CreateProxy(MarshalByRefObject target)
        {
            return new BizProxy(target);
        }
    }

    public class BizServiceFacadeProxy : BizService
    {
        public BizServiceFacadeProxy(string ownerType)
        {
            OwnerTypeFullName = ownerType;
        }
        public String OwnerTypeFullName;
        public object Execute(Func<object, object> execute, object request)
        {
            return execute(request);
        }
    }

    public interface IBizServiceFacade
    {
        object Execute(object request);
    }

    public abstract class BizServiceFacade<TRequest, TResponse> : IBizServiceFacade
    {
        public abstract TResponse Execute(TRequest request);
        object IBizServiceFacade.Execute(object request)
        {
            var svc = new BizServiceFacadeProxy(this.GetType().FullName);
            return svc.Execute(x => (TResponse)Execute((TRequest)x), (TRequest)request);
        }

        /// <summary>
        /// ビジネス例外を通知します。
        /// </summary>
        /// <param name="hasError">真の場合は例外を通知します。</param>
        /// <param name="message">通知するメッセージ</param>
        protected virtual void ThrowValidationError(bool hasError, string message)
        {
            if (hasError)
                throw new BizApplicationException(message);
        }
    }

    [Serializable]
    public sealed class NullRequest
    {
        private NullRequest() { }
        public static readonly NullRequest Value = new NullRequest();
    }

    [Serializable]
    public sealed class NullResponse
    {
        private NullResponse() { }
        public static readonly NullResponse Value = new NullResponse();
    }

}
