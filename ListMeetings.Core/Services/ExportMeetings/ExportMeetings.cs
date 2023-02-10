using System.ComponentModel;

namespace ListMeetings.Core.Services.ExportMeetings
{
    public class Export<T> : IExportEntity<T> where T : class
    {
        

        public void StartExportToFile(List<T> model) 
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //Для выделения пути к каталогу, воспользуйтесь `System.IO.Path`:
            var path = Path.GetDirectoryName(location);

            var file = new FileInfo("Text.txt");

            using (var writer = new StreamWriter(file.FullName))
            {
                foreach (var item in model)
                {
                    var line = GetDataString(item);
                    writer.WriteLine(line);

                }

                writer.Close();
            }

        }


        private string GetDataString(T obj)
        {
            string line = String.Empty;
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                foreach (DisplayNameAttribute attr in property.GetCustomAttributes(typeof(DisplayNameAttribute), false))
                {
                    line += attr.DisplayName + ": " + property.GetValue(obj).ToString() + "; ";
                }
            }
            return line;
        }


    }
}

