using System;
namespace Playground
{
    public class EventPublisher
    {
        private int _accumulator = 0;
        public int Accumulator => _accumulator;

        public delegate void AccumulatorWasReset();
        public event AccumulatorWasReset AccumlatorWasReset;

        public EventPublisher() {}

        public void ResetAccumulator()
        {
            _accumulator = 0;

            AccumlatorWasReset?.Invoke();
        }

        public int Increment()
        {
            if (_accumulator == short.MaxValue)
            {
                ResetAccumulator();
            }
            _accumulator++;

            return Accumulator;
        }
    }

    public class SimpleEventAdder
    {
        public delegate void dgEventRaiser();
        public event dgEventRaiser OnMultipleOfFiveReached;

        public int Add(int x, int y)
        {
            int isSum = x + y;
            if ((isSum % 5 == 0) && (OnMultipleOfFiveReached != null))
            {
                OnMultipleOfFiveReached();
            }
            return isSum;
        }
    }

    public class DotNetEventAdder
    {
        public event EventHandler OnMultipleOfFiveReached;

        public int Add(int x, int y)
        {
            int isSum = x + y;
            if ((isSum % 5 == 0) && (OnMultipleOfFiveReached != null))
            {
                OnMultipleOfFiveReached(this, EventArgs.Empty);
            }
            return isSum;
        }
    }
}
