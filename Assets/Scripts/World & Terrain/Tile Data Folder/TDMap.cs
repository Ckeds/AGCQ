using UnityEngine;
public class TDMap 
{
   int width;
   int height;

   TDTile[,] mapData;

    /*
     * 0 = grass
     * 1 = water
     * 2 = desert
     * 3 = dirt
     */ 

    public TDMap(int mapWidth, int mapHeight)
    {
        width = mapWidth;
        height = mapHeight;

        mapData = new TDTile[width,height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int rand = Random.Range(0, 7);
                //int rand = 0;
                switch (rand)
                {
                    case 0:
                        mapData[x, y] = new TDTile(TDTypes.TYPE.GRASS);
                        break;

                    case 1:
                        mapData[x, y] = new TDTile(TDTypes.TYPE.OCEAN);
                        break;

                    case 3:
                        mapData[x, y] = new TDTile(TDTypes.TYPE.FOREST);
                        break;

                    case 4:
                        mapData[x, y] = new TDTile(TDTypes.TYPE.DESERT);
                        break;

                    case 5:
                        mapData[x, y] = new TDTile(TDTypes.TYPE.DIRT);
                        break;

                    case 6:
                        mapData[x, y] = new TDTile(TDTypes.TYPE.STONE);
                        break;

                    default:
                        mapData[x, y] = new TDTile(TDTypes.TYPE.GRASS);
                        break;

                }
              
            }
        }
    }

    public int GetTileAt(int x, int y)
    {
        int returnType = mapData[x, y].GetIntType();
        return returnType;
    }
}
