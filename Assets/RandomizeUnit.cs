using UnityEngine;

public class RandomizeUnit : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController[] characters;
    [SerializeField] Animator animator;


    public void RandomizeCharacter()
    {
        animator.runtimeAnimatorController = characters[Random.Range(0, characters.Length)];

    }
}
