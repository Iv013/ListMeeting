using ListMeeting.Core.Repository;
using ListMeeting.Models.Models;
using ListMeetings.Services.ExportMeetings;
using ListMeetings.Services.MeetingRemind;
using ListMeetings.View;

namespace ListMeeting
{
    internal class Mainlogic
    {


        ActionsWithConsole _actionWithConsole;
        IMeetingRepository<Meeting, MeetingDTO> _meetingsRepo;
        IMeetingRemind _meetingRemind;
        public Mainlogic(ActionsWithConsole actionWithConsole,
            IMeetingRepository<Meeting, MeetingDTO> meetingsRepo,
            IMeetingRemind meetingRemind)
        {
            _actionWithConsole = actionWithConsole;
            _meetingsRepo = meetingsRepo;
            _meetingRemind = meetingRemind;
            //запускаем задачу на отправку напоминаний в рамках втроичного потока 
            Task snakeTask = SendRemind(_meetingRemind);
        }

        public async Task Runlogic()
        {

            _actionWithConsole.Initializind();
            bool runProgram = true;
            while (runProgram)
            {
                _actionWithConsole.WriteEnterCommand();

                switch (_actionWithConsole.ReadLine())
                {
                    case "1":  //Команда добавить
                        var entity = _actionWithConsole.GetMeetingForAdd();
                        var result = _meetingsRepo.AddMeeting(entity);
                        _actionWithConsole.WriteMessage(result);
                        break;
                    case "2": //Команда изменить
                              //получаем номер записи из консоли
                        var id = _actionWithConsole.GetIdForDeleteOrUpdate(1);
                        var meeting = _meetingsRepo.FirstOfDefault(x => x.Id == id);//проверяем наличие в базе
                        if (meeting.Item1 is null) //если записи нет выдаем сообщение и выходим
                        {
                            _actionWithConsole.WriteMessage(meeting.Item2);
                            break;
                        }
                        //получаем новую отредакитрованную запись из консоли
                        entity = _actionWithConsole.GetMeetingForUpdate(meeting.Item1);

                        if (entity != null)
                        {
                            result = _meetingsRepo.UpdateMeeting(entity);
                            _actionWithConsole.WriteMessage(result);
                        }
                        break;
                    case "3": //Команда удалить
                              //получаем номер записи из консоли
                        id = _actionWithConsole.GetIdForDeleteOrUpdate();
                        //удаляем запись, по результату выдаем сообщение об успехе или ошибке
                        result = _meetingsRepo.RemoveMeeting(id);
                        _actionWithConsole.WriteMessage(result);
                        break;
                    case "4": //Показать все записи
                        var meetings = _meetingsRepo.GetAllMeetings();
                        _actionWithConsole.ShowMeetings(meetings);
                        Export(meetings);


                        break;
                    case "5": //Показать  записи за определенную дату
                        var dateMeetings = _actionWithConsole.GetEnterDateOrTime().Date;
                        meetings = _meetingsRepo.GetAllMeetings(x => x.DateTimeStartEvent.Date == dateMeetings);
                        _actionWithConsole.ShowMeetings(meetings);
                        Export(meetings);
                        break;


                    case "?":
                        _actionWithConsole.Initializind();
                        break;
                    case "6":
                        runProgram = false;
                        break;
                    case "7":
                        addTestEvent(_meetingsRepo);
                        break;
                }

            }

        }

        void Export(List<Meeting> meetings)
        {
            if (meetings.Count > 0)
            {
                var nameFile = _actionWithConsole.AskQuestionExportInFile();
                if (nameFile != null)
                {
                    Export<Meeting> exportMeetings = new Export<Meeting>();
                    var result = exportMeetings.StartExportToFile(meetings, nameFile);

                    var ActionC = new ActionsWithConsole();
                    ActionC.WriteMessage(new ServiceResponse { Message = result.Item1, Success = result.Item2 ? 200 : 409 });
                }
            }
        }


        // Метод для добавления некоторых тестовых записей
        void addTestEvent(IMeetingRepository<Meeting, MeetingDTO> _meetingsRepo)
        {
            for (int i = 0; i < 10; i++)
            {
                _meetingsRepo.AddMeeting(new Meeting
                {
                    Id = i,
                    DateTimeStartEvent = DateTime.Now + TimeSpan.FromDays(i) + TimeSpan.FromMinutes(4),
                    DurationEvent = 120,
                    TimeReminder = 10,
                    NameMeeting = "Встреча №" + (i + 1).ToString(),
                });



            }


        }
        //метод выдачи уведомлений о предстоящей встрече
        // проверка выполняется каждые 10 сек,
        // при наличии нового сообщения оно выводиться в консоль
        static async Task SendRemind(IMeetingRemind eventRepository)
        {
            ActionsWithConsole viewInteface = new ActionsWithConsole();
            while (true)
            {
                await Task.Delay(10000);
                foreach (var meeting in eventRepository.MakeRemind())
                {
                    viewInteface.WriteMessage(meeting);
                }
            }
        }
    }
}
