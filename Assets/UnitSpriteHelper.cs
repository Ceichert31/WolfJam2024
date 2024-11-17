using UnityEngine;
using DG.Tweening;

public class UnitSpriteHelper : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private bool isPickedUp;

    public void Pickup()
    {
        isPickedUp = true;

        target.DOScale(1.1f, 0.5f);
    }

    public void PutDown()
    {
        isPickedUp = false;

        target.DOKill(true);
        target.DOScale(1.0f, 0.5f);
    }
}
