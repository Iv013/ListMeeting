namespace ListMeeting.Models.Models
{
    public static class StringConst
    {
        public const string UpdateQuestions = $"Выберите параметры, которые вы хотите изменить\n" +
                $"   [1] - Название встречи\n" +
                $"   [2] - Дата начала\n" +
                $"   [3] - Время начала встречи\n" +
                $"   [4] - Длительность встречи \n" +
                $"   [5] - Время напоминания\n" +
                $"   [6] - Выйти и режима редактирования\n";


        public const string ListComand = $"Добрый день \n Реализованы следуюзин функции:\n" +
                $"  Команда [1] - Добавление записи\n" +
                $"  Команда [2] - Редиктирование записи\n" +
                $"  Команда [3] - Удаление записей\n" +
                $"  Команда [4] - Просмотр всех записей \n" +
                $"  Команда [5] - Просмотр записей за выбранную дату\n" +
                $"  Команда [6] - Заверщить программу";
        public const string AddTestMeetings = $"  Команда [7] - Добавить тестовые записи";
        public const string EnterComand = "\nВведите команду([?] - список команд):";
        public const string EnterDate = "\nВыберите дату начала  с помощью стрелок клавиатуры:";
        public const string EnterTime = "\n\nВыберите время начала  с помощью стрелок клавиатуры";
        public const string EnterName = "\nВведите описание события:";
        public const string EnterTimeReminder = "\nВведите время напоминания о встрече:";
        public const string EnterTimeDuration = "\n\nВведите продолжительность встречи в минутах(больше 0):";
        public const string EnterIdForUpdate = "\nВведите номер записи которую хотите изменить:";
        public const string EnterIdForDelete = "\nВведите номер записи которую хотите удалить:";
        public const string ObjectNotFound = "\nОшибка, запись с данным номером отсутствует в базе:";
        public const string EditModeEnterCommand = "\nРежим редактирования. Чтобы выбрать параметр для изменения введите команду:";
        public const string SaveOrNo = "\nСохранить введенные изменения(Y/N):";
        public const string AddSuccess = "\nНовая встреча успешно добавлена";
        public const string DelSuccess = "\nЗапись успешно удалена";
        public const string UpdateSuccess = "\nЗапись успешно изменена";
        public const string ErrorMeetingsIntersection = " Данная встреча пересекается с другими по времени!";
        public const string ErrorMeetingInPast = " Вcтреча должна быть запланирована на будущее!";
        public const string ErrorUpdate = " \n Запись не обновлена";
        public const string ErrorAdd = " \n Запись не добавлена";
        public const string ExportInFile = " \n Экспортировать данные в файл ?(Y/N):";
        public const string NameExportFile = " \n Ввtдите имя файла:";
    }
}
