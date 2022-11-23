using System.Collections.Generic;

namespace Data.Resources
{
    public interface IBaseSchema
    {
        string GetDataId();
        Dictionary<string, SchemaTypeEnum> GetDependencies();
    }
}