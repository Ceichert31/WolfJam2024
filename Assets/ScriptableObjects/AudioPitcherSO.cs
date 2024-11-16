using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Pitcher")]
public class AudioPitcherSO : ScriptableObject
{
    [Header("Audio Settings")]
    [SerializeField] private List<AudioClip> audioClipList;
    public RangedFloat volume;
    public RangedFloat pitch;

    public void Play(AudioSource source)
    {
        if (audioClipList.Count <= 0 || source == null) return;

        AudioClip currentClip = audioClipList[Random.Range(0, audioClipList.Count)];

        source.volume = Random.Range(volume.Min, volume.Max);

        source.pitch = Random.Range(pitch.Min, pitch.Max);

        source.PlayOneShot(currentClip);
    }
}

[System.Serializable]
public struct RangedFloat
{
    public float Min;
    public float Max;
}