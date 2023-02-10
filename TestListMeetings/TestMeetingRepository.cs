using ListMeeting.Core.Repository;
using ListMeeting.Models.Models;
using ListMeetings.Core.Data;
using ListMeetings.Core.Services.DataMapper;

namespace TestListMeetings
{
    public class TestMeetingRepository
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

            DataBase.Add(new MeetingDTO { Id  = 1, 
                DateTimeEndEvent = DateTime.Now + TimeSpan.FromMinutes(50) , 
                NameMeeting = "Name", 
                DateTimeStartEvent=DateTime.Now, 
                TimeReminder = 10});
            DataBase.Add(new MeetingDTO
            {
                Id = 11,
                DateTimeEndEvent = DateTime.Now + TimeSpan.FromMinutes(30) + TimeSpan.FromDays(1),
                NameMeeting = "Name",
                DateTimeStartEvent = DateTime.Now + TimeSpan.FromDays(1),
                TimeReminder = 10
            });
        }

        [Test]
        public void TestGetFirstOfDefault()
        {
          
           


            Assert.IsNull(_meetingsRepo.FirstOfDefault(x => x.Id == 2).Item1);
            Assert.NotNull(_meetingsRepo.FirstOfDefault(x => x.Id == 1).Item1);
            Assert.That(_meetingsRepo.FirstOfDefault(x => x.Id == 1).Item2.Success, Is.EqualTo(200));
            Assert.That(_meetingsRepo.FirstOfDefault(x => x.Id == 2).Item2.Success, Is.EqualTo(404));
        }



        [Test] 
        //Тестируем добваление записи, ожидаем ошибку
        public void TestAddMeetingWithError()
        {

           


            var newObj = new Meeting { NameMeeting = "Name 2",
                DurationEvent = 60,
                DateTimeStartEvent = DateTime.Now+TimeSpan.FromMinutes(2),
                TimeReminder = 10
            };
            //Добавляем запись с временем которое пересекается с имеющейся записью, ожиаем ошибку, и количество записей не измениться
            Assert.That(_meetingsRepo.AddMeeting(newObj).Success, Is.EqualTo(409));       
            Assert.That(_meetingsRepo.AddMeeting(newObj).Message, Is.EqualTo(StringConst.ErrorAdd + StringConst.ErrorMeetingsIntersection));
            Assert.That(DataBase.Count(), Is.EqualTo(2));

             newObj = new Meeting
            {
                NameMeeting = "Name 2",
                DurationEvent = 10,
                DateTimeStartEvent = DateTime.Now - TimeSpan.FromMinutes(40),
                TimeReminder = 10
            };
            //Добавляем запись с временем из прошлого, ожиаем ошибку, и количество записей не измениться
            Assert.That(_meetingsRepo.AddMeeting(newObj).Success, Is.EqualTo(409));
            Assert.That(_meetingsRepo.AddMeeting(newObj).Message, Is.EqualTo(StringConst.ErrorAdd + StringConst.ErrorMeetingInPast));
            Assert.That(DataBase.Count(), Is.EqualTo(2));

        }

        [Test]
        //Тестируем добваление записи, ожидаем ошибку
        public void TestAddMeetingWithSuccess()
        {

            var newObj = new Meeting
            {
                NameMeeting = "Name 2",
                DurationEvent = 60,
                DateTimeStartEvent = DateTime.Now + TimeSpan.FromMinutes(120),
                TimeReminder = 10
            };
            //Добавляем запись ожидаем корректное жоавбение и количество записей будет 2
            Assert.That(_meetingsRepo.AddMeeting(newObj).Success, Is.EqualTo(200));
            Assert.That(DataBase.Count(), Is.EqualTo(3));

             newObj = new Meeting
            {
                NameMeeting = "Name 3",
                DurationEvent = 30,
                DateTimeStartEvent = DateTime.Now + TimeSpan.FromMinutes(180),
                TimeReminder = 10
            };
            //Добавляем запись ожидаем корректное жоавбение и количество записей будет 3
            Assert.That(_meetingsRepo.AddMeeting(newObj).Message, Is.EqualTo(StringConst.AddSuccess));
            Assert.That(DataBase.Count(), Is.EqualTo(4));

        }

        //Тестируем удаление записи, ожидаем ошибку
        [Test]
        public void TestDeleteMeetingWithError()
        {

     
            //Пытаемся удалить запись под номером 10, ожидаем сообщение об ошибке
            Assert.That(_meetingsRepo.RemoveMeeting(10).Success, Is.EqualTo(409));

            Assert.That(_meetingsRepo.RemoveMeeting(10).Message, Is.EqualTo(StringConst.ObjectNotFound));
            Assert.That(DataBase.Count(), Is.EqualTo(2));

        }
        //Тестируем удаление записи
        [Test]
        public void TestDeleteMeetingWithSuccess()
        {

            //Пытаемся удалить запись под номером 1, ожидаем успех
            Assert.That(_meetingsRepo.RemoveMeeting(1).Success, Is.EqualTo(200));
            Assert.That(DataBase.Count(), Is.EqualTo(1));

            //Пытаемся удалить запись под номером 11, ожидаем успех
            Assert.That(_meetingsRepo.RemoveMeeting(11).Message, Is.EqualTo(StringConst.DelSuccess));
            Assert.That(DataBase.Count(), Is.EqualTo(0));

        }

        [Test]
        public void TestUpdateMeetingWithError()
        {
            var obj = _meetingsRepo.FirstOfDefault(x => x.Id == 1);
            obj.Item1.DateTimeStartEvent = DateTime.Now - TimeSpan.FromMinutes(10);

           
            Assert.That(_meetingsRepo.UpdateMeeting(obj.Item1).Success, Is.EqualTo(409));
            // ожидаем что запись не обновиться так как дата начала в прошлом
          Assert.That(_meetingsRepo.UpdateMeeting(obj.Item1).Message, Is.EqualTo(StringConst.ErrorUpdate + StringConst.ErrorMeetingInPast));

            obj.Item1.DateTimeStartEvent = DateTime.Now + TimeSpan.FromDays(1);

            // ожидаем что запись не обновиться так как даты пересекаются
            Assert.That(_meetingsRepo.UpdateMeeting(obj.Item1).Message, Is.EqualTo(StringConst.ErrorUpdate + StringConst.ErrorMeetingsIntersection));

        }
        [Test]
        public void TestUpdateMeetingWithSuccess()
        {
            var obj = _meetingsRepo.FirstOfDefault(x => x.Id == 1);
            obj.Item1.DateTimeStartEvent = DateTime.Now + TimeSpan.FromMinutes(10);


            Assert.That(_meetingsRepo.UpdateMeeting(obj.Item1).Success, Is.EqualTo(200));

            obj.Item1.NameMeeting = "Изменено";
            // ожидаем что запись не обновиться так как дата начала в прошлом
            Assert.That(_meetingsRepo.UpdateMeeting(obj.Item1).Message, Is.EqualTo(StringConst.UpdateSuccess));



            obj.Item1.DateTimeStartEvent = DateTime.Now + TimeSpan.FromDays(3);
            _meetingsRepo.UpdateMeeting(obj.Item1);

            Assert.AreEqual("Изменено", _meetingsRepo.FirstOfDefault(x => x.Id == 1).Item1.NameMeeting);



        }
    }
}
