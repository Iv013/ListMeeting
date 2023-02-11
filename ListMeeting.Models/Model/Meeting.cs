using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ListMeeting.Models.Models
{
    public class Meeting :ICloneable
    {
        [DisplayName("Номер записи")]
        public int Id { get; set; }

        [DisplayName ("Название")]
        [MaxLength(50)]
        [Required]
        public string NameMeeting { get; set; } = String.Empty;

        [DisplayName( "Начало встречи")]
        [Required]
        //добаляем логику, что при установке начального времени, устанавливаем и конечное время, в зависимости от длятельности встречи
        public DateTime DateTimeStartEvent 
        { get  { return _dateTimeStartEvent;}
          set
          {
                _dateTimeStartEvent = value;
                _dateTimeEndEvent = _dateTimeStartEvent + TimeSpan.FromMinutes(_durationEvent);

            }
        }

        [Required]
        [Range(1,1440)]
        //при изменении продолжительности встречи меняем конечное время
        public int DurationEvent
        {
            get
            {
                return _durationEvent; 
            }
            set
            {
                _dateTimeEndEvent = DateTimeStartEvent + TimeSpan.FromMinutes(value);
                _durationEvent = value;
            }
        }

        private DateTime _dateTimeEndEvent;
        private DateTime _dateTimeStartEvent;
        private int _durationEvent;
        private bool needToRemind = true;  
        private int _timeReminder = 5;

        [DisplayName("Окончание встречи")]
        public DateTime DateTimeEndEvent 
        {
            get 
            {
                return _dateTimeEndEvent; 
            }
        }

        [Range(1, 1440)]
        [DisplayName("Напомнить за")]
        public int TimeReminder 
        {
            get
            {
                return _timeReminder; 
            }
            set
            { 
                _timeReminder = value;
                needToRemind = !(_timeReminder == 0); //Есил время напоимнания установлено в 0, сбрасываем флаг необходимости напоминания
            }
        } 
        public bool NeedToRemind { get { return needToRemind; }  }


        
        //переопределяем метод, чтобы при выводе в консоль данные корректно выводились
        public override string ToString()  
        {
            StringBuilder stringBuilder =  new StringBuilder();
            stringBuilder.Append(getStringWithSpace(Id.ToString()));
            stringBuilder.Append(getStringWithSpace(NameMeeting));
            stringBuilder.Append(getStringWithSpace(DateTimeStartEvent.ToString("g"))); ;
            stringBuilder.Append(getStringWithSpace(DateTimeEndEvent.ToString("g")));
            stringBuilder.Append(getStringWithSpace(TimeReminder.ToString()+"мин"));

            return stringBuilder.ToString();
        }

        private string getStringWithSpace(string text, int maxlenght = 20)
        {
            if (text.Length > maxlenght - 4)
            {
                text = text.Substring(0, maxlenght - 4 ) + "...";
            }
            return ("|" + text.PadRight(maxlenght) + "|");
        }

        //реализуем  интерфейс IClonable, так как мы не использууем настоящую базу данных, при получении данных из листа будем получать ссылку
        //чтобы корректно отрабатывал алгоритм обновления, при обновлении данных будем вначале полуать запись, потом клонировать,
        //затем изменять данный клонированного объекта и затем подменять старую запись на новую.
        public object Clone()
        {
            return MemberwiseClone();
        }
    }

}
