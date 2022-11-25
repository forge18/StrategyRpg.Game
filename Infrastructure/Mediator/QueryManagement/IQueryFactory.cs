namespace Infrastructure.MediatorNS.QueryManagement
{
    public interface IQueryFactory
    {
        IQuery CreateInstance(QueryTypeEnum queryType);
    }
}