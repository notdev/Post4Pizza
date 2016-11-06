using System.IO;

namespace Post4Pizza
{
    public static class DebugContent
    {
        public static void WriteToHtmlFile(string content)
        {
#if DEBUG
            File.WriteAllText("C:\\!work\\test.html", content);
#endif
        }
    }
}