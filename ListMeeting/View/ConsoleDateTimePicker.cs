using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListMeetings.View
{
    class ConsoleDateTimePicker  //класс выбора даты или времени
    {
        public int Left { get; }
        public int Top { get; set; }
        public DateTime SelectedDate { get; private set; }
        public int TypePicker { get; set; }


    
        /// <param name="typePicker">0-data, 1 time</param>
        public ConsoleDateTimePicker(int left, int top, DateTime date, int typePicker=0)
        {
            Left = left;
            Top = top;
            SelectedDate = date;
            TypePicker = typePicker;

            Console.WriteLine();
            Console.WriteLine();
            Console.SetCursorPosition(Console.GetCursorPosition().Left, Console.GetCursorPosition().Top - 2);
        }

        public ConsoleDateTimePicker(int left, int top) : this(left, top, DateTime.Now) { }

        public ConsoleDateTimePicker() : this(0, 0) { }

        public void Show()
        {
            int oldLeft = Console.CursorLeft, oldTop = Console.CursorTop;
        
            ShowDate();

            Console.SetCursorPosition(oldLeft, oldTop);
        }

        public DateTime GetDate()
        {
            int oldLeft = Console.CursorLeft, oldTop = Console.CursorTop;
            bool cursorVisible = Console.CursorVisible;
            Console.CursorVisible = false;
            int f = 0;
            ConsoleKeyInfo key;
            do
            {
                ShowCursor(f);
                Console.SetCursorPosition(oldLeft, oldTop);
                key = Console.ReadKey();
                var step = TypePicker == 0 ? 3 : 2;


                if (key.Key == ConsoleKey.RightArrow) f = (f + 1) % step;
                if (key.Key == ConsoleKey.LeftArrow) f = (f + 2) % step;
                if (key.Key == ConsoleKey.UpArrow ||
                    key.Key == ConsoleKey.DownArrow)
                {
                    int additive = key.Key == ConsoleKey.UpArrow ? 1 : -1;
                    if (TypePicker == 0)
                    {
                        if (f == 0) SelectedDate = SelectedDate.AddDays(additive);
                        else if (f == 1) SelectedDate = SelectedDate.AddMonths(additive);
                        else if (f == 2) SelectedDate = SelectedDate.AddYears(additive);

                    } else
                    {
                        if (f == 1) SelectedDate = SelectedDate.AddMinutes(additive);
                        else if (f == 0) SelectedDate = SelectedDate.AddHours(additive);
                    }

                
                }
                ShowDate();
            }
            while (key.Key != ConsoleKey.Enter);
            Console.CursorVisible = cursorVisible;
            Console.SetCursorPosition(oldLeft, oldTop);
            return SelectedDate;
        }

        void ShowDate()
        {
            
            if (Console.WindowHeight - Top <= 2)
            {
                Top = Top -(Console.WindowHeight - Top);
            }
           

            Console.SetCursorPosition(Left, Top);
            Console.Write("          ");
            Console.SetCursorPosition(Left, Top + 1);
            Console.Write(SelectedDate.ToString(TypePicker == 0?"dd.MM.yyyy": "HH.mm"));
           
            
            Console.SetCursorPosition(Left, Top + 2);
            Console.Write("          ");

        }

        void ShowCursor(int field)
        {
            int offset = new[] { 1, 4, 9 }[field]; // 
            Console.SetCursorPosition(Left + offset, Top);
            Console.Write("+");
            Console.SetCursorPosition(Left + offset, Top + 2);
            Console.Write("-");
        }

    }
}
