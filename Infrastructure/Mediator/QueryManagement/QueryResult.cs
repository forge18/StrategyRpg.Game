using System;

namespace Infrastructure.MediatorNS.QueryManagement
{
    public class QueryResult : IQueryResult
    {
        QueryTypeEnum QueryType { get; set; }
        bool IsSuccessful { get; set; }
        dynamic ResultValue { get; set; }
        Type ResultValueType { get; set; }

        public QueryResult(QueryTypeEnum queryType, bool isSuccessful, dynamic resultValue, Type resultValueType)
        {
            QueryType = queryType;
            IsSuccessful = isSuccessful;
            ResultValue = resultValue;
            ResultValueType = resultValueType;
        }

        public TReturn ConvertResultValue<TReturn>()
        {
            return (TReturn)ResultValue;
        }
    }
}