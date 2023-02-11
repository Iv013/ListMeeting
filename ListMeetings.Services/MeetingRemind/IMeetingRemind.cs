using ListMeeting.Models.Models;


namespace ListMeetings.Services.MeetingRemind
{
    public interface IMeetingRemind
    {
        IEnumerable<ServiceResponse> MakeRemind();
    }
}
