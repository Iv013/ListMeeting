using ListMeeting.Models.Models;

namespace ListMeetings.Services.ExportMeetings
{
    public interface IExportEntity<T> where T : class
    {
        (string,bool) StartExportToFile(List<T> model, string nameFile);
    }
}
