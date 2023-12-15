namespace AdventOfCode2023.Day13a;

public class Worker : IWorker
{

    public long DoWork(string inputFile)
    {
        // read input
        var notes = new List<string[]>();
        var note = new List<string>();
        foreach (var line in File.ReadLines(inputFile))
        {
            if (string.IsNullOrEmpty(line))
            {
                notes.Add(note.ToArray());
                note = new List<string>();
            }
            else
            {
                note.Add(line);
            }
        }
        notes.Add(note.ToArray());

        // analyze notes
        var sumCols = 0L;
        var sumRows = 0L;
        foreach (var item in notes)
        {
            sumRows += CheckForHorizontalReflection(item);
            sumCols += CheckForVerticalReflection(item);
        }

        return sumRows * 100 + sumCols;
    }

    private long CheckForHorizontalReflection(string[] note)
    {
        for (var row = 1; row < note.Length; row++)
        {
            var index = 0;
            while (row - index > 0 && row + index < note.Length && RowsAreEqual(note, row - index - 1, row + index))
            {
                index++;
            }
            if (row - index == 0 || row + index == note.Length)
            {
                return row;
            }
        }
        return 0;
    }

    private bool RowsAreEqual(string[] note, int row1, int row2)
    {
        return note[row1] == note[row2];
    }

    private long CheckForVerticalReflection(string[] note)
    {
        for (var col = 1; col < note[0].Length; col++)
        {
            var index = 0;
            while (col - index > 0 && col + index < note[0].Length && ColsAreEqual(note, col - index - 1, col + index))
            {
                index++;
            }
            if (col - index == 0 || col + index == note[0].Length)
            {
                return col;
            }
        }
        return 0;
    }

    private bool ColsAreEqual(string[] note, int col1, int col2)
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

