using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListMeeting.Models.Models
{
    [Serializable]
    public class MeetingDTO

    {
        [DisplayName("Номер записи")]
        public int Id { get; set; }

        [DisplayName ("Название")]
        [MaxLength(50)]
        [Required]
        public string NameMeeting { get; set; } = String.Empty;

        [DisplayName( "Начало")]
        [Required]
        public DateTime DateTimeStartEvent { get; set; }

        [DisplayName("Окончание")]
        [Required]
        public DateTime DateTimeEndEvent {get; set;}

        [Range(1, 1440)]
        //[DisplayName("Напомнить за")]
        [Required]
        public int TimeReminder {get; set;}

       // public bool NeedToRemind { get; set; }

       
    }

}
