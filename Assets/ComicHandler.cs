using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ComicHandler : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _panel1;

    [SerializeField]
    private SpriteRenderer _panel2;

    [SerializeField]
    private SpriteRenderer _panel3;

    [SerializeField]
    private AudioClip _panel1Sound;

    [SerializeField]
    private AudioClip _panel2Sound;

    [SerializeField]
    private AudioClip _panel3Sound;

    [SerializeField]
    private AudioSource _source;


    void Start()
    {
        StartCoroutine(PlayComic());
    }

    private IEnumerator PlayComic()
    {
        // SHOW PANEL 1
        yield return new WaitForSeconds(1.0f);

        _source.PlayOneShot(_panel1Sound);

        _panel1.color = new Color(_panel1.color.r, _panel1.color.g, _panel1.color.b, 0.0f);
        _panel1.gameObject.SetActive(true);
        _panel1.DOFade(1.0f, 0.5f);

        _panel1.transform.position = new Vector3(_panel1.transform.position.x - 3.0f, _panel1.transform.position.y);
        _panel1.transform.DOMoveX(_panel1.transform.position.x + 3.0f, 0.5f);

        // SHOW PANEL 2
        yield return new WaitForSeconds(3.0f);
        _source.PlayOneShot(_panel2Sound);

        _panel2.gameObject.SetActive(true);
        _panel2.transform.localScale = new Vector3(1.4f, 1.4f, 1.0f);
        yield return _panel2.transform.DOScale(1.0f, 0.2f).SetEase(Ease.Linear).WaitForCompletion();
        _panel2.transform.DOShakePosition(0.4f, 0.2f, 50);

        // SHOW PANEL 3
        yield return new WaitForSeconds(3.0f);

        _panel3.gameObject.SetActive(true);
        _panel3.transform.DOShakePosition(0.4f, 0.6f, 50);
        _source.PlayOneShot(_panel3Sound);

        yield return null;
    }
}
