using System;

namespace Infrastructure.MediatorNS.QueryManagement
{
    public interface IQueryResult
    {
        TReturn ConvertResultValue<TReturn>();
    }
}