
public class TDTile
{
    private int intType;
    private TDTypes.TYPE tileType;

    public TDTile(TDTypes.TYPE type = TDTypes.TYPE.GRASS)
    {
        tileType = type;
        
        switch (type)
        {
            case TDTypes.TYPE.GRASS:
                intType = 0;
                break;

            case TDTypes.TYPE.OCEAN:
                intType = 1;
                break;

            case TDTypes.TYPE.DESERT:
                intType = 2;
                break;

            case TDTypes.TYPE.DIRT:
                intType = 3;
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
