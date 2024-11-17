using UnityEngine;

public class RandomizeUnit : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController[] characters;
    [SerializeField] Animator animator;
    private void Awake()
    {
        animator.runtimeAnimatorController = characters[Random.Range(0, characters.Length)];
    }
}
