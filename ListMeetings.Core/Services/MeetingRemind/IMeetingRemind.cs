using ListMeeting.Models.Models;


namespace ListMeetings.Core.Services
{
    public interface IMeetingRemind
    {
        IEnumerable<ServiceResponse> MakeRemind();
    }
}
