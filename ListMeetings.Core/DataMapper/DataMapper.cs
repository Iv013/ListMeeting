using ListMeeting.Models.Models;

namespace ListMeetings.Core.DataMapper
{
    public class DataMapper : IDataMapper<Meeting, MeetingDTO>
    {
        public Meeting CreateDomainModel(MeetingDTO DTO)
        {
            if (DTO == null)  return null;

            return new Meeting 
            { 
                Id = DTO.Id,
                NameMeeting = DTO.NameMeeting,
                DateTimeStartEvent = DTO.DateTimeStartEvent,
                DurationEvent =(int)(DTO.DateTimeEndEvent -  DTO.DateTimeStartEvent).TotalMinutes,
                TimeReminder = DTO.TimeReminder,
            };
        }

        public MeetingDTO CreateDTO(Meeting domainModel)
        {
              if (domainModel == null) return null;
            return new MeetingDTO
            {
                Id = domainModel.Id,
                DateTimeStartEvent = domainModel.DateTimeStartEvent,
                DateTimeEndEvent = domainModel.DateTimeEndEvent,
                TimeReminder = domainModel.TimeReminder,
                NameMeeting = domainModel.NameMeeting,
            };
        }
    }
}
