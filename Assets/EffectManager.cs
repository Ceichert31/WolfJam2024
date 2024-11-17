using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    Tween shakeTween;
    public Camera mainCam;
    public Image fadeImage;
    public Image screenFlashImage;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        mainCam = Camera.main;
    }

    // CallUpdate is called once per frame
    void Update()
    {
        if (!shakeTween.IsActive())
        {
            mainCam.transform.localPosition = new Vector3(0,0,-10);
        }

    }

    public void CameraShake(float duration, float strength, int vibrato)
    {
        shakeTween = mainCam.transform.DOShakePosition(duration, strength, vibrato);
    }

    public void CallScreenFlash(float time)
    {
        StartCoroutine(ScreenFlash(time));
    }

    public IEnumerator ScreenFlash(float fadeOutTime)
    {
        screenFlashImage.color = new Color(screenFlashImage.color.r, screenFlashImage.color.g, screenFlashImage.color.b, 1.0f);

        Color initialColor = screenFlashImage.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0.0f);

        float elapsed = 0.0f;
        while(elapsed < fadeOutTime)
        {
            screenFlashImage.color = Color.Lerp(initialColor, targetColor, elapsed / fadeOutTime);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        screenFlashImage.color = targetColor;
    }

    public IEnumerator ScreenFade(Color color, float targetOpacity, float fadeInDuration, float fadeOutDuration, float solidDuration)
    {
        float elapsed = 0;

        fadeImage.color = new Color(color.r, color.g, color.b, 0);
        Color initialColor = fadeImage.color;
        Color targetColor = new Color(color.r, color.g, color.b, targetOpacity);

        while (elapsed < fadeInDuration)
        {
            fadeImage.color = Color.Lerp(initialColor, targetColor, elapsed / fadeInDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = targetColor;

        yield return new WaitForSeconds(solidDuration);

        elapsed = 0;

        while (elapsed < fadeOutDuration)
        {
            fadeImage.color = Color.Lerp(targetColor, initialColor, elapsed / fadeOutDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = initialColor;
    }
}
