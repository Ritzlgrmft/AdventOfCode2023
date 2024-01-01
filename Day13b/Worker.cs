namespace AdventOfCode2023.Day13b;

public class Worker : IWorker
{

    public long DoWork(string inputFile)
    {
        // read input
        var notes = new List<char[][]>();
        var note = new List<char[]>();
        foreach (var line in File.ReadLines(inputFile))
        {
            if (string.IsNullOrEmpty(line))
            {
                notes.Add(note.ToArray());
                note = new List<char[]>();
            }
            else
            {
                note.Add(line.ToCharArray());
            }
        }
        notes.Add(note.ToArray());

        // analyze notes
        var sum = 0L;
        foreach (var item in notes)
        {
            var isFixed = false;
            for (var row = 0; row < item.Length && !isFixed; row++)
            {
                for (var col = 0; col < item[row].Length && !isFixed; col++)
                {
                    var previousRowResult = CheckForHorizontalReflection(item, 0);
                    var previousColResult = CheckForVerticalReflection(item, 0);

                    var original = item[row][col];
                    item[row][col] = original == '.' ? '#' : '.';

                    var currentRowResult = CheckForHorizontalReflection(item, previousRowResult);
                    if (currentRowResult > 0)
                    {
                        sum += 100 * currentRowResult;
                        isFixed = true;
                    }

                    var currentColResult = CheckForVerticalReflection(item, previousColResult);
                    if (currentColResult > 0)
                    {
                        sum += currentColResult;
                        isFixed = true;
                    }

                    item[row][col] = original;
                }
            }
        }

        return sum;
    }

    private long CheckForHorizontalReflection(char[][] note, long previousRowResult)
    {
        for (var row = 1; row < note.Length; row++)
        {
            var index = 0;
            while (row - index > 0 && row + index < note.Length && RowsAreEqual(note, row - index - 1, row + index))
            {
                index++;
            }
            if ((row - index == 0 || row + index == note.Length) && row != previousRowResult)
            {
                return row;
            }
        }
        return 0;
    }

    private bool RowsAreEqual(char[][] note, int row1, int row2)
    {
        for (var col = 0; col < note[0].Length; col++)
        {
            if (note[row1][col] != note[row2][col])
            {
                return false;
            }
        }
        return true;
    }

    private long CheckForVerticalReflection(char[][] note, long previousColResult)
    {
        for (var col = 1; col < note[0].Length; col++)
        {
            var index = 0;
            while (col - index > 0 && col + index < note[0].Length && ColsAreEqual(note, col - index - 1, col + index))
            {
                index++;
            }
            if ((col - index == 0 || col + index == note[0].Length) && col != previousColResult)
            {
                return col;
            }
        }
        return 0;
    }

    private bool ColsAreEqual(char[][] note, int col1, int col2)
    {
        for (var row = 0; row < note.Length; row++)
        {
            if (note[row][col1] != note[row][col2])
            {
                return false;
            }
        }
        return true;
    }
}
