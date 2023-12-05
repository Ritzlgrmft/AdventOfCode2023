using System.Dynamic;

namespace AdventOfCode2023.Day3a;

public class Worker : IWorker
{
    public long DoWork(string inputFile)
    {
        var numbers = new List<Number>();
        var symbols = new List<Symbol>();
        var row = 0;
        foreach (var line in File.ReadLines(inputFile))
        {
            var col = 0;
            var isNumberParsing = false;
            var currentNumber = 0;
            var startCol = 0;
            foreach (var c in line.ToCharArray())
            {
                if (char.IsDigit(c))
                {
                    if (!isNumberParsing)
                    {
                        startCol = col;
                        currentNumber = c - '0';
                    }
                    else
                    {
                        currentNumber = currentNumber * 10 + c - '0';
                    }
                    isNumberParsing = true;
                }
                else
                {
                    if (isNumberParsing)
                    {
                        numbers.Add(new Number { row = row, col = startCol, length = col - startCol, value = currentNumber });
                        isNumberParsing = false;
                    }

                    if (c != '.')
                    {
                        symbols.Add(new Symbol { row = row, col = col, value = c });
                    }
                }

                col++;
            }

            if (isNumberParsing)
            {
                numbers.Add(new Number { row = row, col = startCol, length = col - startCol, value = currentNumber });
                isNumberParsing = false;
            }

            row++;
        }

        var sum = 0;
        foreach (var number in numbers)
        {
            if (symbols.Any(s => Enumerable.Range(number.row - 1, 3).Contains(s.row) && Enumerable.Range(number.col - 1, number.length + 2).Contains(s.col)))
            {
                sum += number.value;
            }
        }
        return sum;
    }
}

struct Number
{
    public int row;
    public int col;
    public int length;
    public int value;
}

struct Symbol
{
    public int row;
    public int col;
    public char value;
}