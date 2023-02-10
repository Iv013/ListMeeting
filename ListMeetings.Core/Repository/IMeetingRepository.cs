using ListMeeting.Models.Models;


namespace ListMeeting.Core.Repository
{
    public interface IMeetingRepository<T,T1>
    {
        List<T> GetAllMeetings(Func<T1, bool> dateFilter = null  );
        (T, ServiceResponse) FirstOfDefault(Func<T1, bool> filter);
        ServiceResponse AddMeeting(T entity);
        ServiceResponse RemoveMeeting(int IdEntity);
        ServiceResponse UpdateMeeting(T entity);
        
    }
}
