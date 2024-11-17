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
        _source.PlayOneShot(_panel1Sound);

        _source.PlayOneShot(_panel2Sound);

        _source.PlayOneShot(_panel3Sound);

        yield return null;
    }
}
