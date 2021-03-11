using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Models.ServiceModels;

namespace TechnicalSupport.Utils.Logger
{
    public class DbLogger : ILogger
    {
        private readonly DbContextOptionsBuilder<ChatServiceContext> _DbOptionsBuilder;
        private static string _path;
        private static object _lock = new object();
        


        public DbLogger(string path , string connectionString)
        {
            _path = path;

            _DbOptionsBuilder = new DbContextOptionsBuilder<ChatServiceContext>()
                .UseSqlServer(connectionString);
        }


        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }


        //Write log only if error occure
        public bool IsEnabled(LogLevel logLevel)
        {

            return logLevel == LogLevel.Error || logLevel == LogLevel.Critical;
        }


        //write logs in db and dublicate them in specified file
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if(formatter != null)
            {
                lock (_lock)
                {

                    var logMessage = DateTime.UtcNow + formatter(state, exception) + Environment.NewLine;

                    using var _db = new ChatServiceContext(_DbOptionsBuilder.Options);

                    File.AppendAllText(_path,  logMessage);

                    _db.Logs.Add(new Log
                    {
                        Time = DateTime.UtcNow,
                        LogMessage = logMessage
                    });

                    _db.SaveChanges();

                }
            }
        }
    }
}
