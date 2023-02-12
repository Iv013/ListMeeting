using ListMeeting.Models.Models;
using ListMeetings.Core.DataMapper;

namespace TestListMeetings
{
    internal class TestDataMapper
    {
        MeetingDTO meetingDTO;
        Meeting meeting;
       [SetUp]
        public void Setup()
        {
             meetingDTO = new MeetingDTO
            {
                Id = 11,
                DateTimeEndMeeting = DateTime.Now + TimeSpan.FromMinutes(30) + TimeSpan.FromDays(1),
                NameMeeting = "Name",
                DateTimeStartMeeting = DateTime.Now + TimeSpan.FromDays(1),
                TimeReminder = 10
            };

            meeting = new Meeting
            { Id = 12,
                NameMeeting = "Name 2",
                DurationMeeting = 60,
                DateTimeStartMeeting = DateTime.Now + TimeSpan.FromMinutes(2),
                TimeReminder = 10
            };


        }
        [Test]

        public void TestCreateModelGetNull()
        {
            DataMapper dataMapper = new DataMapper();
            Assert.IsNull(dataMapper.CreateDomainModel(null));
            Assert.IsNull(dataMapper.CreateDTO(null));
        }
        [Test]

        public void TestCreateDomainModel()
        {
            DataMapper dataMapper = new DataMapper();
            Assert.IsTrue(dataMapper.CreateDomainModel(meetingDTO).NeedToRemind);
    
            Assert.That(dataMapper.CreateDomainModel(meetingDTO).Id, Is.EqualTo(11));

           var meetingDTO2 = new MeetingDTO
            {
                Id = 1,
                DateTimeEndMeeting = DateTime.Now + TimeSpan.FromMinutes(30) + TimeSpan.FromDays(1),
                NameMeeting = "Name2",
                DateTimeStartMeeting = DateTime.Now + TimeSpan.FromDays(1),
                TimeReminder = 0
            };

            Assert.IsFalse(dataMapper.CreateDomainModel(meetingDTO2).NeedToRemind);
        }

        public void TestCreateDTOModel()
        {
            DataMapper dataMapper = new DataMapper();
            Assert.That(dataMapper.CreateDTO(meeting).Id, Is.EqualTo(12));
            Assert.That(dataMapper.CreateDTO(meeting).NameMeeting, Is.EqualTo("Name 2"));
        }

    }
}
