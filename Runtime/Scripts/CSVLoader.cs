using System.Text.RegularExpressions;
using System;
using System.Text;

namespace CZToolKit.LocalizationText
{
    public static class CSVLoader
    {
        private const char lineSperator = '\n';
        private const char surround = '"';
        private static string fieldSperator = "\",\"";
        private static Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        public static string SerializeTableLine(string[] _fields)
        {

            for (int f = 0; f < _fields.Length; f++)
            {
                if (string.IsNullOrEmpty(_fields[f]))
                    _fields[f] = "";
                else
                    _fields[f] = _fields[f].Replace("\"", "\"\"");
            }
            return string.Concat("\"", string.Join(fieldSperator, _fields), "\"");
        }

        public static string SerializeTable(string[][] _dataTable)
        {
            StringBuilder sb = new StringBuilder();
            string line;
            for (int lineIndex = 0; lineIndex < _dataTable.Length; lineIndex++)
            {
                sb.AppendLine(SerializeTableLine(_dataTable[lineIndex]));
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
                if (string.IsNullOrEmpty(lines[i])) continue;
                string[] fields = DeserializeTableLine(lines[i]);
                everyLineCallback(fields);
                dataTable[i] = fields;
            }
        }
    }
}
