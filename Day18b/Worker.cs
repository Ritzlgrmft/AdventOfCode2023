using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Pipes;
using System.Net.Mail;
using AdventOfCode2023.Day4a;

namespace AdventOfCode2023.Day18b;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var corners = new List<(long Row, long Col)>() { (0L, 0L) };
        var currentRow = 0L;
        var currentCol = 0L;
        var perimeter = 0L;
        foreach (var line in File.ReadLines(inputFile))
        {
            var lineParts = line.Split(new char[] { ' ', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            var color = lineParts[2];
            var meters = Convert.ToInt64(color.Substring(1, 5), 16);
            var direction = color.Substring(6);

            switch (direction)
            {
                case "0":
                    // right
                    currentCol += meters;
                    break;
                case "1":
                    // down
                    currentRow += meters;
                    break;
                case "2":
                    // left
                    currentCol -= meters;
                    break;
                case "3":
                    // up
                    currentRow -= meters;
                    break;
            }

            perimeter += meters;
            corners.Add((currentRow, currentCol));
        }

        var area = Math.Abs(corners.Take(corners.Count - 1)
            .Select((p, i) => (corners[i + 1].Col - p.Col) * (corners[i + 1].Row + p.Row))
            .Sum() / 2);

        return area + perimeter / 2 + 1;
    }

}
