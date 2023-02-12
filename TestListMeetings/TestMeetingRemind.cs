using ListMeeting.Core.Repository;
using ListMeeting.Models.Models;
using ListMeetings.Core.Data;
using ListMeetings.Core.DataMapper;
using ListMeetings.Services.MeetingRemind;

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
                DateTimeEndMeeting = DateTime.Now + TimeSpan.FromMinutes(50),
                NameMeeting = "Name",
                DateTimeStartMeeting = DateTime.Now + TimeSpan.FromMinutes(1),
                TimeReminder = 10
            });
      
        }
        [Test]
        public void TestMeatingRemindSend()
        {
            var remind = new MeetingReminder(_meetingsRepo);
            Assert.That(remind.MakeReminder().First().Success, Is.EqualTo(100));
       
        }



    }

}
