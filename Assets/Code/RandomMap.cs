using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMap
{
    //map array
    public int[,] dungeonMap;
    //room array
    public List<Vector4> roomArray;

    private int xSize;
    private int ySize;
    private int minRoomSize;
    private int maxRoomSize;
    private int roomAttenpt;
    //four direction around
    private int[] xt;
    private int[] yt;
    //eight direction around
    private int[] xd;
    private int[] yd;
    //vis array
    private bool[,] vis;
    private List<Vector2> visitedPredoorList;
    private enum items
    {
        soil,
        outWall,
        wall,
        visitedPredoor,
        predoor,
        door,
        floor,
        corridor,
    };

    public RandomMap(int x, int y)
    {
        xSize = x;
        ySize = y;
        minRoomSize = 4;
        maxRoomSize = 9;
        roomAttenpt = 50;
        xt = new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };
        yt = new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };
        xd = new int[] { 0, 1, 0, -1 };
        yd = new int[] { 1, 0, -1, 0 };
        visitedPredoorList = new List<Vector2>();
        dungeonMap = new int[xSize, ySize];
        vis = new bool[xSize, ySize];
        roomArray = new List<Vector4>();

        createMap();
    }
    public void createMap()
    {
        Array.Clear(dungeonMap, 0, dungeonMap.Length);
        Array.Clear(vis, 0, vis.Length);

        //set outwall
        for (int i = 0; i < xSize; ++i)
            dungeonMap[i, 0] = dungeonMap[i, ySize - 1] = (int)items.outWall;

        for (int i = 0; i < ySize; ++i)
            dungeonMap[0, i] = dungeonMap[xSize - 1, i] = (int)items.outWall;

        createRoom();
        createPath();
        setDoor();
        removeEnd();
        addWall();
    }

    private void createRoom()
    {
        for (int k = 0; k < roomAttenpt; ++k)
        {

            int x = UnityEngine.Random.Range(minRoomSize, maxRoomSize);
            int y = UnityEngine.Random.Range(minRoomSize, maxRoomSize);

            int xAxis = UnityEngine.Random.Range(1, xSize);
            int yAxis = UnityEngine.Random.Range(1, ySize);

            bool flag = false;
            for (int i = 0; i < x; ++i)
            {

                if (flag)
                    break;
                for (int j = 0; j < y; ++j)
                {

                    int xi = xAxis + i;
                    int yi = yAxis + j;

                    if (xi < 0 || xi >= xSize ||
                        yi < 0 || yi >= ySize ||
                        dungeonMap[xi, yi] != (int)items.soil)
                    {

                        flag = true;
                        break;
                    }
                }
            }

            if (!flag)
            {
                roomArray.Add(new Vector4(xAxis, yAxis, x, y));

                for (int i = 0; i < x; ++i)
                {
                    for (int j = 0; j < y; ++j)
                    {
                        dungeonMap[xAxis + i, yAxis + j] = (int)items.floor;
                    }
                }
            }

        }
    }

    private void createPath()
    {
        Stack<Vector2> stk = new Stack<Vector2>();
        int x;
        int y;
        int direction;

        for (int i = 1; i < xSize - 1; ++i)
        {
            for (int j = 1; j < ySize - 1; ++j)
            {

                if (canBeCorridor(i, j))
                {

                    stk.Push(new Vector2(i, j));

                    while (stk.Count > 0)
                    {

                        x = (int)stk.Peek().x;
                        y = (int)stk.Peek().y;

                        dungeonMap[x, y] = (int)items.corridor;

                        if ((direction = nextCorridor(x, y)) == -1)
                        {
                            stk.Pop();
                        }
                        else
                        {
                            stk.Push(new Vector2(x + xd[direction], y + yd[direction]));
                        }
                    }
                }
            }
        }
    }

    private bool canBeCorridor(int x, int y)
    {
        if (x < 1 || x >= xSize - 1 || y < 1 || y >= ySize - 1)
        {
            return false;
        }

        if (dungeonMap[x, y] != (int)items.soil)
        {
            return false;
        }

        for (int k = 0; k < 8; ++k)
            if (dungeonMap[x + xt[k], y + yt[k]] == (int)items.floor ||
                dungeonMap[x + xt[k], y + yt[k]] == (int)items.corridor)
            {
                return false;
            }

        return true;
    }

    private int nextCorridor(int x, int y)
    {
        int direction;

        direction = UnityEngine.Random.Range(0, 4);

        for (int i = 0; i < 4; ++i)
        {
            direction = (direction + 1) % 4;
            int xi = x + xd[direction];
            int yi = y + yd[direction];

            bool flag = false;

            if (xi < 1 || xi >= xSize - 1 || yi < 1 || yi >= ySize - 1)
            {
                continue;
            }

            if (dungeonMap[xi, yi] != (int)items.soil)
            {
                continue;
            }

            for (int j = 6; j <= 10; ++j)
            {
                if (dungeonMap[xi + xt[(direction * 2 + j) % 8], yi + yt[(direction * 2 + j) % 8]] != (int)items.soil &&
                    dungeonMap[xi + xt[(direction * 2 + j) % 8], yi + yt[(direction * 2 + j) % 8]] != (int)items.outWall)
                {
                    flag = true;
                }
            }

            if (flag == false)
            {
                return direction;
            }
        }

        return -1;
    }

    private void setDoor()
    {
        //find position that can be dug
        for (int i = 1; i < xSize - 1; ++i)
        {
            for (int j = 1; j < ySize - 1; ++j)
            {
                if (dungeonMap[i, j] == (int)items.soil &&
                        (((dungeonMap[i - 1, j] == (int)items.soil || dungeonMap[i - 1, j] == (int)items.predoor) &&
                        (dungeonMap[i + 1, j] == (int)items.soil || dungeonMap[i + 1, j] == (int)items.predoor) &&
                        (dungeonMap[i, j - 1] == (int)items.floor || dungeonMap[i, j - 1] == (int)items.corridor) &&
                        (dungeonMap[i, j + 1] == (int)items.floor || dungeonMap[i, j + 1] == (int)items.corridor) &&
                        !(dungeonMap[i, j - 1] == (int)items.corridor && dungeonMap[i, j + 1] == (int)items.corridor)) ||
                            ((dungeonMap[i, j - 1] == (int)items.soil || dungeonMap[i, j - 1] == (int)items.predoor) &&
                            (dungeonMap[i, j + 1] == (int)items.soil || dungeonMap[i, j + 1] == (int)items.predoor) &&
                            (dungeonMap[i - 1, j] == (int)items.floor || dungeonMap[i - 1, j] == (int)items.corridor) &&
                            (dungeonMap[i + 1, j] == (int)items.floor || dungeonMap[i + 1, j] == (int)items.corridor) &&
                            !(dungeonMap[i - 1, j] == (int)items.corridor && dungeonMap[i + 1, j] == (int)items.corridor))))
                {

                    dungeonMap[i, j] = (int)items.predoor;
                }
            }
        }

        //////set the strat point
        int rx, ry;
        int index = UnityEngine.Random.Range(0, roomArray.Count);
        rx = (int)roomArray[index].x;
        ry = (int)roomArray[index].y;
        vis[rx, ry] = true;
        combine(rx, ry);

        //randomly select a visited predoor to be a door
        while (visitedPredoorList.Count > 0)
        {
            index = UnityEngine.Random.Range(0, visitedPredoorList.Count - 1);
            rx = (int)visitedPredoorList[index].x;
            ry = (int)visitedPredoorList[index].y;

            visitedPredoorList.RemoveAt(index);
            vis[rx, ry] = true;
            dungeonMap[rx, ry] = (int)items.door;
            combine(rx, ry);
        }
    }

    private void combine(int x, int y)
    {
        Stack<Vector2> stk = new Stack<Vector2>();

        stk.Push(new Vector2(x, y));

        while (stk.Count > 0)
        {

            x = (int)stk.Peek().x;
            y = (int)stk.Peek().y;
            stk.Pop();

            for (int i = 0; i < 4; ++i)
            {

                int xi = x + xd[i];
                int yi = y + yd[i];

                if (xi > 0 && xi < xSize - 1 && yi > 0 && yi < ySize - 1 && vis[xi, yi] == false)
                {
                    if (dungeonMap[xi, yi] > (int)items.predoor)
                    {
                        vis[xi, yi] = true;
                        stk.Push(new Vector2(xi, yi));
                    }
                    else if (dungeonMap[xi, yi] == (int)items.predoor)
                    {
                        dungeonMap[xi, yi] = (int)items.visitedPredoor;
                        visitedPredoorList.Add(new Vector2(xi, yi));
                    }
                    else if (dungeonMap[xi, yi] == (int)items.visitedPredoor)
                    {

                        /////delete the unusful predoor in a small rate
                        if (UnityEngine.Random.Range(0, 12) < 1)
                        {
                            dungeonMap[xi, yi] = (int)items.door;
                            vis[xi, yi] = true;
                            stk.Push(new Vector2(xi, yi));
                        }
                        else
                        {
                            dungeonMap[xi, yi] = (int)items.soil;
                        }

                        visitedPredoorList.Remove(new Vector2(xi, yi));
                    }
                }
            }
        }
    }
    
    //remove extra corrider
    private void removeEnd()
    {
        bool flag = false;

        while (!flag)
        {

            flag = true;

            for (int i = 1; i < xSize - 1; ++i)
            {
                for (int j = 1; j < ySize - 1; ++j)
                {
                    if (dungeonMap[i, j] != (int)items.corridor &&
                        dungeonMap[i, j] != (int)items.door)
                    {
                        continue;
                    }

                    int count = 0;
                    for (int k = 0; k < 4; ++k)
                    {
                        if (dungeonMap[i + xd[k], j + yd[k]] <= (int)items.outWall)
                        {
                            ++count;
                        }
                    }

                    if (count == 3)
                    {
                        dungeonMap[i, j] = (int)items.soil;
                        flag = false;
                    }
                }
            }
        }
    }

    private void addWall()
    {
        for (int i = 0; i < xSize; ++i)
        {
            for (int j = 0; j < ySize; ++j)
            {

                if (dungeonMap[i, j] != (int)items.soil &&
                    dungeonMap[i, j] != (int)items.outWall)
                    continue;
                bool flag = false;
                for (int k = 0; k < 4; ++k)
                {
                    if (i + xd[k] < 0 || i + xd[k] >= xSize || j + yd[k] < 0 || j + yd[k] >= ySize)
                        continue;
                    if (dungeonMap[i + xd[k], j + yd[k]] == (int)items.floor ||
                        dungeonMap[i + xd[k], j + yd[k]] == (int)items.corridor)
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag)
                    dungeonMap[i, j] = (int)items.wall;
            }
        }

    }

}
