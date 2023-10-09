namespace la_mia_pizzeria_static.CustomLoggers
{
    public class CustomFileLogger : ICustomLogger
    {
        public void WriteFileLog(string logText) 
        {
            File.AppendAllText("C:\\Users\\alexn\\Desktop\\Experis\\la-mia-pizzeria-crud-mvc\\la-mia-pizzeria-static\\log-file.txt", DateTime.Now + " - " + logText + "\n");
        }
    }
}
