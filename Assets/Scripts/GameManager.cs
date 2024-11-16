using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;

    public Transform Player;

    [SerializeField]
    private float _slowDownTime = 3.0f;

    // 
    public enum EGameState
    {
        Playing,
        Building,
        Lose
    }

    private EGameState gameState = EGameState.Playing;

    private float timePlaying;
    private int kills;
    private int connections;

    public bool debugMode = false;

    // Getters
    public float TimePlaying { get { return timePlaying; } }
    public int Kills { get { return kills; } }
    public int Connections { get { return connections; } }
    public EGameState GameState { get { return gameState; } }

    // Helper functions
    public delegate void GameStateChanged(EGameState state);
    public GameStateChanged OnGameStateChanged;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        // auto lose
        if (debugMode && Input.GetKeyDown(KeyCode.L)) UpdateGameState(EGameState.Lose);

        if (gameState == EGameState.Lose) return;

        if (gameState == EGameState.Playing)
        {
            timePlaying += Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(1))
        {
            if(gameState == EGameState.Playing)
            {
                Time.timeScale = 0.0f;
                UpdateGameState(EGameState.Building);
            }
            else if (gameState == EGameState.Building)
            {
                Time.timeScale = 1.0f;
                UpdateGameState(EGameState.Playing);
            }
        }
    }

    public void UpdateGameState(EGameState newGameState)
    {
        gameState = newGameState;

        // calls an event that other scripts can lsiten to and perform logic
        OnGameStateChanged?.Invoke(gameState);

        if (gameState == EGameState.Lose) HandleLoseState();
    }

    private void HandleLoseState()
    {
        StartCoroutine(HandleLose());
    }

    private IEnumerator HandleLose()
    {
        float elapsed = 0.0f;

        while(elapsed < _slowDownTime)
        {
            elapsed += Time.unscaledDeltaTime;

            Time.timeScale = Mathf.Lerp(1.0f, 0.0f, elapsed / _slowDownTime);
            yield return null;
        }

        Time.timeScale = 0.0f;
    }

    public void AddToKills()
    {
        kills++;
    }

    public void AddToConnections()
    {
        connections++;
    }
}
