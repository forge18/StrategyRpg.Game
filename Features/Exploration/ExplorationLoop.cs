using System.Collections.Generic;
using Infrastructure;
using Infrastructure.GameLoop;
using Features.Exploration.Unit;
using System;

namespace Features.Exploration
{
    public class ExplorationSystems : IGameLoop
    {
        private static List<WatcherRegistration> _watchers = new List<WatcherRegistration>
        {
            new WatcherRegistration(typeof(PlayerVelocityWatcher), new List<Type> { typeof(SpawnUnitWatcher) }),
            new WatcherRegistration(typeof(SpawnUnitWatcher))
        };

        public List<WatcherRegistration> GetWatchers()
        {
            return _watchers;
        }
    }
}