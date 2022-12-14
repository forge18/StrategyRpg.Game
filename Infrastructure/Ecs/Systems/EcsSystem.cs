using System;
using DefaultEcs.System;

namespace Infrastructure.Ecs
{
    public class EcsSystem : IEcsSystem, ISystem<float>
    {
        public virtual bool IsEnabled { get; set; } = true;
        public virtual void Update(float elapsedTime) { }
        public virtual void Dispose() { }
        public virtual Type[] GetDependencies() 
        { 
            return new Type[] { };
        }
    }
}