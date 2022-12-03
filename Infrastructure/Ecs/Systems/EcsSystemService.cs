using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DefaultEcs.System;
using DependencySorter;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Ecs
{
    public class EcsSystemService : IEcsSystemService
    {
        private readonly IServiceProvider _serviceProvider;

        private List<ISystem<float>> _registeredSystems = new List<ISystem<float>>();

        public EcsSystemService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public SequentialSystem<float> GetRegisteredSystems()
        {
            return new SequentialSystem<float>(_registeredSystems.AsEnumerable<ISystem<float>>());
        }

        public bool HasSystems()
        {
            return _registeredSystems.Count > 0;
        }
    
        public void ProcessSystems(float delta)
        {
            var systems = GetRegisteredSystems();
            systems.Update(delta);
        }

        private void RegisterSystem(ISystem<float> system)
        {
            _registeredSystems.Add(system);
        }

        public void LoadUnregisteredSystems()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => typeof(IWatcher).IsAssignableFrom(p) && p.IsClass);

            var unregisteredSystems = new Collection<ValueTuple<Type,Type[]>>();
            foreach (var type in types)
            {
                var watcherInstance = (IWatcher)ActivatorUtilities.CreateInstance(_serviceProvider, type);
                var dependencies = watcherInstance.GetDependencies();
                var props = new ValueTuple<Type,Type[]>(type, dependencies);
                unregisteredSystems.Add(props);
            }

            var sortedSystems = SortSystems(unregisteredSystems);
            foreach (var system in sortedSystems)
            {
                var systemInstance = ActivatorUtilities.CreateInstance(_serviceProvider, system) as ISystem<float>;
                RegisterSystem(systemInstance);
            }
        }

        private List<Type> SortSystems(Collection<ValueTuple<Type,Type[]>> unregisteredSystems)
        {
            var unsortedWatchers = new DependencyCollection<Type>();
            foreach (var unregisteredSystem in unregisteredSystems)
            {
                if (unregisteredSystem.Item2.Length > 0)
                    unsortedWatchers.Add(unregisteredSystem.Item1, unregisteredSystem.Item2);
                else
                    unsortedWatchers.Add(unregisteredSystem.Item1);
            }

            return unsortedWatchers.ToList(); 
        }
    }
}