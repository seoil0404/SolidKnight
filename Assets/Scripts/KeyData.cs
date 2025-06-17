using UnityEngine;

public static class KeyData
{
    public static KeyCode Left { get; private set; }
    public static KeyCode Right { get; private set; }
    public static KeyCode Jump { get; private set; }
    public static KeyCode Attack { get; private set; }
    public static KeyCode Skill { get; private set; }
    public static KeyCode Dash { get; private set; }
    public static KeyCode Parring { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        Left = KeyCode.LeftArrow;
        Right = KeyCode.RightArrow;
        Jump = KeyCode.Z;
        Attack = KeyCode.X;
        Skill = KeyCode.A;
        Dash = KeyCode.C;
        Parring = KeyCode.LeftShift;
    }
}
