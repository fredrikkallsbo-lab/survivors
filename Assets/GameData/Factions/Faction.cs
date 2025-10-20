using UnityEngine;

public enum Faction
{
    Player,
    Enemy,
    
}

public static class FactionUtil {
    public static bool IsHostileTowards(Faction a, Faction b)
    {
        return a != b; 
    }
}