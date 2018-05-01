using System;
using System.Collections.Generic;

public static class EnumerableExtensions
{
    public static void ForAll(this IEnumerable<Grid> locations,
        Action<Grid> act)
    {
        foreach(var location in locations)
        {
            act(location);
        }
    }
}