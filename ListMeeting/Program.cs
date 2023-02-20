

using Autofac;
using ListMeeting;
using ListMeeting.Core.Repository;
using ListMeeting.Models.Models;
using ListMeetings.Core.Data;
using ListMeetings.Core.DataMapper;
using ListMeetings.Core.Repository;
using ListMeetings.Services.ExportMeetings;
using ListMeetings.Services.MeetingRemind;
using ListMeetings.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

static class Program
{


    static void Main()
    {
        //класс в котором сдела потокобезопасное обращение в листу, замена базы данных в данном приложении
        ConcurrentMeetingsList DataBase = new ConcurrentMeetingsList();
       
        //класс перекладки из модели данных в модель DTO? и обратно 
        DataMapper dataMapper = new DataMapper();

        //класс для где консолидирована работа с консолью, ввод и вывод данных
        ActionsWithConsole actionWithConsole = new ActionsWithConsole();


        ApplicationDbContext context = new ApplicationDbContext();
        //объект реализации обмена  с базой данных. 
        // IMeetingRepository<Meeting, MeetingDTO> _meetingsRepo = new MeetingRepository(DataBase, dataMapper);
        IMeetingRepository<Meeting, MeetingDTO> _meetingsRepo = new MeetingRepositoryInMemory(context, dataMapper);
        //класс для формирования сообщений с напоминаниями о встрече
        IMeetingReminder meetingRemind = new MeetingReminder(_meetingsRepo); 

        //Класс, в котором с помощью рефлексии реализован экспорт параметров в файл.
        IExportEntity<Meeting> _exportEntity = new ExportEntity<Meeting>();

        //класс, который упрравляет основной логикой приложений, выполняет роль посредния, и исключает прямую связанность между другими классами  
        Mainlogic mainLogic = new Mainlogic(actionWithConsole, _meetingsRepo, meetingRemind, _exportEntity); 
        mainLogic.Runlogic();

    }

}