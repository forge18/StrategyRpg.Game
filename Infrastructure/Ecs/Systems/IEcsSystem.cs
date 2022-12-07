using System;

namespace Infrastructure.Ecs
{
    public interface IEcsSystem
    {
        void Update(float elapsedTime);
        void Dispose();
        Type[] GetDependencies();
    }
}