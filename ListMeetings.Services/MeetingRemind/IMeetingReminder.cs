using ListMeeting.Models.Models;


namespace ListMeetings.Services.MeetingRemind
{
    public interface IMeetingReminder
    {
        IEnumerable<ServiceResponse> MakeReminder();
    }
}
