using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class RowPrinter
    {
        // פונקציה שמקבלת רשימת שורות (כל שורה היא מילון של עמודות וערכים)
        public static void Print(List<Dictionary<string, object>> rows)
        {
            // שלב 1: בדיקה אם הרשימה ריקה
            if (rows == null || rows.Count == 0)
            {
                Console.WriteLine("אין נתונים להציג.");
                return;
            }

            // שלב 2: שליפת שמות העמודות (keys מהמילון הראשון)
            List<string> columnNames = rows[0].Keys.ToList();

            // שלב 3: קביעת הרוחב הדרוש לכל עמודה כדי שהתצוגה תהיה מיושרת
            Dictionary<string, int> columnWidths = new Dictionary<string, int>();
            foreach (string column in columnNames)
            {
                int maxWidth = column.Length; // מתחילים מאורך שם העמודה

                // מחשבים את האורך המקסימלי של הערכים בעמודה הזו
                foreach (var row in rows)
                {
                    string value = row[column]?.ToString() ?? "";
                    if (value.Length > maxWidth)
                        maxWidth = value.Length;
                }

                columnWidths[column] = maxWidth + 2; // תוספת רווח לשיפור תצוגה
            }

            // שלב 4: הדפסת כותרות הטבלה
            foreach (string column in columnNames)
            {
                Console.Write(column.PadRight(columnWidths[column]));
            }
            Console.WriteLine();

            // שלב 5: קו מפריד מתחת לכותרות
            foreach (string column in columnNames)
            {
                Console.Write(new string('-', columnWidths[column]));
            }
            Console.WriteLine();

            // שלב 6: הדפסת כל שורה בטבלה
            foreach (var row in rows)
            {
                foreach (string column in columnNames)
                {
                    string value = row[column]?.ToString() ?? "";
                    Console.Write(value.PadRight(columnWidths[column]));
                }
                Console.WriteLine();
            }

            Console.WriteLine(); // שורת רווח לסיום
        }
    }
}
