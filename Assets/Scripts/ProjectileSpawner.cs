using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ProjectileSpawner : MonoBehaviour
{
    //Current Projectile prefab
    [Header("Basic Settings")]
    [SerializeField] private GameObject projectile;

    [SerializeField] private ProjectileStats projectileStats;

    [Header("PeaShooter Settings")]
    [SerializeField] private bool isAutoAimEnabled;
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyLayer;

    private Vector2 targetDirection;
    private GameObject targetEnemy;
    private Collider2D[] enemies;

    [Header("Flamethrower Settings")]
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
        if (isAutoAimEnabled)
        {
            //Find Closest Enemy in range
            enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);
            float closestEnemy = Mathf.Infinity;
            if (enemies != null)
            {
                foreach (Collider2D enemy in enemies)
                {
                    float distance = Vector2.Distance(enemy.transform.position, transform.position);
                    if (distance < closestEnemy)
                    {
                        targetEnemy = enemy.gameObject;
                        closestEnemy = distance;
                    }
                }
            }

            //Rotate
            if (targetEnemy != null)
            {
                targetDirection = (targetEnemy.transform.position - transform.position).normalized;
                //Debug
                Debug.DrawRay(transform.position, targetDirection * 3f, Color.yellow);

                float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1000 * Time.deltaTime);
            }
        }

        //instance = StartCoroutine(RotateTo(transform.eulerAngles, new(transform.eulerAngles.x, transform.eulerAngles.y, angle)));
        //angle = Mathf.Clamp(angle, minRotationZ, maxRotationZ);

        //Flamethrower functionallity
        /* if (!canRotate) return;
         * 
         * if (instance != null) return;

         if (!reverseDirection)
             instance = StartCoroutine(RotateTo(new(transform.eulerAngles.x, transform.eulerAngles.y, minRotationZ), new(transform.eulerAngles.x, transform.eulerAngles.y, maxRotationZ)));
         else
             instance = StartCoroutine(RotateTo(new(transform.eulerAngles.x, transform.eulerAngles.y, maxRotationZ), new(transform.eulerAngles.x, transform.eulerAngles.y, minRotationZ)));*/
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

            //Set Stats
            stats.Direction = transform.right;
            stats.EnemyLayer = Mathf.RoundToInt(Mathf.Log(enemyLayer.value, 2));

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
    public int EnemyLayer;

    public ProjectileStats(float speed, float lifeTime, Vector2 direction, int enemyLayer)
    {
        Speed = speed;
        LifeTime = lifeTime;
        Direction = direction;
        EnemyLayer = enemyLayer;
    }
}
