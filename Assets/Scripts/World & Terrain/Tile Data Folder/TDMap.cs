
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
                mapData[x, y] = new TDTile(TDTypes.TYPE.GRASS);
            }
        }
        //test case
        mapData[5, 5] = new TDTile(TDTypes.TYPE.OCEAN);
        mapData[4, 4] = new TDTile(TDTypes.TYPE.DESERT);
        mapData[3, 3] = new TDTile(TDTypes.TYPE.DIRT);
    }

    public int GetTileAt(int x, int y)
    {
        int returnType = mapData[x, y].GetIntType();
        return returnType;
    }
}
