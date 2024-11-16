using UnityEngine;
using DG.Tweening;

public class MusicHandler : MonoBehaviour
{
    [SerializeField]
    private AudioClip _mainTrack;

    [SerializeField]
    private AudioClip _pauseTrack;

    [SerializeField]
    private AudioSource _source;

    // Update is called once per frame
    float currentTrackTime;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += UpdateMusicState;
    }

    private void UpdateMusicState(GameManager.EGameState gameState)
    {
        currentTrackTime = _source.time;

        if(gameState == GameManager.EGameState.Playing)
        {
            _source.clip = _mainTrack;
            _source.time = currentTrackTime;
            _source.Play();
        }

        else if(gameState == GameManager.EGameState.Building)
        {
            _source.clip = _pauseTrack;
            _source.time = currentTrackTime;
            _source.Play();
        }

        else if(gameState == GameManager.EGameState.Lose)
        {
            _source.DOPitch(0.0f, 3.0f).SetUpdate(true);
            _source.DOFade(0.0f, 3.0f).SetUpdate(true);
        }
    }
}
