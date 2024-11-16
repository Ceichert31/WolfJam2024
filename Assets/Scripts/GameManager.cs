using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameManager instance;

    public Transform Player;

    // 
    public enum EGameState
    {
        Playing,
        Building,
        Lose
    }

    private EGameState gameState;

    private int score;

    // Getters
    public int Score { get { return score; } }
    public EGameState GameState { get { return gameState; } }

    // Helper functions
    public delegate void GameStateChanged(EGameState state);
    public GameStateChanged OnGameStateChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
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
