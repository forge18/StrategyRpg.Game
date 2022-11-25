using System;

namespace Infrastructure.MediatorNS.QueryManagement
{
    public class QueryFactory : IQueryFactory
    {
        public IQuery CreateInstance(QueryTypeEnum queryType)
        {
            try
            {
                Type type = Type.GetType(queryType.ToString() + "Query");
                var command = (IQuery)Activator.CreateInstance(type);

                return command;
            }
            catch (Exception ex)
            {
                throw new Exception("QueryFactory.CreateInstance: " + ex.Message);
            }
        }
    }
}