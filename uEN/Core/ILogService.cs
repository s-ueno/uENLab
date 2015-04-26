using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace uEN.Core
{
    /// <summary>
    /// ログ機能を定義します。
    /// </summary>
    public interface ILogService
    {
        void TraceInformation(string message);
        void TraceInformation(string format, params object[] args);

        void TraceWarning(string message);
        void TraceWarning(string format, params object[] args);

        void TraceError(Exception ex);
    }

    /// <summary>
    /// ログ機能を提供します。
    /// </summary>
    /// <remarks>
    /// フレームワーク層でのログ出力にはTraceを利用し、業務システムではBizUtilsを利用すること
    /// </remarks>
    [PartCreationPolicy(CreationPolicy.Shared)]
    [Export(typeof(ILogService))]
    [ExportMetadata(Repository.Priority, int.MaxValue)]
    public class TraceLogService : ILogService
    {
        public virtual void TraceInformation(string message)
        {
            Trace.TraceInformation(message);
        }

        public virtual void TraceInformation(string format, params object[] args)
        {
            Trace.TraceInformation(format, args);
        }

        public virtual void TraceWarning(string message)
        {
            Trace.TraceWarning(message);
        }

        public virtual void TraceWarning(string format, params object[] args)
        {
            Trace.TraceWarning(format, args);
        }

        public virtual void TraceError(Exception ex)
        {
            if (ex == null) return;
            Trace.TraceError(ex.ToString());
            var tex = ex as TargetInvocationException;
            if (tex != null)
            {
                var innerException = tex.InnerException as Exception;
                if (innerException != null)
                    Trace.TraceError(innerException.ToString());
            }
        }
    }


    /// <summary>
    /// エンタープライズ アプリケーション用ログリスナーを提供します
    /// </summary>
    /// <seealso cref="System.Diagnostics.DelimitedListTraceListener"/>
    /// <seealso cref="System.Diagnostics.TextWriterTraceListener"/>
    public class BizTraceListener : TraceListener
    {
        public BizTraceListener() : this("BizTrace") { }
        public BizTraceListener(string name) : base(name) { }
        protected override string[] GetSupportedAttributes()
        {
            return new[] { "delimiter", "encoding", "maximumSize", "escape", "dateTimeFormat", 
                "fileNamePrefixDateFormat", "fileName" };
        }
        public string FileNamePrefixDateFormat
        {
            get
            {
                lock (this)
                {
                    if (Attributes.ContainsKey("fileNamePrefixDateFormat"))
                    {
                        _fileNamePrefixDateFormat = Attributes["fileNamePrefixDateFormat"];
                    }
                }
                return _fileNamePrefixDateFormat;
            }
            set
            {
                if (value == null) return;
                _fileNamePrefixDateFormat = value;
            }
        }
        private string _fileNamePrefixDateFormat = @"yyyyMMddHHmmssfff";
        public string FileName
        {
            get
            {
                lock (this)
                {
                    if (Attributes.ContainsKey("fileName"))
                    {
                        _fileName = Attributes["fileName"];
                    }
                }
                return _fileName;
            }
            set
            {
                if (value == null) return;
                _fileName = value;
            }
        }
        private string _fileName = @"trace.log";

        public string DateTimeFormat
        {
            get
            {
                lock (this)
                {
                    if (Attributes.ContainsKey("dateTimeFormat"))
                    {
                        _dateTimeFormat = Attributes["dateTimeFormat"];
                    }
                }
                return _dateTimeFormat;
            }
            set
            {
                if (value == null) return;
                _dateTimeFormat = value;
            }
        }
        private string _dateTimeFormat = "yyyy/MM/dd HH:mm:ss.fff";

        public string Escape
        {
            get
            {
                lock (this)
                {
                    if (Attributes.ContainsKey("escape"))
                    {
                        _escape = Attributes["escape"];
                    }
                }
                return _escape;
            }
            set
            {
                if (value == null) return;
                _escape = value;
            }
        }
        private string _escape = "\"";


        public string Delimiter
        {
            get
            {
                lock (this)
                {
                    if (Attributes.ContainsKey("delimiter"))
                    {
                        _delimiter = Attributes["delimiter"];
                    }
                }
                return _delimiter;
            }
            set
            {
                if (value == null) return;
                _delimiter = value;
            }
        }
        private string _delimiter = ",";
        public Encoding Encoding
        {
            get
            {
                lock (this)
                {
                    if (Attributes.ContainsKey("encoding"))
                    {
                        var buff = Attributes["encoding"];
                        try
                        {
                            _encoding = Encoding.GetEncoding(buff);
                        }
                        catch
                        {
                        }
                    }
                }
                return _encoding;
            }
            set
            {
                if (value == null) return;
                _encoding = value;
            }
        }
        private Encoding _encoding = Encoding.UTF8;


        public int MaximumSize
        {
            get
            {
                lock (this)
                {
                    if (Attributes.ContainsKey("maximumSize"))
                    {
                        var buff = Attributes["maximumSize"];
                        var i = _maximumSize;
                        if (int.TryParse(buff, out i))
                        {
                            _maximumSize = i;
                        }
                    }
                }
                return _maximumSize;
            }
            set
            {
                _maximumSize = value;
            }
        }
        private int _maximumSize = 10 * 1024 * 1024;

        protected StreamWriter Writer
        {
            get
            {
                EnsureWriter();
                return _writer;
            }
        }
        private StreamWriter _writer;
        private void EnsureWriter()
        {
            if (_writer == null)
            {
                lock (this)
                {
                    if (_writer == null)
                    {
                        _writer = GenerateWriter();
                    }
                }
            }
            if (MaximumSize < _writer.BaseStream.Length)
            {
                lock (this)
                {
                    if (MaximumSize < _writer.BaseStream.Length)
                    {
                        _writer.Flush();
                        _writer.Close();
                        _writer.Dispose();
                        _writer = GenerateWriter();
                    }
                }
            }
        }
        protected virtual StreamWriter GenerateWriter()
        {
            var uri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var dir = Path.GetDirectoryName(uri.LocalPath);

            var buff = System.IO.Path.Combine(dir, FileName);
            dir = System.IO.Path.GetDirectoryName(buff);
            var baseName = System.IO.Path.GetFileName(buff);
            var fileName = string.Format("{0}_{1}", DateTime.Now.ToString(FileNamePrefixDateFormat), baseName);


            var i = 0;
            while (System.IO.File.Exists(System.IO.Path.Combine(dir, fileName)))
            {
                System.Threading.Thread.Sleep(1);
                fileName = string.Format("{0}_{1}", DateTime.Now.ToString(FileNamePrefixDateFormat), baseName);
                if (3 < ++i)
                {
                    break; //3回トライしてファイルが存在するようであれば、意図的なファイル名付与として上書きする
                }
            }

            var writer = new StreamWriter(System.IO.Path.Combine(dir, fileName), false, Encoding);
            writer.AutoFlush = true;
            return writer;
        }
        public override void Write(string message)
        {
            if (base.NeedIndent)
            {
                this.WriteIndent();
            }
            try
            {
                Writer.Write(message);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public override void WriteLine(string message)
        {
            if (base.NeedIndent)
            {
                this.WriteIndent();
            }
            try
            {
                Writer.WriteLine(message);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            TraceEvent(eventCache, source, eventType, id, message, null);
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if ((this.Filter == null) || this.Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null))
            {
                this.WriteDateTime(eventCache);


                this.Write(Delimiter);
                this.WriteHeader(source, eventType, id);


                this.Write(Delimiter);
                if (args != null)
                {
                    this.Write(DoEscape(string.Format(CultureInfo.InvariantCulture, format, args)));
                }
                else
                {
                    this.Write(DoEscape(format));
                }


                this.Write(Delimiter);
                this.WriteFooter(eventCache);
            }
        }
        protected void WriteDateTime(TraceEventCache eventCache)
        {
            if (IsEnabled(TraceOptions.DateTime))
            {
                this.Write(DoEscape(DateTime.Now.ToString(DateTimeFormat)));
            }
        }
        protected void WriteHeader(string source, TraceEventType eventType, int id)
        {
            var list = new List<string>();
            list.Add(DoEscape(source));
            list.Add(DoEscape(eventType.ToString()));
            list.Add(DoEscape(id.ToString(CultureInfo.InvariantCulture)));
            this.Write(string.Join(Delimiter, list));
        }
        protected string DoEscape(string s)
        {
            return Escape + (s ?? string.Empty).Replace(Escape, Escape + Escape) + Escape;
        }
        protected void WriteFooter(TraceEventCache eventCache)
        {
            if (eventCache != null)
            {
                var list = new List<string>();
                if (IsEnabled(TraceOptions.ProcessId))
                {
                    list.Add(DoEscape(eventCache.ProcessId.ToString()));
                }
                if (IsEnabled(TraceOptions.ThreadId))
                {
                    list.Add(DoEscape(eventCache.ThreadId.ToString()));
                }
                if (IsEnabled(TraceOptions.Timestamp))
                {
                    list.Add(DoEscape(eventCache.Timestamp.ToString()));
                }
                if (IsEnabled(TraceOptions.LogicalOperationStack))
                {
                    var buff = new List<string>();
                    foreach (var each in eventCache.LogicalOperationStack)
                    {
                        buff.Add("  " + each.ToString());
                    }
                    list.Add(DoEscape(string.Join("\n", buff)));
                }
                if (IsEnabled(TraceOptions.Callstack))
                {
                    list.Add(DoEscape(eventCache.Callstack.ToString()));
                }
                this.Write(string.Join(Delimiter, list));
            }
            this.WriteLine("");
        }

        protected bool IsEnabled(TraceOptions opts)
        {
            return ((opts & this.TraceOutputOptions) != TraceOptions.None);
        }

        public override void Flush()
        {
            Writer.Flush();
        }
        public override void Close()
        {
            Writer.Close();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Writer.Dispose();
            }
        }
    }

}
