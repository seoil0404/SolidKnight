using UnityEngine;

public static class Layer
{
    public static LayerMask Ground { get; private set; }
    public static LayerMask Player { get; private set; }
    public static LayerMask Enemy { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        Ground = LayerMask.NameToLayer("Ground");
        Player = LayerMask.NameToLayer("Player");
        Enemy = LayerMask.NameToLayer("Enemy");
    }
}
