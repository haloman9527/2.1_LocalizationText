using System.Text.RegularExpressions;
using System;
using System.Text;

namespace CZToolKit.LocalizationText
{
    public class CSVLoader
    {
        private static char lineSperator = '\n';
        private static char surround = '"';
        private static string[] fieldSperator = { "\",\"" };
        private static Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        public static string SerializeTable(string[][] dataTable, Action<string> everyLineCallback = null)
        {
            StringBuilder sb = new StringBuilder();
            string line;
            bool callback = everyLineCallback != null;
            for (int lineIndex = 0; lineIndex < dataTable.Length; lineIndex++)
            {
                for (int f = 0; f < dataTable[lineIndex].Length; f++)
                {
                    if (string.IsNullOrEmpty(dataTable[lineIndex][f]))
                        dataTable[lineIndex][f] = "";
                    else
                        dataTable[lineIndex][f] = dataTable[lineIndex][f].Replace("\"", "\"\"");
                }
                line = string.Concat("\"", string.Join(fieldSperator[0], dataTable[lineIndex]), "\"");
                if (callback)
                    everyLineCallback(line);
                sb.AppendLine(line);
            }
            return sb.ToString();
        }

        public static string[] DeserializeTableLine(string line)
        {
            string[] fields = CSVParser.Split(line);
            if (fields.Length <= 0)
                return fields;
            for (int f = 0; f < fields.Length; f++)
            {
                fields[f] = fields[f].Substring(fields[f].IndexOf(surround) + 1);
                fields[f] = fields[f].Remove(fields[f].LastIndexOf(surround));
                fields[f] = fields[f].Replace("\"\"", "\"");
            }
            return fields;
        }

        public static void DeserializeTable(string text, Action<string[]> everyLineCallback)
        {
            string[] lines = text.Split(lineSperator);
            string[][] dataTable = new string[lines.Length][];
            bool callback = everyLineCallback != null;
            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                    continue;
                string[] fields = DeserializeTableLine(lines[i]);
                everyLineCallback(fields);
                dataTable[i] = fields;
            }
        }
        
    }
}
