// TODO: implement the LogService class from the ILogService interface.
//       One explicit requirement - for the read method, if the file is not found, an InvalidOperationException should be thrown
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in LogServiceTests you can find the necessary constructor format.

// TODO: реализовать класс LogService из интерфейса ILogService.
// Одно явное требование - для метода чтения, если файл не найден, должно быть выброшено InvalidOperationException
// Другие детали реализации на ваше усмотрение, они просто должны соответствовать требованиям интерфейса
// и тесты, например, в LogServiceTests можно найти нужный формат конструктора.

using CoolParking.BL.Interfaces;
using System.Reflection;
using System.Text;

namespace CoolParking.BL
{
    public class LogService : ILogService
    {
        private readonly string _logFilePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\transactions.log";
        public LogService(string logFilePath)
        {
            this._logPath = logFilePath;
        }

        private string _logPath;
        public string LogPath => _logPath;


        public string Read()
        {
            if (!string.IsNullOrEmpty(_logPath))
            {
                if (!File.Exists(_logPath))
                {
                    throw new System.InvalidOperationException();
                }
            }
            string readTransactions = File.ReadAllText(_logPath, Encoding.Default);

            return readTransactions.Length > 0 ? readTransactions : "File is empty";
        }

        public void Write(string logInfo)
        {
            if (!string.IsNullOrEmpty(_logPath))
            {  
                var res = string.Concat(logInfo, "\r\n"); 
                if(File.Exists(_logPath))
                {
                    File.AppendAllText(_logPath, res);    
                }
                else
                {
                    File.WriteAllText(_logPath, res);
                }
            }
        }
    }
}
