using System.ComponentModel;
using System.Text;

namespace ListMeetings.View
{
    public class HeadTable<T> where T : class
    {
       public void GetHeadTable(int wightColumn = 20)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            StringBuilder stringBuilder = new StringBuilder();
            // C помощью рефлексии получаем список свойств класса, а также их атрибуты DisplayName для названия столбцов
            foreach (var prop in typeof(T).GetProperties())
            {

                foreach (DisplayNameAttribute attr in prop.GetCustomAttributes(typeof(DisplayNameAttribute), false))
                {
                    stringBuilder.Append("|" + attr.DisplayName.PadRight(wightColumn) + "|");
                }
            }
            Console.WriteLine(stringBuilder.ToString()); //отображаем название столбцов
            Console.WriteLine("".PadRight(stringBuilder.ToString().Count(), '-')); //делам  подчеркивание шапки
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
