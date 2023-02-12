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
                DateTimeStartMeeting = DTO.DateTimeStartMeeting,
                DurationMeeting =(int)(DTO.DateTimeEndMeeting -  DTO.DateTimeStartMeeting).TotalMinutes,
                TimeReminder = DTO.TimeReminder,
            };
        }

        public MeetingDTO CreateDTO(Meeting domainModel)
        {
              if (domainModel == null) return null;
            return new MeetingDTO
            {
                Id = domainModel.Id,
                DateTimeStartMeeting = domainModel.DateTimeStartMeeting,
                DateTimeEndMeeting = domainModel.DateTimeEndMeeting,
                TimeReminder = domainModel.TimeReminder,
                NameMeeting = domainModel.NameMeeting,
            };
        }
    }
}
