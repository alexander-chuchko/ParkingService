// TODO: implement the LogService class from the ILogService interface.
//       One explicit requirement - for the read method, if the file is not found, an InvalidOperationException should be thrown
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in LogServiceTests you can find the necessary constructor format.


using CoolParking.BL.Interfaces;
using System.Text;

namespace CoolParking.BL.Services
{
    public class LogService : ILogService
    {
        public LogService(string logFilePath)
        {
            this._logPath = logFilePath;
        }

        private string _logPath;
        public string LogPath => _logPath;


        public string Read()
        {
            string? readTransactions = null;

            if (!File.Exists(_logPath))
            {
                throw new System.InvalidOperationException();
            }

            using (var file = new StreamReader(_logPath))
            {
                readTransactions = file.ReadToEnd();
            }

            return readTransactions?.Length > default(int) ? readTransactions : "There are no recorded transactions";

        }

        public void Write(string logInfo)
        {
            if (!string.IsNullOrEmpty(_logPath) && !string.IsNullOrEmpty(logInfo))
            {  
                string? formattedString = string.Concat(logInfo, "\n");

                bool isFile = File.Exists(_logPath);

                using (StreamWriter write = new StreamWriter(_logPath, isFile))
                {
                    write.Write(formattedString);
                }
            }
        }
    }
}
