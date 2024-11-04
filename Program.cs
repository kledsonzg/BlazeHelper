using System.Diagnostics;
using BlazeHelper.Testing;
using Newtonsoft.Json;

namespace BlazeHelper
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void  Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1() );
        }

        internal static void Print(object? content)
        {

            var msg = content == null ? "null" : content is string ? content.ToString() : JsonConvert.SerializeObject(content);
            var now = DateTime.Now;
            var dateString = now.ToShortDateString();
            var timeString = now.ToShortTimeString() + (now.Second < 10 ? $":0{now.Second}" : $":{now.Second}");
            Console.WriteLine($"{dateString} {timeString} : {msg}");
            Debug.WriteLine($"{dateString} {timeString} : {msg}");
        }
    }
}