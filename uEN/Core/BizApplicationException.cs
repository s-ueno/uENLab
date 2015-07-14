using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Xml;

namespace uEN
{
    [Serializable]
    public class BizApplicationException : ApplicationException
    {
        public BizApplicationException() : base() { }
        public BizApplicationException(string message)
            : base(message)
        {
        }
        public BizApplicationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    
    /// <summary>
    /// SoapExceptionにInnerExceptionを注入する
    /// </summary>
    /// <remarks>
    /// http://codezine.jp/article/detail/834?p=2
    /// </remarks>
    [Serializable]
    public class ExtractSoapExtension : SoapExtension
    {
        public override object GetInitializer(Type serviceType)
        {
            return null;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return null;
        }

        public override void Initialize(object initializer)
        {

        }
        public override void ProcessMessage(SoapMessage message)
        {
            try
            {
                switch (message.Stage)
                {
                    case SoapMessageStage.BeforeSerialize:
                        if (message.Exception != null)
                        {
                            /*
                            var nativeException = message.Exception.GetBaseException();
                            var bizException = nativeException as BizApplicationException;
                            if (bizException != null)
                            {
                                var node = DetailNode(bizException);
                                message.Exception = new SoapException(bizException.Message, SoapException.ServerFaultCode, null, DetailNode(bizException));
                            }
                            */

                            var nativeException = message.Exception.GetBaseException();
                            if (nativeException != null)
                            {
                                var node = DetailNode(nativeException);
                                message.Exception = new SoapException(nativeException.Message, SoapException.ServerFaultCode, null, DetailNode(nativeException));
                            }
                        }
                        break;
                    case SoapMessageStage.AfterDeserialize:
                        if (message.Exception != null)
                        {
                            var node = message.Exception.Detail;
                            XmlNode typeNode = node.SelectSingleNode("nativeExceptionType");
                            if (typeNode != null)
                            {
                                var nativeExceptionType = Type.GetType(typeNode.InnerText);
                                if (nativeExceptionType == null) return;

                                var exceptionMessage = node.SelectSingleNode("nativeExceptionMessage");
                                var ex = Activator.CreateInstance(nativeExceptionType, exceptionMessage.InnerText) as Exception;
                                message.Exception = new SoapException(exceptionMessage.InnerText, SoapException.ServerFaultCode, ex);
                            }
                        }
                        break;
                }
            }
            catch
            {
                //error free
            }
        }
        private XmlNode DetailNode(Exception nativeException)
        {
            var xmlDoc = new XmlDocument();
            var detailNode = xmlDoc.CreateNode(XmlNodeType.Element,
                                SoapException.DetailElementName.Name, SoapException.DetailElementName.Namespace);

            var message = xmlDoc.CreateElement("nativeExceptionMessage");
            message.AppendChild(xmlDoc.CreateTextNode(nativeException.Message));
            detailNode.AppendChild(message);

            var type = xmlDoc.CreateElement("nativeExceptionType");
            type.AppendChild(xmlDoc.CreateTextNode(nativeException.GetType().AssemblyQualifiedName));
            detailNode.AppendChild(type);

            return detailNode;
        }
    }
    public class ExtractSoapExtensionAttribute : SoapExtensionAttribute
    {
        public override Type ExtensionType
        {
            get { return typeof(ExtractSoapExtension); }
        }
        public override int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }
        private int _priority = 1;
    }
}
