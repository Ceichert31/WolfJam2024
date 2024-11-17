using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class UnitSpriteHelper : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    private bool isPickedUp;

    private Coroutine rotationAnimation;

    public void Pickup()
    {
        Debug.Log("PICK UP ANIM");
        isPickedUp = true;

        target.DOScale(1.5f, 0.3f).SetUpdate(true).SetEase(Ease.OutQuart);
        rotationAnimation = StartCoroutine(HandleRotationAnimation());
    }

    public void PutDown()
    {
        Debug.Log("PUT DOWN ANIM");

        isPickedUp = false;

        target.DOKill(true);
        target.DOScale(1.0f, 0.3f).SetUpdate(true).SetEase(Ease.OutQuart);
        target.DOLocalRotate(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.Linear).SetUpdate(true);

        if (rotationAnimation != null) StopCoroutine(rotationAnimation);
    }

    private IEnumerator HandleRotationAnimation()
    {
        while (true)
        {
            yield return target.DOLocalRotate(new Vector3(0, 0, 7), 0.2f).SetEase(Ease.Linear).SetUpdate(true).WaitForCompletion();

            yield return target.DOLocalRotate(new Vector3(0, 0, -7), 0.2f).SetEase(Ease.Linear).SetUpdate(true).WaitForCompletion();
        }
    }
}
