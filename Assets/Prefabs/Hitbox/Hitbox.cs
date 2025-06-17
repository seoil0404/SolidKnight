using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private EffectData effectData;

    private Target? target = null;
    private uint damage;

    public Hitbox Initialize(Target target, Vector2 scale, uint damage)
    {
        this.target = target;
        this.damage = damage;

        transform.localScale = scale;

        return this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(target == null)
        {
            Debug.LogError("Error: Hitbox target is not allocated");
            return;
        }

        if (target == Target.Player)
        {
            PlayerHealthManager healthManager;
            if (collision.gameObject.TryGetComponent(out healthManager))
                healthManager.ReduceHealth(damage);

            Destroy(gameObject);
            return;
        }
        else if (target == Target.Enemy)
        {
            EnemyHealthManager healthManager;
            if (collision.gameObject.TryGetComponent(out healthManager))
                healthManager.ReduceHealth(damage);
            else return;
        }
        else return;

        Instantiate(effectData.Hit).transform.position = transform.position;
        Destroy(gameObject);
    }

    public enum Target
    {
        Player, Enemy
    }
}
