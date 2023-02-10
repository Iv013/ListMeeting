

using ListMeeting.Core.Repository;
using ListMeeting.Models.Models;
using ListMeetings.Core.Data;
using ListMeetings.Core.Services;
using ListMeetings.Core.Services.DataMapper;
using ListMeetings.Core.Services.ExportMeetings;
using ListMeetings.Core.Services.MeetingRemind;
using ListMeetings.View;

static class Program
{


    static async Task Main()
    {



        ConcurrentMeetingsList DataBase = new ConcurrentMeetingsList();
        DataMapper dataMapper = new DataMapper();
        ActionsWithConsole actionWithConsole = new ActionsWithConsole();
        IMeetingRepository<Meeting, MeetingDTO> _meetingsRepo = new MeetingRepository(DataBase, dataMapper);
        IMeetingRemind meetingRemind = new MeetingRemind(DataBase, dataMapper);
        //запускаем задачу в рамках втроичного потока на отправку напоминаний
        Task snakeTask = SendRemind(meetingRemind);

        actionWithConsole.Initializind();
        bool runProgram = true;
        while (runProgram)
        {
            actionWithConsole.WriteEnterCommand();

            switch (actionWithConsole.ReadLine())
            {
                case "1":  //Команда добавить
                    var entity = actionWithConsole.GetMeetingForAdd();
                    var result = _meetingsRepo.AddMeeting(entity);
                    actionWithConsole.WriteMessage(result);
                    break;
                case "2": //Команда изменить
                    //получаем номер записи из консоли
                    var id = actionWithConsole.GetIdForDeleteOrUpdate(1);
                    var meeting = _meetingsRepo.FirstOfDefault(x => x.Id == id);//проверяем наличие в базе
                    if (meeting.Item1 is null) //если записи нет выдаем сообщение и выходим
                    {
                        actionWithConsole.WriteMessage(meeting.Item2);
                        break;
                    }
                    //получаем новую отредакитрованную запись из консоли
                    entity = actionWithConsole.GetMeetingForUpdate(meeting.Item1);

                    if (entity != null)
                    {
                        result = _meetingsRepo.UpdateMeeting(entity);
                        actionWithConsole.WriteMessage(result);
                    }
                    break;
                case "3": //Команда удалить
                    //получаем номер записи из консоли
                    id = actionWithConsole.GetIdForDeleteOrUpdate();
                    //удаляем запись, по результату выдаем сообщение об успехе или ошибке
                    result = _meetingsRepo.RemoveMeeting(id);
                    actionWithConsole.WriteMessage(result);
                    break;
                case "4": //Показать все записи
                    var meetings = _meetingsRepo.GetAllMeetings();
                    actionWithConsole.ShowMeetings(meetings);
                    Export<Meeting> exportMeetings = new Export<Meeting>();
                    exportMeetings.StartExportToFile(meetings);
                    break;
                case "5": //Показать  записи за определенную дату
                    var dateMeetings = actionWithConsole.GetEnterDateOrTime().Date;
                    meetings = _meetingsRepo.GetAllMeetings(x => x.DateTimeStartEvent.Date == dateMeetings);
                    actionWithConsole.ShowMeetings(meetings);

                    break;


                case "?":
                    Console.WriteLine(StringConst.ListComand);
                    break;
                case "6":
                    runProgram = false;
                    break;
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