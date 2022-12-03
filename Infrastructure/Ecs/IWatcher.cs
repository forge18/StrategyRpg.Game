using System;

namespace Infrastructure.Ecs
{
    public interface IWatcher
    {
        void Update(float elapsedTime);
        void Dispose();
        Type[] GetDependencies();
    }
}