using System;

public static class Extention_Random
{
    public static long Next(this Random Self,long in_Max)
    {
        byte[] bytes = new byte[8];
        Self.NextBytes(bytes);

        return Math.Abs(BitConverter.ToInt64(bytes, 0)) % in_Max;
    }

}
