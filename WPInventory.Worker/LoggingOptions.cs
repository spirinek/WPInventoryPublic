using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Serilog.Events;

namespace WPInventory.Worker
{
    public class LoggingOptions
    {
        public LogEventLevel MinimumLevel { get; set; } = LogEventLevel.Debug;

        public string OutputTemplate { get; set; } = "[{Timestamp:HH:mm:ss.fff}] [{RequestId}] [{Level:u3}] [{UserId}] {SourceContext}: {Message:lj}{NewLine}{Exception}";
        public FileOptions File { get; set; }
        public EmailOptions Email { get; set; }

        public ElasticOptions Elastic { get; set; }

        public class FileOptions
        {
            public LogEventLevel MinimumLevel { get; set; } = LogEventLevel.Debug;
            public string Path { get; set; }
            public RollingInterval RollingInterval { get; set; }
            public string OutputTemplate { get; set; } = "[{Timestamp:o}] [{RequestId}] [{Level:u3}] [{UserId}] {SourceContext}: {Message:lj}{NewLine}{Exception}";
        }

        public class EmailOptions
        {
            public LogEventLevel MinimumLevel { get; set; } = LogEventLevel.Error;
            public string FromEmail { get; set; }
            public string ToEmail { get; set; }
            public string MailServer { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string OutputTemplate { get; set; } = "[{Timestamp:o}] [{RequestId}] [{Level:u3}] [{UserId}] {SourceContext}: {Message:lj}{NewLine}{Exception}";
        }

        public class ConsoleOptions
        {
            public LogEventLevel MinimumLevel { get; set; } = LogEventLevel.Error;
            public string OutputTemplate { get; set; } = "[{Timestamp:o}] [{RequestId}] [{Level:u3}] [{UserId}] {SourceContext}: {Message:lj}{NewLine}{Exception}";

        }


        public class ElasticOptions
        {
            public string NodeUrl { get; set; }
        }
    }
}
