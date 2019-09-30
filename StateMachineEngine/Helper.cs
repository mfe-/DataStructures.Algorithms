using System.IO;
using System.Threading.Tasks;

namespace StateMachineEngine
{
    public static class Helper
    {
        public static Task Wait(int totalMilliseconds)
        {
            return Task.Delay(totalMilliseconds);
        }
        public static string SaveStringToFile(string pathToFile, string content)
        {
            File.AppendAllText(pathToFile, content);
            return content;
        }
    }
}
