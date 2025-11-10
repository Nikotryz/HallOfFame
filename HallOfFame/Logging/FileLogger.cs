namespace HallOfFame.Logging
{
    public class FileLogger : ILogger
    {
        private string _filePath;
        private static object _lock = new object();

        public FileLogger(string filePath)
        {
            _filePath = filePath;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel.Error;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null)
            {
                return;
            }

            if (!IsEnabled(logLevel))
            {
                return;
            }

            lock (_lock)
            {
                string fullFilePath = Path.Combine(_filePath, DateTime.Now.ToString("dd-MM-yyyy") + "_log.txt");

                string n = Environment.NewLine;
                string exceptionInfo = string.Empty;

                if (exception != null)
                {
                    exceptionInfo = $"{n}{exception.GetType()}: {exception.Message}{n}{exception.StackTrace}{n}";
                }

                string log = $"[{logLevel.ToString()}] [{DateTime.Now.ToString()}] {formatter(state, exception)}{n}{exceptionInfo}";

                File.AppendAllText(fullFilePath, log);
            }
        }
    }
}