using System;

namespace Infrastructure.HubMediator
{
    public class QueryInstance
    {
        dynamic ResultValue { get; set; }
        Type ResultValueType { get; set; }

        public QueryInstance(dynamic resultValue, Type resultValueType)
        {
            ResultValue = resultValue;
            ResultValueType = resultValueType;
        }

        public TReturn ConvertResultValue<TReturn>()
        {
            return (TReturn)ResultValue;
        }
    }
}