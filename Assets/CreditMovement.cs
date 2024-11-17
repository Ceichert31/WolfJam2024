using UnityEngine;
using UnityEngine.UI;

public class CreditMovement : MonoBehaviour
{
    public RawImage RawImage;
    public float speed = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Rect uvRect = RawImage.uvRect;
        uvRect.y += speed * Time.deltaTime;
        RawImage.uvRect = uvRect;
    }
}
