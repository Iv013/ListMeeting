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
                needToRemind = !(_timeReminder == 0); 
            }
        } 
        public bool NeedToRemind { get { return needToRemind; }  }




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

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

}
