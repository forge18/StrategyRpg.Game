using System.Collections.Generic;
using Infrastructure.GameLoop;

namespace Infrastructure
{
    public interface IGameLoop
    {
        List<WatcherRegistration> GetWatchers();
    }
}