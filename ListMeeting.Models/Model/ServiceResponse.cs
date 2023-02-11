namespace ListMeeting.Models.Models
{
    //Класс, объекты которого будем возвращать как результат выполнения  различных операций.
    public class ServiceResponse
    {
        public int Success { get; set; } = 200;
        public string Message { get; set; } = string.Empty;
    }
}
