using System;
using System.Threading.Tasks;

namespace Event1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Event!");
            #region event1
            Publisher pub = new Publisher();
            pub.SampleEvent += publishEvent;
            pub.RaiseSampleEvent();
            Console.ReadKey();
            #endregion
            #region event2
            Counter c = new Counter(new Random().Next(10));
            Console.WriteLine($"临界值为:{c.GetThreshold()}");
            c.ThresholdReached += c_ThresholdReached;

            Console.WriteLine("press 'a' key to increase total");
            while (Console.ReadKey(true).KeyChar == 'a')
            {
                Console.WriteLine("adding one");
                c.Add(1);
            }
            #endregion

        }

        static void publishEvent(object sender, SampleEventArgs e)
        {
            Console.WriteLine("publish event say: " + e.Text);
        }

        static async void c_ThresholdReached(object sender, ThresholdReachedEventArgs e)
        {
            Console.WriteLine("The threshold of {0} was reached at {1}.", e.Threshold, e.TimeReached);

            Console.WriteLine("exit coming soon...");
            await Task.Delay(3000);
            Environment.Exit(0);
        }
    }
    #region event1
    public class SampleEventArgs
    {
        public SampleEventArgs(string s) { Text = s; }
        public string Text { get; }//readonly
    }

    public class Publisher
    {
        // Declare the delegate (if using non-generic pattern).
        public delegate void SampleEventHandler(object sender, SampleEventArgs e);

        // Declare the event.
        public event SampleEventHandler SampleEvent;

        // Wrap the event in a protected virtual method
        // to enable derived classes to raise the event.
        public void RaiseSampleEvent()
        {
            // Raise the event in a thread-safe manner using the ?. operator.
            SampleEvent?.Invoke(this, new SampleEventArgs("Hello"));
        }
    }
    #endregion

    #region event2
    public class Counter
    {
        private int threshold;
        private int total;
        public Counter(int passedThreshold)
        {
            threshold = passedThreshold;
        }

        public int GetThreshold() => this.threshold;

        public void Add(int x)
        {
            total += x;
            if (total >= threshold)
            {
                ThresholdReachedEventArgs args = new ThresholdReachedEventArgs();
                args.Threshold = threshold;
                args.TimeReached = DateTime.Now;
                OnThresholdReached(args);
            }
        }

        protected virtual void OnThresholdReached(ThresholdReachedEventArgs e)
        {
            EventHandler<ThresholdReachedEventArgs> handler = ThresholdReached;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<ThresholdReachedEventArgs> ThresholdReached;
    }

    public class ThresholdReachedEventArgs : EventArgs
    {
        public int Threshold { get; set; }
        public DateTime TimeReached { get; set; }
    }
    #endregion


}
