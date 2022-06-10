using CoolParking.BL.Interfaces;
using System.Timers;

namespace CoolParking.BL.Tests
{
    public class FakeTimerService : ITimerService
    {
        public double Interval { get; set; }

        public event ElapsedEventHandler Elapsed;

        public void FireElapsedEvent()
        {
            Elapsed?.Invoke(this, null);
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public void Dispose()
        {
        }
    }
}
