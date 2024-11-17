using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public GameObject tutorial1;
    public GameObject tutorial2;
    public GameObject tutorial3;
    public GameObject tutorial4;
    public GameObject tutorial5;
    public GameObject tutorial6;

    public bool firstMove;
    public bool firstDeath;
    public bool firstBuildMode;
    public bool attachFirstUnit;
    public bool placeFirstUnit;
    public bool exitBuildModeFirst;

    static public TutorialController Instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
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
}
