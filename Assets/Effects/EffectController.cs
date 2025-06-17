using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] private float lifeTime;

    private void Awake()
    {
        StartCoroutine(Destroy());
    }

    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
