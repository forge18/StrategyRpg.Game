using System;

namespace Infrastructure.Hub.QueryManagement
{
    public interface IQueryFactory
    {
        Type GetQueryType(QueryTypeEnum queryHandlerEnum);
        IQueryHandler CreateInstance(Type type);
    }
}