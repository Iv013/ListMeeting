using ListMeeting.Core.Repository;
using ListMeeting.Models.Models;

namespace ListMeetings.Services.MeetingRemind
{
    public class MeetingReminder : IMeetingReminder
    {
  
     IMeetingRepository<Meeting, MeetingDTO> _meetingsRepo;
        public MeetingReminder(IMeetingRepository<Meeting, MeetingDTO> meetingsRepo)
        {
            _meetingsRepo = meetingsRepo;

        }

        public IEnumerable<ServiceResponse> MakeReminder()
        {
          var  _dataBase = _meetingsRepo.GetAllMeetings();
                foreach (var meeting in _dataBase)
                {
           //проверяем если наступило время для напоминаия и взеден флаг о необходимости напомнить, формируем разовое напоминание
                    if (meeting.DateTimeStartMeeting - TimeSpan.FromMinutes(meeting.TimeReminder) < DateTime.Now
                        && meeting.NeedToRemind)

                    {
                        yield return new ServiceResponse
                        {
                            Message = $"Встреча номер {meeting.Id} " +
                            $"начнется {meeting.DateTimeStartMeeting.ToString("d")} в  {meeting.DateTimeStartMeeting.ToString("t")}",
                            Success = 100
                        };
                    //Сбрасываем необходимость напоминания
                        meeting.TimeReminder = 0;
                    _meetingsRepo.UpdateMeeting(meeting);
    
                    }
                }
           

        }
     }
 }
