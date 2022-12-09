using System.Collections.Generic;
using DefaultEcs;

namespace Infrastructure.Ecs
{
    public interface IEcsEntityService
    {
        Entity CreateEntityInWorld(EcsWorldEnum worldName = EcsWorldEnum.Default);
        bool HasSchemaIdInWorld(string schemaId, EcsWorldEnum worldName = EcsWorldEnum.Default);
        void DestroyEntityInWorld(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default);
        void SetEntityEnabled(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default);
        void SetEntityDisabled(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default);
        bool IsEntityEnabled(int entityId, EcsWorldEnum worldName = EcsWorldEnum.Default);
        int ParseEntityId(Entity newEntity);
    }
}