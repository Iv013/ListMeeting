namespace ListMeetings.Core.DataMapper
{
    public interface IDataMapper<T, T1>
    {
        T1 CreateDTO(T domainModel);
        T CreateDomainModel(T1 modelDTO);
    }
}
