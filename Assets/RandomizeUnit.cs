using UnityEditor.Animations;
using UnityEngine;

public class RandomizeUnit : MonoBehaviour
{
    [SerializeField] AnimatorController[] characters;
    [SerializeField] Animator animator;
    private void Awake()
    {
        animator.runtimeAnimatorController = characters[Random.Range(0, characters.Length)];
    }
}
