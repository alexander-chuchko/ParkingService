// TODO: implement class TimerService from the ITimerService interface.
//       Service have to be just wrapper on System Timers.

// TODO: реализация класса TimerService из внешнего интерфейса ITimerService.
// Служба должна быть просто оболочкой для системных таймеров.
using CoolParking.BL.Interfaces;
using System.Threading;
using System.Timers;

namespace CoolParking.BL
{
    public class TimerService : ITimerService
    {
        public event ElapsedEventHandler Elapsed;
        private static System.Timers.Timer aTimer;

        public TimerService()
        {
            aTimer = new System.Timers.Timer();
            //Elapsed?.Invoke(this, null);
        }
        public double Interval
        {
            get => aTimer.Interval;
            set => aTimer.Interval = value;
        }


        public void Dispose()
        {
            //Try...Catch...
            System.Console.WriteLine($"{aTimer} has been disposed");
        }

        public void Start()
        {
            aTimer.Start();
        }

        public void Stop()
        {
            aTimer.Stop();
        }

        public void FireElapsedEvent()
        {
            Elapsed?.Invoke(this, null);
        }
    }
}