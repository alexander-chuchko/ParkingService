// TODO: implement the LogService class from the ILogService interface.
//       One explicit requirement - for the read method, if the file is not found, an InvalidOperationException should be thrown
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in LogServiceTests you can find the necessary constructor format.


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

            return readTransactions;
        }

        public void Write(string logInfo)
        {
            if (!string.IsNullOrEmpty(_logPath) && !string.IsNullOrEmpty(logInfo))
            {  
                string formattedString = string.Concat(logInfo, "\r\n"); 

                if(File.Exists(_logPath))
                {
                    File.AppendAllText(_logPath, formattedString);    
                }
                else
                {
                    File.WriteAllText(_logPath, formattedString);
                }
            }
        }
    }
}
