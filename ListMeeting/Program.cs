

using ListMeeting;
using ListMeeting.Core.Repository;
using ListMeeting.Models.Models;
using ListMeetings.Core.Data;
using ListMeetings.Core.DataMapper;
using ListMeetings.Services.MeetingRemind;
using ListMeetings.View;

static class Program
{


    static async Task Main()
    {



        ConcurrentMeetingsList DataBase = new ConcurrentMeetingsList();
        DataMapper dataMapper = new DataMapper();
        ActionsWithConsole actionWithConsole = new ActionsWithConsole();
        IMeetingRepository<Meeting, MeetingDTO> _meetingsRepo = new MeetingRepository(DataBase, dataMapper);
        IMeetingRemind meetingRemind = new MeetingRemind(_meetingsRepo);

        Mainlogic biznes = new Mainlogic(actionWithConsole, _meetingsRepo, meetingRemind);

        biznes.Runlogic();

    }

}