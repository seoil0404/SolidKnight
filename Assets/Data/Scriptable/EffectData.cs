using UnityEngine;

[CreateAssetMenu(fileName = "EffectData", menuName = "Scriptable Objects/EffectData")]
public class EffectData : ScriptableObject
{
    [SerializeField] private EffectController dash;
    [SerializeField] private EffectController hit;
    [SerializeField] private EffectController playerHit;
    [SerializeField] private EffectController parring;
    [SerializeField] private EffectController dust;
    [SerializeField] private EffectController death;

    public EffectController Dash => dash;
    public EffectController Hit => hit;
    public EffectController PlayerHit => playerHit;
    public EffectController Parring => parring;
    public EffectController Dust => dust;
    public EffectController Death => death;
}
