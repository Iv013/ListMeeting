using ListMeeting.Core.Repository;
using ListMeeting.Models.Models;
using ListMeetings.Core.Data;
using ListMeetings.Core.Services.DataMapper;
using ListMeetings.Core.Services.MeetingRemind;

namespace TestListMeetings
{
    internal class TestMeetingRemind
    {
        ConcurrentMeetingsList DataBase;
        DataMapper dataMapper;
        IMeetingRepository<Meeting, MeetingDTO> _meetingsRepo;


        [SetUp]
        public void Setup()
        {
            dataMapper = new DataMapper();

            DataBase = new ConcurrentMeetingsList();
            _meetingsRepo = new MeetingRepository(DataBase, dataMapper);

            DataBase.Add(new MeetingDTO
            {
                Id = 1,
                DateTimeEndEvent = DateTime.Now + TimeSpan.FromMinutes(50),
                NameMeeting = "Name",
                DateTimeStartEvent = DateTime.Now + TimeSpan.FromMinutes(1),
                TimeReminder = 10
            });
      
        }
        [Test]
        public void TestMeatingRemindSend()
        {
            var remind = new MeetingRemind(_meetingsRepo);
            Assert.That(remind.MakeRemind().First().Success, Is.EqualTo(100));
       
        }



    }

}
