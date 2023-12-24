using System.ComponentModel;
using System.Data;
using System.Net.Mail;

namespace AdventOfCode2023.Day17b;

public class Worker : IWorker
{
    const int MAX_STRAIGHT = 3;

    public long DoWork(string inputFile)
    {
        // read input
        var map = new List<List<int>>();
        foreach (var line in File.ReadLines(inputFile))
        {
            map.Add(line.ToCharArray().Select(n => n - 48).ToList());
        }

        var heatLossPerState = new Dictionary<(int Row, int Col, int Direction, int Distance), int>();
        var unvisitedStates = new List<(int HeatLoss, int Row, int Col, int Direction, int Distance)>(){
            (0, 0, 0, 90, 1),
            (0, 0, 0, 180, 1)
        };

        var current = unvisitedStates[0];
        while (current.Row != map.Count - 1 || current.Col != map[0].Count - 1 || current.Distance < 4)
        {
            unvisitedStates.Remove(current);

            if (current.Distance > 3)
            {
                AddUnvisitedNeighbor(current.HeatLoss, current.Row, current.Col, (current.Direction + 90) % 360, 1);
                AddUnvisitedNeighbor(current.HeatLoss, current.Row, current.Col, (current.Direction + 270) % 360, 1);
            }
            if (current.Distance < 10)
            {
                AddUnvisitedNeighbor(current.HeatLoss, current.Row, current.Col, current.Direction, current.Distance + 1);
            }

            current = unvisitedStates.OrderBy(s => s.HeatLoss).First();
        }

        return current.HeatLoss;

        void AddUnvisitedNeighbor(int currentHeatLoss, int currentRow, int currentCol, int direction, int distance)
        {
            int nextRow = currentRow, nextCol = currentCol;
            if (direction == 0) nextRow--;
            if (direction == 180) nextRow++;
            if (direction == 270) nextCol--;
            if (direction == 90) nextCol++;

            if (nextRow < 0 || nextRow >= map.Count) return;
            if (nextCol < 0 || nextCol >= map[0].Count) return;

            var nextHeatLoss = currentHeatLoss + map[nextRow][nextCol];
            var state = (nextRow, nextCol, direction, distance);

            if (!heatLossPerState.ContainsKey(state))
            {
                heatLossPerState[state] = nextHeatLoss;
                unvisitedStates.Add((nextHeatLoss, nextRow, nextCol, direction, distance));
            }
        }
    }
}