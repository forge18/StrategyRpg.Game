using System;

namespace Infrastructure.HubMediator
{
    public interface IQueryFactory
    {
        Type GetQueryType(QueryTypeEnum queryHandlerEnum);
        IQueryHandler CreateInstance(Type type);
    }
}