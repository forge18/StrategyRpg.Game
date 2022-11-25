using System.Collections.Generic;
using Data.Resources;
using DefaultEcs;

namespace Data
{
    public interface IEcsDataLoader
    {
        Entity LoadResource(SchemaTypeEnum schema, string resourceName);
        void LoadDependencies(Dictionary<string, SchemaTypeEnum> dependencies);
    }
}