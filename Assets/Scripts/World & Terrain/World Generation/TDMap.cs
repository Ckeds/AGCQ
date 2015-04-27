using System;
using System.Threading;
using UnityEngine;
public class TDMap 
{
    int width;
    int height;

    int numRivers;
    int numLakes;
	int numDirt;
    int numDeserts;
    int desertSize;
    int desertDensity;
    int numStones;
    int stoneSize;
    int stoneDensity;
    int forestDensity;
    int forestThickness;


	System.Random rand = new System.Random();
   	public byte[,] mapData;

	//Thread[] threads;
	//int currentThread = 0;

    public TDMap(int mapWidth, int mapHeight, int rivers, int lakes, int dirt, int numDes, int desert, int desdense, int numStone, int stone, int stonedense,int forDense, int forThick)
    {
		//threads = new Thread[3];
        width = mapWidth - 1;
        height = mapHeight - 1;
        numRivers = rivers;
        numLakes = lakes;
        numDirt = dirt;
        numDeserts = numDes;
        desertSize = desert;
        desertDensity = desdense;
        numStones = numStone;
        stoneSize = stone;
        stoneDensity = stonedense;
        forestDensity = forDense;
        forestThickness = forThick;
		//Debug.Log (mapWidth);

        mapData = new byte[mapWidth, mapHeight];
        doGrass();
       	for(int i = 0; i < forestDensity; i++)
      	{
			//currentThread = 0;
			/*foreach (Thread t in threads)
			{
				if(t == null)
					break;
				if(!t.IsAlive)
					break;
				currentThread++;
				Debug.Log (t);
			}
			if(currentThread == threads.Length)
			{
				foreach (Thread t in threads)
				{
					if(t.IsAlive)
						t.Join();
				}
				currentThread = 0;
			}
			threads[currentThread] = new Thread(doForest);
			threads[currentThread].Start();
			Debug.Log(currentThread);
			while(!threads[currentThread].IsAlive);*/
			doForest();
       	}
		//Debug.Log ("Done");

        for (int i = 0; i < numDirt; i++)
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
		/*foreach (Thread t in threads)
		{
			t.Join();
		}*/
	}
	
	public int GetTileAt(int x, int y)
    {
		//Debug.Log (x + ", " + y);
		return mapData[x, y];
    }
    void doGrass()
    {
        for (int x = 0; x < width + 1; x++)
        {
            for (int y = 0; y < height + 1; y++)
            {
                mapData[x, y] = 0;
            }
        }
    }
    void doLake()
    {
        //Debug.Log("Lake Start");
		int randX = rand.Next(0, width);
		int randY = rand.Next(0, height);
        mapData[randX, randY] = 1;
        for (int i = 0; i < 2500; i++)
        {
			int randRange = rand.Next(0, 4);
            switch (randRange)
            {
                case 0:
                    randX++;
                    if (randX >= width)
                    {
                        randX = 0;
                    }
                    break;

                case 1:
                    randX--;
                    if (randX < 0)
                        randX = width - 1;
                    break;

                case 2:
                    randY++;
                    if (randY >= height)
                        randY = 0;
                    break;

                case 3:
                    randY--;
                    if (randY < 0)
                        randY = height - 1;
                    break;
            }
            
            mapData[randX, randY] = 1;
            mapData[randX + 1, randY] = 1;
            mapData[randX, randY + 1] = 1;
            mapData[randX + 1, randY + 1] = 1;
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
		int ignore = rand.Next(0, 4);
        switch (ignore)
        {
            case 0:
                startXMin = 30;
                if (startXMin > startXMax)
                    startXMin = startXMax;
                break;

            case 1:
                startXMax = width-30;
                if (startXMin > startXMax)
                    startXMax = startXMin;
                break;

            case 2:
                startYMin = 30;
                if (startYMin > startYMax)
                    startYMin = startYMax;
                break;

            case 3:
                startYMax = height-30;
                if (startYMin > startYMax)
                    startYMax = startYMin;
                break;
        }
		int randX = rand.Next(startXMin, startXMax);
		int randY = rand.Next(startYMin, startYMax);
        mapData[randX, randY] = 1;

        for (int i = 0; i < 1000; i++)
        {
            if (i >= 635)
                ignore = 5;
            int randRange;
            do
            {
				randRange = rand.Next(0, 4);
            } while (randRange == ignore);
            //Debug.Log(randRange);
           

            switch (randRange)
            {
                case 0:
                    randX++;
                    if (randX >= width)
                    {
                        randX = 0;
                    }
                    break;

                case 1:
                    randX--;
                    if (randX < 0)
                        randX = width - 1;
                    break;

                case 2:
                    randY++;
                    if (randY >= height)
                        randY = 0;
                    break;

                case 3:
                    randY--;
                    if (randY < 0)
                        randY = height - 1;
                    break;
            }
            mapData[randX, randY] = 1;
            mapData[randX + 1, randY] = 1;
            mapData[randX, randY + 1] = 1;

        }
        //Debug.Log("River End");
    }
    void doDesert()
    {
        //Debug.Log("Desert Start");
		int randX = rand.Next(0,width);
		int randY = rand.Next(0,height);
        int maxDist = desertSize;
        int startPointX = randX;
        int startPointY = randY;
		//Debug.Log (startPointX + ", " + startPointY);
        mapData[randX, randY] = 3;

        for (int i = 0; i < desertDensity; i++)
        {
			int randRange = rand.Next(0, 4);
            switch (randRange)
            {
                case 0:
                randX++;
				if (randX == startPointX + maxDist || randX == startPointX + maxDist - width)
				{
					//Debug.Log(randX >= startPointX + maxDist);
					randX -= rand.Next(maxDist - 10, maxDist);
					if(randX < 0)
					{
						randX += width;
					}
					//Debug.Log(randX + ", " + randY);
                }
                if (randX >= width)
                randX = 0;
                break;

                case 1:
                randX--;
                if (randX == startPointX - maxDist || randX == startPointX - maxDist + width)
                {
					//Debug.Log(randX <= startPointX - maxDist);
                    randX += rand.Next(maxDist - 10, maxDist);
					if(randX >= width)
					{
						randX -= width;
                   	}
					//Debug.Log(randX + ", " + randY);
                }
                if (randX < 0)
                    randX = width - 1;
                break;

                case 2:
                randY++;
                if (randY == startPointY + maxDist || randY == startPointY + maxDist - height)
                {
					//Debug.Log(randX + ", " + randY + " before");
                    randY -= rand.Next(maxDist - 10, maxDist);
					if(randY < 0)
					{
						randY += height;
                   	}
					//Debug.Log(randX + ", " + randY + " after");
                }
                if (randY >= height)
                    randY = 0;
                break;

                case 3:
                randY--;
				if (randY == startPointY - maxDist || randY == startPointY - maxDist + height)
                {
					//Debug.Log(randY <= startPointY - maxDist);
                    randY += rand.Next(maxDist - 10, maxDist);
					if(randY >= height)
					{
						randY -= height;
                   	}
					//Debug.Log(randX + ", " + randY);
				}
                if (randY < 0)
                    randY = height - 1;
                break;
			}
            mapData[randX, randY] = 3;
            mapData[randX + 1, randY] = 3;
            mapData[randX, randY + 1] = 3;
            mapData[randX + 1, randY + 1] = 3;
        }
        //Debug.Log("Desert End");

    }
    void doStone()
    {
        //Debug.Log("Stone Start");
		int randX = rand.Next(0, width);
		int randY = rand.Next(0, height);
        int maxDist = stoneSize;
        int startPointX = randX;
        int startPointY = randY;
        mapData[randX, randY] = 5;

        for (int i = 0; i < stoneDensity; i++)
        {
			int randRange = rand.Next(0, 4);
			switch (randRange)
			{
			case 0:
				randX++;
				if (randX == startPointX + maxDist || randX == startPointX + maxDist - width)
				{
					//Debug.Log(randX >= startPointX + maxDist);
					randX -= rand.Next(maxDist - 10, maxDist);
					if(randX < 0)
					{
						randX += width;
					}
					//Debug.Log(randX + ", " + randY);
				}
				if (randX >= width)
					randX = 0;
				break;
				
			case 1:
				randX--;
				if (randX == startPointX - maxDist || randX == startPointX - maxDist + width)
				{
					//Debug.Log(randX <= startPointX - maxDist);
					randX += rand.Next(maxDist - 10, maxDist);
					if(randX >= width)
					{
						randX -= width;
					}
					//Debug.Log(randX + ", " + randY);
				}
				if (randX < 0)
					randX = width - 1;
				break;
				
			case 2:
				randY++;
				if (randY == startPointY + maxDist || randY == startPointY + maxDist - height)
				{
					//Debug.Log(randX + ", " + randY + " before");
					randY -= rand.Next(maxDist - 10, maxDist);
					if(randY < 0)
					{
						randY += height;
					}
					//Debug.Log(randX + ", " + randY + " after");
				}
				if (randY >= height)
					randY = 0;
				break;
				
			case 3:
				randY--;
				if (randY == startPointY - maxDist || randY == startPointY - maxDist + height)
				{
					//Debug.Log(randY <= startPointY - maxDist);
					randY += rand.Next(maxDist - 10, maxDist);
					if(randY >= height)
					{
						randY -= height;
					}
					//Debug.Log(randX + ", " + randY);
				}
				if (randY < 0)
					randY = height - 1;
				break;
			}
            mapData[randX, randY] = 5;
            mapData[randX + 1, randY] = 5;
            mapData[randX, randY + 1] = 5;
            mapData[randX + 1, randY + 1] = 5;
        }
        //Debug.Log("Stone End");
    }
    void doForest()
    {
        //Debug.Log("Forest Start");
		int randX = rand.Next(0, width);
		int randY = rand.Next(0, height);

        mapData[randX, randY] = 2;
        for (int i = 0; i < forestThickness; i++)
        {
			int randRange = rand.Next(0, 4);
            switch (randRange)
            {
                case 0:
                    randX++;
                    if (randX >= width)
                    {
                        randX = 0;
                    }
                    break;

                case 1:
                    randX--;
                    if (randX < 0)
                        randX = width - 1;
                    break;

                case 2:
                    randY++;
                    if (randY >= height)
                        randY = 0;
                    break;

                case 3:
                    randY--;
                    if (randY < 0)
                        randY = height - 1;
                    break;
            }

            mapData[randX, randY] = 2;
            mapData[randX + 1, randY] = 2;
            mapData[randX, randY + 1] = 2;
            mapData[randX + 1, randY + 1] = 2;
        }
        //Debug.Log("Forest End");
    }
    void doDirt()
    {
        //Debug.Log("Dirt Start");
		int randX = rand.Next(5, width - 5);
		int randY = rand.Next(5, height - 5);

        mapData[randX, randY] = 4;
        for (int i = 0; i < 15; i++)
        {
			int randRange = rand.Next(0, 4);
            switch (randRange)
            {
                case 0:
                    randX++;
                    if (randX >= width)
                    {
                        randX = 0;
                    }
                    break;

                case 1:
                    randX--;
                    if (randX < 0)
                        randX = width - 1;
                    break;

                case 2:
                    randY++;
                    if (randY >= height)
                        randY = 0;
                    break;

                case 3:
                    randY--;
                    if (randY < 0)
                        randY = height - 1;
                    break;
            }

            mapData[randX, randY] = 4;
            mapData[randX + 1, randY] = 4;
            mapData[randX, randY + 1] = 4;
            mapData[randX + 1, randY + 1] = 4;
        }
        //Debug.Log("Dirt End");
    }
}
