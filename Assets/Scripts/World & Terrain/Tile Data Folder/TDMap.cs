using UnityEngine;
public class TDMap 
{
   int width;
   int height;

   int numRivers;
   int numLakes;

   public TDTile[,] mapData;

    public TDMap(int mapWidth, int mapHeight, int rivers, int lakes)
    {
        width = mapWidth;
        height = mapHeight;
        numRivers = rivers;
        numLakes = lakes;

        mapData = new TDTile[width,height];
        doGrass();
        if (numRivers > 0)
        {
            for (int i = 0; i < numRivers; i++)
            {
                doRiver();
            }
        }
        if(numLakes > 0)
        {
            for (int i = 0; i < numLakes; i++)
            {
                doLake();
            }
        }
        
        
       
    }

    public int GetTileAt(int x, int y)
    {
        int returnType = mapData[x, y].GetIntType();
        return returnType;
    }
    void doGrass()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                mapData[x, y] = new TDTile(TDTypes.TYPE.GRASS);
            }
        }
    }
    void doLake()
    {
        int randX = Random.Range(5, 120);
        int randY = Random.Range(5, 120);

        mapData[randX, randY] = new TDTile(TDTypes.TYPE.OCEAN);
        mapData[randX + 1, randY] = new TDTile(TDTypes.TYPE.OCEAN);
        for (int i = 0; i < 3000; i++)
        {
            int randRange = Random.Range(0, 4);
            switch (randRange)
            {
                case 0:
                    randX++;
                    if (randX >= width-2)
                        randX = width - 3;
                    break;

                case 1:
                    randX--;
                    if (randX <= 0)
                        randX = 1;
                    break;

                case 2:
                    randY++;
                    if (randY >= height-2)
                        randY = height-3;
                    break;

                case 3:
                    randY--;
                    if (randY <= 0)
                        randY = 1;
                    break;
            }
            mapData[randX, randY] = new TDTile(TDTypes.TYPE.OCEAN);
            mapData[randX + 1, randY] = new TDTile(TDTypes.TYPE.OCEAN);
            mapData[randX, randY + 1] = new TDTile(TDTypes.TYPE.OCEAN);
        }
    }
    void doRiver()
    {
        int startXMin = 5;
        int startYMin = 5;
        int startXMax = 120;
        int startYMax = 120;
        int ignore = Random.Range(0, 4);
        switch (ignore)
        {
            case 0:
                startXMin = 80;
                break;

            case 1:
                startXMax = 50;
                break;

            case 2:
                startYMin = 80;
                break;

            case 3:
                startYMax = 50;
                break;
        }
        int randX = Random.Range(startXMin, startXMax);
        int randY = Random.Range(startYMin, startYMax);
        Debug.Log(randX);
        Debug.Log(randY);
        bool[] tested = new bool[4] { false, false, false, false };
        mapData[randX, randY] = new TDTile(TDTypes.TYPE.OCEAN);
        mapData[randX + 1, randY] = new TDTile(TDTypes.TYPE.OCEAN);

        for (int i = 0; i < 500; i++)
        {
            if (i >= 250)
                ignore = 5;
            int randRange;
            do
            {
                randRange = Random.Range(0, 4);
            } while (randRange == ignore);
            //Debug.Log(randRange);
            if (tested[0] && tested[1] && tested[2] && tested[3])
            {
                Debug.Log(randX + " Before");
                tested = new bool[4] { false, false, false, false };
                switch (randRange)
                {
                    case 0:
                        randX++;
                        if (randX >= width - 2)
                            randX = width - 4;
                        break;

                    case 1:
                        randX--;
                        if (randX <= 0)
                            randX = 2;
                        break;

                    case 2:
                        randY++;
                        if (randY >= height - 2)
                            randY = height - 4;
                        break;

                    case 3:
                        randY--;
                        if (randY <= 0)
                            randY = 2;
                        break;
                }
                Debug.Log("TRIGGERED!");
                Debug.Log(randX + " After");
                continue;
            }
            int testX = randX;
            int testY = randY;

            switch (randRange)
            {
                case 0:
                    testX++;
                    if (!tested[0])
                    {
                        tested[0] = true;
                        break;
                    }
                    else
                    {
                        testX--;
                        goto case 1;
                    }

                case 1:
                    testX--;
                    if (!tested[1])
                    {
                        tested[1] = true;
                        break;
                    }
                    else
                    {
                        testX++;
                        goto case 2;
                    }

                case 2:
                    testY++;
                    if (!tested[2])
                    {
                        tested[2] = true;
                        break;
                    }
                    else
                    {
                        testY--;
                        goto case 3;
                    }

                case 3:
                    testY--;
                    if (!tested[3])
                    {
                        tested[3] = true;
                        break;
                    }
                    else
                    {
                        testY++;
                        goto case 0;
                    }
            }
            if (testX >= width - 2)
                testX = width - 2;
            if (testY >= height - 2)
                testY = height - 2;
            if (testX <= 0)
                testX = 0;
            if (testY <= 0)
                testY = 0;


            // Debug.Log("Found it!");
            randX = testX;
            randY = testY;
            //Debug.Log(randX);
            //Debug.Log(randY);
            tested = new bool[4] { false, false, false, false };
            //tested[randRange] = true;
            mapData[randX, randY] = new TDTile(TDTypes.TYPE.OCEAN);
            mapData[randX + 1, randY] = new TDTile(TDTypes.TYPE.OCEAN);
            mapData[randX, randY + 1] = new TDTile(TDTypes.TYPE.OCEAN);

        }
    }
}
