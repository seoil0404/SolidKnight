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
            {
                if(healthManager.ReduceHealth(damage))
                {
                    Instantiate(effectData.PlayerHit).transform.position = transform.position;
                    healthManager.gameObject.GetComponent<PlayerRenderManager>().FadeColor(Color.red, Color.white, 0.5f);
                }
                else
                    Instantiate(effectData.Parring).transform.position = transform.position;
            }
            else return;

            
        }
        else if (target == Target.Enemy)
        {
            if (collision.gameObject.TryGetComponent(out EnemyHealthManager healthManager))
            {
                healthManager.ReduceHealth(damage);
                healthManager.gameObject.GetComponent<EnemyRenderManager>().FadeColor(Color.red, Color.white, 0.5f);
            }
            else return;

            Instantiate(effectData.Hit).transform.position = transform.position;
        }
        else return;
        Destroy(gameObject);
    }

    public enum Target
    {
        Player, Enemy
    }
}
