using System;
using System.Threading;

namespace MiffyLiye.XConsole
{
    public class Wait
    {
        public static void Until(Func<bool> predicate, int retryLimit = 10, TimeSpan retryInterval = default(TimeSpan), string label = null)
        {
            retryInterval = retryInterval == default(TimeSpan) ? TimeSpan.FromSeconds(0.1) : retryInterval;
            for (var retryCount = 0; retryCount < retryLimit; retryCount++)
            {
                if (predicate())
                {
                    return;
                }
                Thread.Sleep(retryInterval);
            }
            throw new TimeoutException(label ?? "Wait.Until timed out");
        }
    }
}