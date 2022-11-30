using DefaultEcs.System;

namespace Infrastructure.Ecs
{
    public class Watcher : ISystem<float>
    {
        public virtual bool IsEnabled { get; set; } = true;
        public virtual void Update(float elapsedTime) { }
        public virtual void Dispose() { }
    }
}