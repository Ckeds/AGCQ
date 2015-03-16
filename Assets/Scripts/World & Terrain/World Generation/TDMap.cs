using UnityEngine;
public class TDMap 
{
   int width;
   int height;

   int numRivers;
   int numLakes;
   int numDeserts;
   int desertSize;
   int desertDensity;
   int numStones;
   int stoneSize;
   int stoneDensity;
   int forestDensity;

   public TDTile[,] mapData;

    public TDMap(int mapWidth, int mapHeight, int rivers, int lakes, int numDes, int desert, int desdense, int numStone, int stone, int stonedense,int forDense)
    {
        width = mapWidth - 1;
        height = mapHeight - 1;
        numRivers = rivers;
        numLakes = lakes;
        numDeserts = numDes;
        desertSize = desert;
        desertDensity = desdense;
        numStones = numStone;
        stoneSize = stone;
        stoneDensity = stonedense;
        forestDensity = forDense;

        mapData = new TDTile[mapWidth, mapHeight];

        doGrass();
       for(int i = 0; i < forestDensity; i++)
       {
            doForest();
       }


        for (int i = 0; i < 75; i++)
        {
            doDirt();
        }
        for (int i = 0; i < numStones; i++)
        {
            doStone();
        }

        if (numRivers > 0)
        {
          for (int i = 0; i < numRivers; i++)
             doRiver();
        }
        if(numLakes > 0)
        {
          for (int i = 0; i < numLakes; i++)
              doLake();
        }
        for (int i = 0; i < numDeserts; i++)
        {
            doDesert();
        }
    }

    public int GetTileAt(int x, int y)
    {
		//Debug.Log (x + ", " + y);
        int returnType = mapData[x, y].GetIntType();
        return returnType;
    }
    void doGrass()
    {
        for (int x = 0; x <= width; x++)
        {
            for (int y = 0; y <= height; y++)
            {
                mapData[x, y] = new TDTile(TDTypes.TYPE.GRASS, x, y);
            }
        }
    }
    void doLake()
    {
        //Debug.Log("Lake Start");
        int randX = Random.Range(0, width);
        int randY = Random.Range(0, height);
        mapData[randX, randY] = new TDTile(TDTypes.TYPE.OCEAN, randX, randY);
        for (int i = 0; i < 3000; i++)
        {
            int randRange = Random.Range(0, 4);
            switch (randRange)
            {
                case 0:
                    randX++;
                    if (randX >= width)
                    {
                        randX = width - 2;
                    }
                    break;

                case 1:
                    randX--;
                    if (randX < 0)
                        randX = 0;
                    break;

                case 2:
                    randY++;
                    if (randY >= height)
                        randY = height - 2;
                    break;

                case 3:
                    randY--;
                    if (randY < 0)
                        randY = 0;
                    break;
            }
            
            mapData[randX, randY] = new TDTile(TDTypes.TYPE.OCEAN, randX, randY);
            mapData[randX + 1, randY] = new TDTile(TDTypes.TYPE.OCEAN, randX+1, randY);
            mapData[randX, randY + 1] = new TDTile(TDTypes.TYPE.OCEAN, randX, randY+1);
            mapData[randX + 1, randY + 1] = new TDTile(TDTypes.TYPE.OCEAN, randX+1, randY+1);
        }
        //Debug.Log("Lake End");
    }
    void doRiver()
    {
        //Debug.Log("River Start");
        int startXMin = 5;
        int startYMin = 5;
        int startXMax = width-5;
        int startYMax = height-5;
        int ignore = Random.Range(0, 4);
        switch (ignore)
        {
            case 0:
                startXMin = 30;
                break;

            case 1:
                startXMax = width-30;
                break;

            case 2:
                startYMin = 30;
                break;

            case 3:
                startYMax = height-30;
                break;
        }
        int randX = Random.Range(startXMin, startXMax);
        int randY = Random.Range(startYMin, startYMax);
        mapData[randX, randY] = new TDTile(TDTypes.TYPE.OCEAN, randX, randY);

        for (int i = 0; i < 700; i++)
        {
            if (i >= 635)
                ignore = 5;
            int randRange;
            do
            {
                randRange = Random.Range(0, 4);
            } while (randRange == ignore);
            //Debug.Log(randRange);
           

            switch (randRange)
            {
                case 0:
                    randX++;
                    if (randX >= width)
                        randX = width - 2;
                    break;

                case 1:
                    randX--;
                    if (randX < 0)
                        randX = 0;
                    break;

                case 2:
                    randY++;
                    if (randY >= height)
                        randY = height - 2;
                    break;

                case 3:
                    randY--;
                    if (randY < 0)
                        randY = 0;
                    break;
            }
            mapData[randX, randY] = new TDTile(TDTypes.TYPE.OCEAN, randX, randY);
            mapData[randX + 1, randY] = new TDTile(TDTypes.TYPE.OCEAN, randX + 1, randY);
            mapData[randX, randY + 1] = new TDTile(TDTypes.TYPE.OCEAN, randX, randY + 1);

        }
        //Debug.Log("River End");
    }
    void doDesert()
    {
        //Debug.Log("Desert Start");
        int randX = Random.Range(0,width);
        int randY = Random.Range(0,height);
        int maxDist = desertSize;
        int startPointX = randX;
        int startPointY = randY;
        mapData[randX, randY] = new TDTile(TDTypes.TYPE.DESERT, randX, randY);

        for (int i = 0; i < desertDensity; i++)
        {
            int randRange = Random.Range(0, 4);
            switch (randRange)
            {
                case 0:
                    randX++;
                    if (randX == startPointX + maxDist)
                    {
                        randX -= Random.Range(15, 25);
                    }
                    if (randX >= width)
                        randX = width-2;
                    break;

                case 1:
                    randX--;
                    if (randX == startPointX - maxDist)
                    {
                        randX += Random.Range(15, 25);
                    }
                    if (randX < 0)
                        randX = 0;
                    break;

                case 2:
                    randY++;
                    if (randY == startPointY + maxDist)
                    {
                        randY -= Random.Range(15, 25);
                    }
                    if (randY >= height)
                        randY = height-2;
                    break;

                case 3:
                    randY--;
                    if (randY == startPointY - maxDist)
                    {
                        randY += Random.Range(15, 25);
                    }
                    if (randY < 0)
                        randY = 0;
                    break;
            }
            mapData[randX, randY] = new TDTile(TDTypes.TYPE.DESERT, randX, randY);
            mapData[randX + 1, randY] = new TDTile(TDTypes.TYPE.DESERT, randX + 1, randY);
            mapData[randX, randY + 1] = new TDTile(TDTypes.TYPE.DESERT, randX, randY + 1);
            mapData[randX + 1, randY + 1] = new TDTile(TDTypes.TYPE.DESERT, randX + 1, randY + 1);
        }
        //Debug.Log("Desert End");

    }
    void doStone()
    {
        //Debug.Log("Stone Start");
        int randX = Random.Range(0, width);
        int randY = Random.Range(0, height);
        int maxDist = stoneSize;
        int startPointX = randX;
        int startPointY = randY;
        mapData[randX, randY] = new TDTile(TDTypes.TYPE.STONE,randX,randY);

        for (int i = 0; i < stoneDensity; i++)
        {
            int randRange = Random.Range(0, 4);
            switch (randRange)
            {
                case 0:
                    randX++;
                    if (randX == startPointX + maxDist)
                    {
                        randX -= Random.Range(15, 25);
                    }
                    if (randX >= width)
                        randX = width - 2;
                    break;

                case 1:
                    randX--;
                    if (randX == startPointX - maxDist)
                    {
                        randX += Random.Range(15, 25);
                    }
                    if (randX < 0)
                        randX = 0;
                    break;

                case 2:
                    randY++;
                    if (randY == startPointY + maxDist)
                    {
                        randY -= Random.Range(15, 25);
                    }
                    if (randY >= height)
                        randY = height - 2;
                    break;

                case 3:
                    randY--;
                    if (randY == startPointY - maxDist)
                    {
                        randY += Random.Range(15, 25);
                    }
                    if (randY < 0)
                        randY = 0;
                    break;
            }
            mapData[randX, randY] = new TDTile(TDTypes.TYPE.STONE, randX, randY);
            mapData[randX + 1, randY] = new TDTile(TDTypes.TYPE.STONE, randX + 1, randY);
            mapData[randX, randY + 1] = new TDTile(TDTypes.TYPE.STONE, randX, randY + 1);
            mapData[randX + 1, randY + 1] = new TDTile(TDTypes.TYPE.STONE, randX + 1, randY + 1);
        }
        //Debug.Log("Stone End");
    }
    void doForest()
    {
        //Debug.Log("Forest Start");
        int randX = Random.Range(0, width);
        int randY = Random.Range(0, height);

        mapData[randX, randY] = new TDTile(TDTypes.TYPE.FOREST,randX,randY);
        for (int i = 0; i < 20000; i++)
        {
            int randRange = Random.Range(0, 4);
            switch (randRange)
            {
                case 0:
                    randX++;
                    if (randX >= width)
                    {
                        randX = width - 2;
                    }
                    break;

                case 1:
                    randX--;
                    if (randX < 0)
                        randX = 0;
                    break;

                case 2:
                    randY++;
                    if (randY >= height)
                        randY = height - 2;
                    break;

                case 3:
                    randY--;
                    if (randY < 0)
                        randY = 0;
                    break;
            }

            mapData[randX, randY] = new TDTile(TDTypes.TYPE.FOREST, randX, randY);
            mapData[randX + 1, randY] = new TDTile(TDTypes.TYPE.FOREST, randX + 1, randY);
            mapData[randX, randY + 1] = new TDTile(TDTypes.TYPE.FOREST, randX, randY + 1);
            mapData[randX + 1, randY + 1] = new TDTile(TDTypes.TYPE.FOREST, randX + 1, randY + 1);
        }
        //Debug.Log("Forest End");
    }
    void doDirt()
    {
        //Debug.Log("Dirt Start");
        int randX = Random.Range(5, width - 5);
        int randY = Random.Range(5, height - 5);

        mapData[randX, randY] = new TDTile(TDTypes.TYPE.DIRT, randX, randY);
        for (int i = 0; i < 15; i++)
        {
            int randRange = Random.Range(0, 4);
            switch (randRange)
            {
                case 0:
                    randX++;
                    if (randX >= width)
                    {
                        randX = width - 2;
                    }
                    break;

                case 1:
                    randX--;
                    if (randX < 0)
                        randX = 0;
                    break;

                case 2:
                    randY++;
                    if (randY >= height)
                        randY = height - 2;
                    break;

                case 3:
                    randY--;
                    if (randY < 0)
                        randY = 0;
                    break;
            }

            mapData[randX, randY] = new TDTile(TDTypes.TYPE.DIRT, randX, randY);
            mapData[randX + 1, randY] = new TDTile(TDTypes.TYPE.DIRT, randX + 1, randY);
            mapData[randX, randY + 1] = new TDTile(TDTypes.TYPE.DIRT, randX, randY + 1);
            mapData[randX + 1, randY + 1] = new TDTile(TDTypes.TYPE.DIRT, randX + 1, randY + 1);
        }
        //Debug.Log("Dirt End");
    }
}
