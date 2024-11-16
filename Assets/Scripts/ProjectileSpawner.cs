using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    //Current Projectile prefab
    [SerializeField] private GameObject projectile;

    [SerializeField] private ProjectileStats projectileStats;

    [SerializeField] private bool canRotate;
    [SerializeField] private float minRotationZ;
    [SerializeField] private float maxRotationZ;

    [SerializeField] private float rotationTime;
    private float currentRotationTime;

    private bool reverseDirection;

    private Coroutine instance;
    void Start()
    {
        SpawnProjectiles(100, 0.3f, projectileStats);
    }
    
    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 3f, Color.yellow);

        //Flamethrower functionallity
        if (!canRotate) return;

        if (instance != null) return;

        if (!reverseDirection)
            instance = StartCoroutine(RotateTo(new(transform.eulerAngles.x, transform.eulerAngles.y, minRotationZ), new(transform.eulerAngles.x, transform.eulerAngles.y, maxRotationZ)));
        else
            instance = StartCoroutine(RotateTo(new(transform.eulerAngles.x, transform.eulerAngles.y, maxRotationZ), new(transform.eulerAngles.x, transform.eulerAngles.y, minRotationZ)));
    }

    IEnumerator RotateTo(Vector3 startEuler, Vector3 targetEuler)
    {
        float timeElapsed = 0;
        transform.eulerAngles = startEuler;
        while (timeElapsed < rotationTime)
        {
            timeElapsed += Time.deltaTime;
            transform.eulerAngles = Vector3.Lerp(startEuler, targetEuler, timeElapsed / rotationTime);
            yield return null;
        }
        transform.eulerAngles = targetEuler;
        instance = null;
        reverseDirection = !reverseDirection;
    }

    /// <summary>
    /// Spawns a certain number of projectiles at certain intervals
    /// </summary>
    /// <param name="projectileNum"></param>
    /// <param name="delayBetween"></param>
    /// <param name="projectileSpeed"></param>
    void SpawnProjectiles(int projectileNum, float delayBetween, ProjectileStats stats)
    {
        StartCoroutine(SpawnPattern(projectileNum, delayBetween, stats));
    }

    IEnumerator SpawnPattern(int projectileNum, float delayBetween, ProjectileStats stats)
    {
        WaitForSeconds waitTime = new WaitForSeconds(delayBetween);

        for (int i = 0; i < projectileNum; i++)
        {
            //Create projectile instance
            Projectile instance = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
            stats.Direction = -transform.right;

            //Set Projectile speed
            instance.SetStats(stats);
            yield return waitTime;
        }
        yield return null;
    }
}

[System.Serializable]
public struct ProjectileStats
{
    public float Speed;
    public float LifeTime;
    [HideInInspector] public Vector2 Direction;

    public ProjectileStats(float speed, float lifeTime, Vector2 direction)
    {
        Speed = speed;
        LifeTime = lifeTime;
        Direction = direction;
    }
}
