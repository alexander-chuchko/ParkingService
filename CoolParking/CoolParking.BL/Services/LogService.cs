// TODO: implement the LogService class from the ILogService interface.
//       One explicit requirement - for the read method, if the file is not found, an InvalidOperationException should be thrown
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in LogServiceTests you can find the necessary constructor format.

// TODO: реализовать класс LogService из интерфейса ILogService.
// Одно явное требование - для метода чтения, если файл не найден, должно быть выброшено InvalidOperationException
// Другие детали реализации на ваше усмотрение, они просто должны соответствовать требованиям интерфейса
// и тесты, например, в LogServiceTests можно найти нужный формат конструктора.

using CoolParking.BL.Interfaces;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CoolParking.BL
{
    public class LogService : ILogService
    {
        //private readonly string _logFilePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\Transactions.log";
        public LogService(string logFilePath)
        {
            this._logPath = logFilePath;
        }

        private string _logPath;
        public string LogPath => _logPath;

        public TransactionInfo[] TransactionInfos { get; set; }

        private string _readText;
        private string _newTransaction;


        public string Read()
        {
            FileInfo fileInf;
            if (!string.IsNullOrEmpty(_logPath))
            {
                fileInf = new FileInfo(_logPath);

                if (!fileInf.Exists)
                {
                    throw new System.InvalidOperationException();
                }
            }
            _readText = File.ReadAllText(_logPath, Encoding.Unicode);

            return _readText;
        }

        public void Write(string logInfo)
        {
            MatchCollection myMatches = Regex.Matches(Read(), _newTransaction, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
            
            if (myMatches.Count<=0)
            {
                //To do дописываем в файл с конца
                File.WriteAllText(_logPath, _newTransaction, Encoding.Unicode);
            }
            else
            {
                System.Console.WriteLine("In file is mathes");
            }

            //5 sec

            //60 sec

        }
    }
}
