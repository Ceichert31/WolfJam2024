using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] EnemySpawner spawner;
    [SerializeField] TextMeshProUGUI textTimer;

    void Update()
    {
        textTimer.text = GameManager.Instance.TimePlaying.ToString("F2");
    }
}
