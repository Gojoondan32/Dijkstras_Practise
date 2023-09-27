using System;
public static class IntExtensions 
{
    public static string ToInfinity(this int value){
        return value == int.MaxValue ? "∞" : value.ToString();
    }
}
