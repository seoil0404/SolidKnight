using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "Scriptable Objects/EffectData")]
public class EffectData : ScriptableObject
{
    [SerializeField] private EffectController dash;
    [SerializeField] private EffectController hit;

    public EffectController Dash => dash;
    public EffectController Hit => hit;
}
