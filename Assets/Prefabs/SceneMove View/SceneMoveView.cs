using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SceneMoveView : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.Play("Appear");
    }

    public void Disappear()
    {
        animator.Play("Disappear");
    }
}
