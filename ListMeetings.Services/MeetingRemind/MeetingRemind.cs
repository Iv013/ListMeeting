using ListMeeting.Core.Repository;
using ListMeeting.Models.Models;

namespace ListMeetings.Services.MeetingRemind
{
    public class MeetingRemind : IMeetingRemind
    {
  
     IMeetingRepository<Meeting, MeetingDTO> _meetingsRepo;
        public MeetingRemind(IMeetingRepository<Meeting, MeetingDTO> meetingsRepo)
        {
            _meetingsRepo = meetingsRepo;

        }

        public IEnumerable<ServiceResponse> MakeRemind()
        {
          var  _dataBase = _meetingsRepo.GetAllMeetings();
                foreach (var meeting in _dataBase)
                {
           
                    if (meeting.DateTimeStartEvent - TimeSpan.FromMinutes(meeting.TimeReminder) < DateTime.Now
                        && meeting.NeedToRemind)

                    {
                        yield return new ServiceResponse
                        {
                            Message = $"Встреча номер {meeting.Id} " +
                            $"начнется {meeting.DateTimeStartEvent.ToString("d")} в  {meeting.DateTimeStartEvent.ToString("t")}",
                            Success = 100
                        };
                        meeting.TimeReminder = 0;
                    _meetingsRepo.UpdateMeeting(meeting);
    
                    }
                }
           

        }
     }
 }
