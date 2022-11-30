using System.Collections.Generic;
using Infrastructure;
using Infrastructure.GameLoop;

namespace Features.Global
{
    public class GlobalLoop : IGameLoop
    {
        private static List<WatcherRegistration> _watchers = new List<WatcherRegistration>
        {
            new WatcherRegistration(typeof(ProcessInputWatcher))
        };

        public List<WatcherRegistration> GetWatchers()
        {
            return _watchers;
        }
    }
}