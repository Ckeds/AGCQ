
public class TDTile
{
    private int intType;
    public int positionX;
    public int positionY;
    public TDTypes.TYPE tileType;

    public TDTile(TDTypes.TYPE type, int posX, int posY)
    {
        tileType = type;
        positionX = posX;
        positionY = posY;
        
        switch (type)
        {
            case TDTypes.TYPE.GRASS:
                intType = 0;
                break;

            case TDTypes.TYPE.OCEAN:
                intType = 1;
                break;

            case TDTypes.TYPE.FOREST:
                intType = 2;
                break;

            case TDTypes.TYPE.DESERT:
                intType = 3;
                break;

            case TDTypes.TYPE.DIRT:
                intType = 4;
                break;

            case TDTypes.TYPE.STONE:
                intType = 5;
                break;

            default:
                intType = 0;
                break;

        }
    }
    public int GetIntType()
    {
        return intType;
    }
}
