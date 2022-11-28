using System.Collections.Generic;
using UnityEngine;

public class PathFinder
{
    public class POINT
    {
        public int x;
        public int y;
        public List<POINT> path;

        public float magnitude()
        {
            return Mathf.Sqrt(x * x + y * y);
        }

        public POINT()
        {
        }

        public POINT(float X, float Y)
        {
            path = null;
            x = (int) X;
            y = (int) Y;
        }
    }

    public static void FindPathBFS(int startX, int startY, ref bool[,] visited, ref int[,] arr, POINT sizes, bool cheliks = false)
    {
        List<POINT> queue = new List<POINT>();
        List<POINT> nextQueue = new List<POINT>();

        POINT start = new POINT();
        start.x = startX;
        start.y = startY;
        start.path = new List<POINT>();
        start.path.Add(new POINT(startX, startY));

        queue.Add(start);

        visited[start.x, start.y] = true;
        int level = 0;

        while (queue.Count > 0)
        {
            var curr = queue[0];
            //if (curr.x == endX && curr.y == endY) return curr.path;

            arr[curr.x, curr.y] = level;
            CheckNext(1, 0, curr, ref nextQueue, ref visited, ref arr, sizes,cheliks);
            CheckNext(-1, 0, curr, ref nextQueue, ref visited, ref arr, sizes, cheliks);
            CheckNext(0, 1, curr, ref nextQueue, ref visited, ref arr, sizes, cheliks);
            CheckNext(0, -1, curr, ref nextQueue, ref visited, ref arr, sizes, cheliks);
            /*
            CheckNext(1,1, curr, ref nextQueue, ref visited, ref arr, sizes);
            CheckNext(-1,1, curr, ref nextQueue, ref visited, ref arr, sizes);
            CheckNext(-1,1, curr, ref nextQueue, ref visited, ref arr, sizes);
            CheckNext(1,-1, curr, ref nextQueue, ref visited, ref arr, sizes);*/

            queue.RemoveAt(0);

            // Next Level
            if (queue.Count == 0)
            {
                level++;
                queue = new List<POINT>(nextQueue);
                nextQueue.Clear();
            }
        }
    }

    public static List<POINT> FindPathBFS(POINT start, POINT end, ref int[,] arr, POINT sizes)
    {
        bool[,] visited = new bool[sizes.x, sizes.y];
        return FindPathBFS(start, end, ref visited, ref arr, sizes, GameObject.FindObjectOfType<MAZE>());
    }

    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(ref List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static List<POINT> FindPathBFS(POINT start, POINT end, ref bool[,] visited, ref int[,] arr, POINT sizes, MAZE mz = null)
    {
        List<POINT> queue = new List<POINT>();
        List<POINT> nextQueue = new List<POINT>();

        start.path = new List<POINT>();
        start.path.Add(new POINT(start.x, start.y));

        queue.Add(start);

        visited[start.x, start.y] = true;
        int level = 0;

        while (queue.Count > 0)
        {
            var curr = queue[0];
            if (curr.x == end.x && curr.y == end.y) return curr.path;

            arr[curr.x, curr.y] = level;
            CheckNext(1, 0, curr, ref nextQueue, ref visited, ref arr, sizes, true, mz);
            CheckNext(-1, 0, curr, ref nextQueue, ref visited, ref arr, sizes, true, mz);
            CheckNext(0, 1, curr, ref nextQueue, ref visited, ref arr, sizes, true, mz);
            CheckNext(0, -1, curr, ref nextQueue, ref visited, ref arr, sizes, true, mz);
            /*
            CheckNext(1,1, curr, ref nextQueue, ref visited, ref arr, sizes);
            CheckNext(-1,1, curr, ref nextQueue, ref visited, ref arr, sizes);
            CheckNext(-1,1, curr, ref nextQueue, ref visited, ref arr, sizes);
            CheckNext(1,-1, curr, ref nextQueue, ref visited, ref arr, sizes);*/

            queue.RemoveAt(0);

            // Next Level
            if (queue.Count == 0)
            {
                level++;
                queue = new List<POINT>(nextQueue);
                nextQueue.Clear();
            }
        }

        return null;
    }

    static void CheckNext(int dX, int dY, POINT point, ref List<POINT> q, ref bool[,] visited, ref int[,] arr,
        POINT sizes, bool checkInkr = false, MAZE mz = null)
    {
        var p = new POINT();
        p.x = point.x;
        p.y = point.y;
        p.path = new List<POINT>(point.path);

        if (p.x + dX < sizes.x && p.x + dX >= 0 && p.y + dY < sizes.y && p.y + dY >= 0
            && visited[p.x + dX, p.y + dY] == false)
            if ((!checkInkr && arr[p.x + dX, p.y + dY] != -1)
                || (checkInkr && arr[p.x + dX, p.y + dY] > 0 && mz.Maze[p.x + dX, p.y + dY].character == null))
            {
                p.x += dX;
                p.y += dY;
                p.path.Add(new POINT(p.x, p.y));
                q.Add(p);

                visited[p.x, p.y] = true;
            }
    }
}