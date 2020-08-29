using System;
using System.Collections.Generic;

namespace Visma
{
    /// <summary>
    /// Class implementing requirements from point (1) from Ex 1
    /// </summary>
    public class Document
    {
        public string Number { get; private set; }
        public int MaxSum;
        public DateTime Date;
        public List<Row> Rows;
        // IMPORTANT: Assuming here that no other fields can be added, 
        // otherwise a private "Row" instance would be added here
        // to store information about discarded row, which would result in
        // more maintainable code.

        public Document()
        {
            Date = DateTime.Now;
            // Rewind current date to current week's Monday
            // Modulus calculation necessary to get around US specific weekday
            // enumeration where Sunday is the first day of the week.
            TimeSpan daysSinceMonday = new TimeSpan( ((int)DateTime.Today.DayOfWeek + 6) % 7, 0, 0, 0);
            Date = DateTime.Today - daysSinceMonday;

            MaxSum = 600;
            Rows = new List<Row>();
            Console.WriteLine("Enter Document Number: ");
            Number = Console.ReadLine();
        }
        
        /// <summary>
        /// Method for executing point (3) from Ex 1
        /// </summary>
        public Row GenerateRowsUntilDiscard()
        {
            int remainingSum = MaxSum;
            string inputCode = String.Empty;
            int randomSum = 0;
            Random random = new Random();
            while (remainingSum > 0)
            {
                Console.WriteLine("Please input code for row, then press Enter: ");
                inputCode = Console.ReadLine();
                
                randomSum = random.Next(0, MaxSum / 3);
                remainingSum -= randomSum;

                Row inputRow = new Row(inputCode, randomSum);
                // If new rows can be added to document, add them to document
                if (remainingSum >= 0)
                { 
                    Rows.Add(inputRow);
                }
                // If maximum sum has been exceeded, return last row for calculation of 4.5
                // in PrintData() method, wihtout adding it to document's Rows list.
                else
                {
                    return inputRow;
                }
            }
            return null;
        }

        /// <summary>
        /// Method for executing point (4) from Ex 1 
        /// </summary>
        public void PrintData(Row discardedRow)
        {
            // 1) "Dokumenta datums, numurs un maksimālā summa"
            Console.WriteLine("Document date: " + Date.ToString("dd.MM.yyyy")
                            + ", Number: " + Number
                            + ", Max Sum: " + MaxSum.ToString());

            // 2) Rindu informācija (kods, summa)
            foreach (Row row in Rows)
            {
                Console.WriteLine("Row code: " + row.Code
                                + ", Row sum: " + row.Sum.ToString());
            }

            // 3) Kopējais dokumenta rindu skaits
            Console.WriteLine("Total number of rows: " + Rows.Count.ToString());

            // 4) Kopējā dokumenta summa
            int totalSum = 0;
            foreach (Row row in Rows)
            {
                totalSum += row.Sum;
            }
            Console.WriteLine("Document total sum: " + totalSum.ToString());

            // 5) Nepievienotās rindas summa un summa, par cik, 
            // to pievienojot dokumentam, tiktu pārsniegta dokumenta Maksimālā summa
            Console.WriteLine("Discarded row's sum: " + discardedRow.Sum.ToString()
                            + ", Document's max sum exceeded by " + (discardedRow.Sum - (MaxSum - totalSum) ).ToString());
        }
    }

    /// <summary>
    /// Class implementing requirements from point (2) from Ex 1
    /// </summary>
    public class Row
    {
        public string Code { get; private set; }
        public int Sum { get; private set; }
        public Row(string code, int sum)
        {
            Code = code;
            Sum = sum;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Document document = new Document();
            Row discardedRow = document.GenerateRowsUntilDiscard();
            document.PrintData(discardedRow);
            Console.ReadLine();
        }
    }
}
