using ListMeeting.Models.Models;
using ListMeetings.Core.Data;
using ListMeetings.Core.Services.DataMapper;

namespace ListMeetings.Core.Services.MeetingRemind
{
    public class MeetingRemind : IMeetingRemind
    {
        private ConcurrentMeetingsList _dataBase;
        private IDataMapper<Meeting, MeetingDTO> _dataMapper;
        public MeetingRemind(ConcurrentMeetingsList dataBase,
         IDataMapper<Meeting, MeetingDTO> dataMapper)
        {
            _dataBase = dataBase;
            _dataMapper = dataMapper;
        }

        public IEnumerable<ServiceResponse> MakeRemind()
        {
                foreach (var meetingDTO in _dataBase)
                {
                    var meeting = _dataMapper.CreateDomainModel(meetingDTO);
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
                    meetingDTO.TimeReminder = 0;
                    }
                }
           

        }
     }
 }
