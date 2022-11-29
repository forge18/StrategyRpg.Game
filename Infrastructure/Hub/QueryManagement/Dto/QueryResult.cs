using System;

namespace Infrastructure.HubMediator
{
    public class QueryResult
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
            if (ResultValue != null)
            {
                return (TReturn)ResultValue;
            }
            
            return default;
        }
    }
}