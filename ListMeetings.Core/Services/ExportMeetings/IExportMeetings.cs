namespace ListMeetings.Core.Services.ExportMeetings
{
    public interface IExportEntity<T> where T : class
    {
        void StartExportToFile(List<T> model);
    }
}
