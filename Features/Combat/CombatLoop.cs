using System;
using System.Collections.Generic;
using Infrastructure;
using Infrastructure.GameLoop;
using Features.Combat.ArenaSetup;

namespace Features.Combat
{
    public class CombatLoop : IGameLoop
    {
        private static List<WatcherRegistration> _watchers = new List<WatcherRegistration>
        {
            new WatcherRegistration(typeof(NewArenaScenarioWatcher))
        };

        public List<WatcherRegistration> GetWatchers()
        {
            return _watchers;
        }
    }
}