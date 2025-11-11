namespace HallOfFame.Logging
{
    /// <summary>
    /// Провайдер логгера, работающего с файлами.
    /// </summary>
    public class FileLoggerProvider : ILoggerProvider
    {
        private string _path;

        public FileLoggerProvider(string path)
        {
            _path = path;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_path);
        }

        public void Dispose()
        {
        }
    }
}
