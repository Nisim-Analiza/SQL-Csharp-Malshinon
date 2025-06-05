using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class RowPrinter
    {
        public static void Print(List<Dictionary<string, object>> rows)
        {
            if (rows == null || rows.Count == 0)
            {
                Console.WriteLine("No data to display.");
                return;
            }

            var headers = rows[0].Keys.ToList();

            // חישוב רוחב כל עמודה
            var columnWidths = new Dictionary<string, int>();
            foreach (var header in headers)
            {
                int maxWidth = header.Length;
                foreach (var row in rows)
                {
                    var val = row[header]?.ToString() ?? "";
                    if (val.Length > maxWidth)
                        maxWidth = val.Length;
                }
                columnWidths[header] = maxWidth + 2;
            }

            // הדפסת כותרות
            foreach (var header in headers)
            {
                Console.Write(header.PadRight(columnWidths[header]));
            }
            Console.WriteLine();

            // הדפסת קו מפריד
            foreach (var header in headers)
            {
                Console.Write(new string('-', columnWidths[header]));
            }
            Console.WriteLine();

            // הדפסת נתונים
            foreach (var row in rows)
            {
                foreach (var header in headers)
                {
                    var val = row[header]?.ToString() ?? "";
                    Console.Write(val.PadRight(columnWidths[header]));
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}

