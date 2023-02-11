
using ListMeeting.Models.Models;

namespace ListMeetings.View
{
    public class ActionsWithConsole
    {

        /// <returns>Возвращает объект MyEvent, подгатовленный для добавления в список </returns>
        public Meeting GetMeetingForAdd()
        {
            Meeting newEvent = new Meeting();
            newEvent.NameMeeting = GetNameMeeting();
            newEvent.DateTimeStartEvent = GetEnterDateOrTime(0).Date + GetEnterDateOrTime(1).TimeOfDay;
            //  Console.WriteLine("");
            newEvent.DurationEvent = GetDurationEvent();

            return newEvent;

        }

        /// <returns>Возвращает название встречи введенное в консоли</returns>
        string GetNameMeeting()
        {
            Console.Write(StringConst.EnterName);
            var result = Console.ReadLine();
            return result == "" ? "Новая встреча" : result;
        }

        /// <returns>Возвращает длительность встречи в минутах введенную с консоли</returns>
        int GetDurationEvent()
        {
            int duration;
            do
            {
                Console.Write(StringConst.EnterTimeDuration);
            } while (!int.TryParse(Console.ReadLine(), out duration) || duration < 0);

            return duration;
        }

        /// <returns>Возвращает выбранную дату или время</returns>
        public DateTime GetEnterDateOrTime(int type = 0)  //type =0 будет возвращать дату, type =1 - Время
        {
            Console.Write(type == 0 ? StringConst.EnterDate : StringConst.EnterTime);

            //класс выбора даты или времени
            ConsoleDateTimePicker consoleDatePickerMini = new ConsoleDateTimePicker(
                Console.GetCursorPosition().Left + 1,
                Console.GetCursorPosition().Top - 1,
                DateTime.Now, type);
            consoleDatePickerMini.Show();
            return consoleDatePickerMini.GetDate();
        }

        //Отрисовка стартовых сообщений и списка команд
        public void Initializind()  
            {
            Console.WriteLine(StringConst.ListComand);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(StringConst.AddTestMeetings);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
  
        public string ReadLine() => Console.ReadLine();
        public void WriteEnterCommand() => Console.Write(StringConst.EnterComand);



        //Формирование в консоли таблицы с данными о встречах
        public void ShowMeetings(List<Meeting> meetings)
        {
            //Отрисовываем шапку таблицы
            HeadTable< Meeting> headTable = new HeadTable<Meeting>   ();
            headTable.GetHeadTable();

            foreach (var meeting in meetings)
            {
                Console.WriteLine(meeting.ToString());
            }
            Console.WriteLine();
        }

        //Получения ID номера встречи которую нужно удалить или изменить
        public int GetIdForDeleteOrUpdate(int type = 0)
        {
            int Id;
            do
            {
                Console.Write(type == 0 ? StringConst.EnterIdForDelete : StringConst.EnterIdForUpdate); ;
            } while (!int.TryParse(Console.ReadLine(), out Id));
            return Id;
        }

        //Получения времени, за сколько минут нужно напомнить о встрече
        public int GetTimeReminder()
        {
            int timeReminder;
            do
            {
                Console.Write(StringConst.EnterTimeReminder);
            } while (!int.TryParse(Console.ReadLine(), out timeReminder));

            return timeReminder;
        }

        //Формирования объекта, который необходимо обновить
        internal Meeting GetMeetingForUpdate(in Meeting meeting)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(StringConst.UpdateQuestions);
            
            var obj = (Meeting)meeting.Clone();

            bool editmode = true;
            while (editmode)
            {
                ShowMeetings(new List<Meeting> { obj });
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(StringConst.EditModeEnterCommand);
                switch (Console.ReadLine())
                {
                    case "1":
                        obj.NameMeeting = GetNameMeeting();
                        break;
                    case "2":
                        obj.DateTimeStartEvent = GetEnterDateOrTime(0).Date + obj.DateTimeStartEvent.TimeOfDay;
                        break;
                    case "3":
                        obj.DateTimeStartEvent = obj.DateTimeStartEvent.Date + GetEnterDateOrTime(1).TimeOfDay;
                        break;
                    case "4":
                        obj.DurationEvent = GetDurationEvent();
                        break;
                    case "5":
                        obj.TimeReminder = GetTimeReminder();
                        break;
                    case "6":
                        if (!SaveOrNoСhanges())
                        {
                            obj = null;
                        }
                        editmode = false;
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                }

            }
            return obj;
        }


        //Запрос сохранять внесенные изменения или нет.
        private bool SaveOrNoСhanges()
        {
            string result;
            do
            {
                Console.Write(StringConst.SaveOrNo);
                result = Console.ReadLine();
            } while (result.ToLower() != "n" && result.ToLower() != "y");
            return result.ToLower() == "y";
        }


        //Отрисовака сообщений в звависимости от ответа  на команды CRUD, и напоминаий о встрече
        public void WriteMessage(ServiceResponse serviceResponse)
        {
            Console.ForegroundColor = serviceResponse.Success == 200 ? ConsoleColor.Green
                : serviceResponse.Success == 100 ? ConsoleColor.Yellow
                : ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(serviceResponse.Message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }


        //Запрос о необходимости экпорта данных в файл.
        public string? AskQuestionExportInFile()
        {
            string result;
            do
            {
                Console.Write(StringConst.ExportInFile);
                result = Console.ReadLine();
            } while (result.ToLower() != "n" && result.ToLower() != "y");
            if (result.ToLower() == "n") return null ;


            if   (result.ToLower() == "y")
            {
                Console.Write(StringConst.NameExportFile);
                result = Console.ReadLine();
            };
            return result;
        }

    }
}
