using System;
using System.Collections.Generic;

namespace Infrastructure.GameLoop
{
    public class WatcherRegistration
    {
        public Type Value { get; set; }
        public List<Type> DependsOn { get; set; }

        public WatcherRegistration(Type value, List<Type> dependsOn = null)
        {
            Value = value;
            DependsOn = dependsOn;
        }
    }
}