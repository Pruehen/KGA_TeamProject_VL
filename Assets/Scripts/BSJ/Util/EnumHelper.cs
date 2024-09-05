using EnumTypes;

public static class EnumHelper
{
    public static int ConvertEnumFlagToInt(Phase phase)
    {
        int value = (int)phase;

        int index = 0;
        while (value > 1)
        {
            value >>= 1;
            index++;
        }
        return index;
    }
}
