using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private Transform _holder;

    [SerializeField]
    private TextMeshProUGUI _timeText;

    [SerializeField]
    private TextMeshProUGUI _killsText;

    [SerializeField]
    private TextMeshProUGUI _connectionsText;

    [SerializeField]
    private TextMeshProUGUI _pressAnyKeyText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnGameStateChanged += CheckForGameover;
    }

    private void CheckForGameover(GameManager.EGameState gameState)
    {
        if (gameState != GameManager.EGameState.Lose) return;

        StartCoroutine(DisplayGameover());
    }

    private IEnumerator DisplayGameover()
    {
        // immediately cache everything
        float time = GameManager.Instance.TimePlaying;
        int kills = GameManager.Instance.Kills;
        int connections = GameManager.Instance.Connections;

        _timeText.enabled = false;
        _killsText.enabled = false;
        _connectionsText.enabled = false;
        _pressAnyKeyText.enabled = false;

        _timeText.text = "";
        _killsText.text = "";
        _connectionsText.text = "";

        yield return new WaitForSecondsRealtime(4.0f);


        _holder.gameObject.SetActive(true);
        // wait to display everything
        yield return new WaitForSecondsRealtime(0.5f);
        _timeText.enabled = true;
        _timeText.text = "TIME: " + time.ToString("F2");

        yield return new WaitForSecondsRealtime(0.5f);
        _killsText.enabled = true;
        _killsText.text = "KILLS: " + kills.ToString();

        yield return new WaitForSecondsRealtime(0.5f);
        _connectionsText.enabled = true;
        _connectionsText.text = "CONNECTIONS: " + connections.ToString();

        yield return new WaitForSecondsRealtime(0.5f);
        _pressAnyKeyText.enabled = true;
    }
}
