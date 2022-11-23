using System.Collections.Generic;
using Data.Resources;

namespace Data
{
    public interface IEcsDataLoader
    {
        void LoadResource<TSchema>(TSchema schema, string resourceName) where TSchema : IBaseSchema;
        void LoadDependencies(Dictionary<string, SchemaTypeEnum> dependencies);
    }
}