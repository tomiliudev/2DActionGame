using System.Collections;
using UnityEngine;

public sealed class SimpleAnime : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(DestroyWhenAnimeFinished());
    }

    IEnumerator DestroyWhenAnimeFinished()
    {
        yield return new WaitForAnimation(animator, 0);
        Destroy(gameObject);
    }
}
