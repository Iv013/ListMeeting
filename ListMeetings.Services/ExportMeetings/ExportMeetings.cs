using System.ComponentModel;

namespace ListMeetings.Services.ExportMeetings
{
    public class Export<T> : IExportEntity<T> where T : class
    {
        

        public(string, bool) StartExportToFile(List<T> model, string nameFile) 
        {
       
            try
            {
                var file = new FileInfo(nameFile.Trim() == "" ? "Новый файл.txt" : nameFile.Trim()+".txt");
                using (var writer = new StreamWriter(file.FullName))
                {
                    foreach (var item in model)
                    {
                        var line = GetDataString(item);
                        writer.WriteLine(line);
                    }
                    writer.Close();
                }

                return ("Файл экспорта экпорта данных успешно создан. Файл находиться о адресу:\n" +
                    ""+ file.FullName,true);

            }
            catch ( Exception ex )
            {

                return (ex.Message, false);
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
                    line += attr.DisplayName + ": " + property.GetValue(obj).ToString() + ";   ";
                }
            }
            return line;
        }


    }
}

