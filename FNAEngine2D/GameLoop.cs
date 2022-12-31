using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNAEngine2D
{
    /// <summary>
    /// Do a looping accurately
    /// </summary>
    public class GameLoop
    {
        //private TimeSpan _elapsedGameTime;
        private Stopwatch _gameTimer = null;
        private TimeSpan _accumulatedElapsedTime;
        private long _previousTicks = 0;
        private TimeSpan _targetElapsedTime = TimeSpan.FromTicks(166667); // 60fps
        private TimeSpan _maxElapsedTime = TimeSpan.FromMilliseconds(500);

        // must be a power of 2 so we can do a bitmask optimization when checking worst case
        private const int PREVIOUS_SLEEP_TIME_COUNT = 128;
        private const int SLEEP_TIME_MASK = PREVIOUS_SLEEP_TIME_COUNT - 1;
        private TimeSpan _worstCaseSleepPrecision = TimeSpan.FromMilliseconds(1);
        private TimeSpan[] _previousSleepTimes = new TimeSpan[PREVIOUS_SLEEP_TIME_COUNT];
        private int _sleepTimeIndex = 0;
        private int _updateFrameLag;
        private bool _isRunningSlowly;

        /// <summary>
        /// Action to run at each interval
        /// </summary>
        private Action _tickAction;

        /// <summary>
        /// ElapsedTime between frame
        /// </summary>
        public TimeSpan TargetElapsedTime { get { return _targetElapsedTime; } set { _targetElapsedTime = value; } }


        /// <summary>
        /// Indicate if server running
        /// </summary>
        public bool IsRunning { get; set; } = true;

        /// <summary>
        /// Indicate if the loop is running slowly
        /// </summary>
        public bool IsRunningSlowly { get { return _isRunningSlowly; } }

        /// <summary>
        /// Global game timer
        /// </summary>
        public Stopwatch GameTimer { get { return _gameTimer; } }


        /// <summary>
        /// Constructor
        /// </summary>
        public GameLoop(Action tickAction)
        {
            _tickAction = tickAction;

            _gameTimer = Stopwatch.StartNew();

            for (int i = 0; i < _previousSleepTimes.Length; i += 1)
            {
                _previousSleepTimes[i] = TimeSpan.FromMilliseconds(1);
            }
        }

        /// <summary>
        /// Start the server
        /// </summary>
        public void RunLoop()
        {

            while (this.IsRunning)
            {

                AdvanceElapsedTime();

                while (_accumulatedElapsedTime + _worstCaseSleepPrecision < _targetElapsedTime)
                {
                    System.Threading.Thread.Sleep(1);
                    TimeSpan timeAdvancedSinceSleeping = AdvanceElapsedTime();
                    UpdateEstimatedSleepPrecision(timeAdvancedSinceSleeping);
                }

                /* Now that we have slept into the sleep precision threshold, we need to wait
                 * for just a little bit longer until the target elapsed time has been reached.
                 * SpinWait(1) works by pausing the thread for very short intervals, so it is
                 * an efficient and time-accurate way to wait out the rest of the time.
                 */
                while (_accumulatedElapsedTime < _targetElapsedTime)
                {
                    System.Threading.Thread.SpinWait(1);
                    AdvanceElapsedTime();
                }


                // Do not allow any update to take longer than our maximum.
                if (_accumulatedElapsedTime > _maxElapsedTime)
                {
                    _accumulatedElapsedTime = _maxElapsedTime;
                }


                //_elapsedGameTime = _targetElapsedTime;
                int stepCount = 0;

                // Perform as many full fixed length time steps as we can.
                while (_accumulatedElapsedTime >= _targetElapsedTime)
                {
                    _accumulatedElapsedTime -= _targetElapsedTime;
                    stepCount += 1;

                    _tickAction();
                }

                // Every update after the first accumulates lag
                _updateFrameLag += Math.Max(0, stepCount - 1);

                /* If we think we are running slowly, wait
				 * until the lag clears before resetting it
				 */
                if (_isRunningSlowly)
                {
                    if (_updateFrameLag == 0)
                    {
                        _isRunningSlowly = false;
                    }
                }
                else if (_updateFrameLag >= 5)
                {
                    /* If we lag more than 5 frames,
					 * start thinking we are running slowly.
					 */
                    _isRunningSlowly = true;
                }

                /* Every time we just do one update and one draw,
				 * then we are not running slowly, so decrease the lag.
				 */
                if (stepCount == 1 && _updateFrameLag > 0)
                {
                    _updateFrameLag -= 1;
                }


                ///* Draw needs to know the total elapsed time
                // * that occured for the fixed length updates.
                // */
                //_elapsedGameTime = TimeSpan.FromTicks(_targetElapsedTime.Ticks * stepCount);

            }

        }


        /// <summary>
        /// Advanced the time in _accumulatedElapsedTime and _previousTicks
        /// </summary>
        private TimeSpan AdvanceElapsedTime()
        {
            long currentTicks = _gameTimer.Elapsed.Ticks;
            TimeSpan timeAdvanced = TimeSpan.FromTicks(currentTicks - _previousTicks);
            _accumulatedElapsedTime += timeAdvanced;
            _previousTicks = currentTicks;
            return timeAdvanced;
        }

        /// <summary>
        /// To calculate the sleep precision of the OS, we take the worst case
        /// time spent sleeping over the results of previous requests to sleep 1ms.
        /// </summary>
        private void UpdateEstimatedSleepPrecision(TimeSpan timeSpentSleeping)
        {
            /* It is unlikely that the scheduler will actually be more imprecise than
			 * 4ms and we don't want to get wrecked by a single long sleep so we cap this
			 * value at 4ms for sanity.
			 */
            TimeSpan upperTimeBound = TimeSpan.FromMilliseconds(4);

            if (timeSpentSleeping > upperTimeBound)
            {
                timeSpentSleeping = upperTimeBound;
            }

            /* We know the previous worst case - it's saved in _worstCaseSleepPrecision.
			 * We also know the current index. So the only way the worst case changes
			 * is if we either 1) just got a new worst case, or 2) the worst case was
			 * the oldest entry on the list.
			 */
            if (timeSpentSleeping >= _worstCaseSleepPrecision)
            {
                _worstCaseSleepPrecision = timeSpentSleeping;
            }
            else if (_previousSleepTimes[_sleepTimeIndex] == _worstCaseSleepPrecision)
            {
                TimeSpan maxSleepTime = TimeSpan.MinValue;
                for (int i = 0; i < _previousSleepTimes.Length; i += 1)
                {
                    if (_previousSleepTimes[i] > maxSleepTime)
                    {
                        maxSleepTime = _previousSleepTimes[i];
                    }
                }
                _worstCaseSleepPrecision = maxSleepTime;
            }

            _previousSleepTimes[_sleepTimeIndex] = timeSpentSleeping;
            _sleepTimeIndex = (_sleepTimeIndex + 1) & SLEEP_TIME_MASK;
        }


    }
}
