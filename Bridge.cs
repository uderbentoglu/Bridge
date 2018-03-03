using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    static int initialBikeCount;
    static int bikeMustSurvive;
    static int roadLength;
    static List<string> result;
    static bool[,] road;
    static void Main(string[] args)
    {
        result = new List<string>();
        int M = int.Parse(Console.ReadLine()); // the amount of motorbikes to control
        int V = int.Parse(Console.ReadLine()); // the minimum amount of motorbikes that must survive
        initialBikeCount = M;
        bikeMustSurvive = V;

        string L0 = Console.ReadLine(); // L0 to L3 are lanes of the road. A dot character . represents a safe space, a zero 0 represents a hole in the road.
        string L1 = Console.ReadLine();
        string L2 = Console.ReadLine();
        string L3 = Console.ReadLine();

        roadLength = L0.Length;
        road = new bool[roadLength, 4];
        for (int i = 0; i < roadLength; i++)
        {
            road[i, 0] = L0[i] == '.' ? true : false;
            road[i, 1] = L1[i] == '.' ? true : false;
            road[i, 2] = L2[i] == '.' ? true : false;
            road[i, 3] = L3[i] == '.' ? true : false;
        }
        int cnt = 0;
        // game loop
        while (true)
        {
            int posX = 0;
            List<int> posY = new List<int>();
            int S = int.Parse(Console.ReadLine()); // the motorbikes' speed
            for (int i = 0; i < M; i++)
            {
                string[] inputs = Console.ReadLine().Split(' ');
                int X = int.Parse(inputs[0]); // x coordinate of the motorbike
                int Y = int.Parse(inputs[1]); // y coordinate of the motorbike
                int A = int.Parse(inputs[2]); // indicates whether the motorbike is activated "1" or detroyed "0"
                posX = X;
                posY.Add(Y);
            }
            if (cnt == 0)
            {
                Backtrack("", posX, posY, S, initialBikeCount);
            }
            Console.WriteLine(result.Count <= cnt ? result.Last() : result[cnt]);
            cnt++;
        }
    }

    static bool Backtrack(string command, int x, List<int> y, int speed, int bikeCount)
    {
        if (!string.IsNullOrEmpty(command))
        {
            result.Add(command);
        }

        int calculatedBikeCount = bikeCount;
        List<int> calculatedYPos = new List<int>();

        for (int i = 0; i < bikeCount; i++)
        {
            bool isHoleDetected = IsEncounteredHole(command, x, y[i], speed);
            if (isHoleDetected)
            {
                calculatedBikeCount--;
            }
            else
            {
                calculatedYPos.Add(y[i]);
            }
        }

        if (x >= roadLength - 1 && calculatedBikeCount >= bikeMustSurvive)
        {
            return true;
        }

        if ((calculatedBikeCount < bikeMustSurvive) || (speed == 0 && command == "SLOW"))
        {
            return false;
        }
        int calculatedX, newX = 0, calculatedSpeed = speed;
        calculatedX = x + calculatedSpeed >= roadLength - 1 ? roadLength - 1 : x + speed;

        // Speed
        calculatedSpeed = speed + 1 > 50 ? 50 : speed + 1;
        newX = x + calculatedSpeed >= roadLength - 1 ? roadLength - 1 : x + calculatedSpeed;
        if (Backtrack("SPEED", newX, calculatedYPos, calculatedSpeed, calculatedBikeCount))
        {
            return true;
        }
        else
        {
            result.RemoveAt(result.Count - 1);
        }

        //Slow
        calculatedSpeed = speed - 1 < 0 ? 0 : speed - 1;
        newX = x + calculatedSpeed >= roadLength - 1 ? roadLength - 1 : x + calculatedSpeed;
        if (Backtrack("SLOW", newX, calculatedYPos, calculatedSpeed, calculatedBikeCount))
        {
            return true;
        }
        else
        {
            if (result.Count > 0)
                result.RemoveAt(result.Count - 1);
        }

        //Jump
        if (Backtrack("JUMP", calculatedX, calculatedYPos, speed, calculatedBikeCount))
        {
            return true;
        }
        else
        {
            if (result.Count > 0)
                result.RemoveAt(result.Count - 1);
        }

        // Wait
        if (Backtrack("WAIT", calculatedX, calculatedYPos, speed, calculatedBikeCount))
        {
            return true;
        }
        else
        {
            if (result.Count > 0)
                result.RemoveAt(result.Count - 1);
        }

        // Up
        if (!calculatedYPos.Any(a => a == 0))
        {
            if (Backtrack("UP", calculatedX, calculatedYPos.Select(a => a = a - 1).ToList(), speed, calculatedBikeCount))
            {
                return true;
            }
            else
            {
                if (result.Count > 0)
                    result.RemoveAt(result.Count - 1);
            }
        }

        //Down
        if (!calculatedYPos.Any(a => a == 3))
        {
            if (Backtrack("DOWN", calculatedX, calculatedYPos.Select(a => a = a + 1).ToList(), speed, calculatedBikeCount))
            {
                return true;
            }
            else
            {
                if (result.Count > 0)
                    result.RemoveAt(result.Count - 1);
            }
        }

        return false;
    }

    static bool IsEncounteredHole(string command, int x, int y, int speed)
    {
        int prevX;
        switch (command)
        {
            case "SPEED":
                prevX = x - speed;
                if (prevX < 0)
                {
                    return false;
                }
                for (int i = prevX; i <= x; i++)
                {
                    if (!road[i, y])
                    {
                        return true;
                    }
                }
                break;
            case "SLOW":
                prevX = x - speed;
                if (prevX < 0)
                {
                    return false;
                }
                for (int i = prevX; i <= x; i++)
                {
                    if (!road[i, y])
                    {
                        return true;
                    }
                }
                break;
            case "WAIT":
                prevX = x - speed;
                if (prevX < 0)
                {
                    return false;
                }
                for (int i = prevX; i <= x; i++)
                {
                    if (!road[i, y])
                    {
                        return true;
                    }
                }
                break;
            case "UP":
                prevX = x - speed;
                if (prevX < 0)
                {
                    return false;
                }
                for (int i = 1; i <= x - prevX; i++)
                {
                    if (!road[prevX + i, y] || !road[x - i, y + 1])
                    {
                        return true;
                    }
                }
                break;
            case "DOWN":
                prevX = x - speed;
                if (prevX < 0)
                {
                    return false;
                }
                for (int i = 1; i <= x - prevX; i++)
                {
                    if (!road[prevX + i, y] || !road[x - i, y - 1])
                    {
                        return true;
                    }
                }
                break;
            case "JUMP":
                if (!road[x, y])
                {
                    return true;
                }
                break;
            default:
                return false;
        }
        return false;
    }
}
