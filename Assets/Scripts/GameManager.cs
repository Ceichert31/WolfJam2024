using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance;

    public Transform Player;

    // 
    public enum EGameState
    {
        Playing,
        Building,
        Lose
    }

    private EGameState gameState = EGameState.Playing;

    private int score;

    // Getters
    public int Score { get { return score; } }
    public EGameState GameState { get { return gameState; } }

    // Helper functions
    public delegate void GameStateChanged(EGameState state);
    public GameStateChanged OnGameStateChanged;

    private void Awake()
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
        //DEBUG
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(gameState == EGameState.Playing)
            {
                UpdateGameState(EGameState.Building);
            }
            else if (gameState == EGameState.Building)
            {
                UpdateGameState(EGameState.Playing);
            }
        }
    }

    public void UpdateGameState(EGameState newGameState)
    {
        gameState = newGameState;

        // calls an event that other scripts can lsiten to and perform logic
        OnGameStateChanged?.Invoke(gameState);
    }

    public void UpdateScore(int scoreCount)
    {
        score += scoreCount;
    }
}
