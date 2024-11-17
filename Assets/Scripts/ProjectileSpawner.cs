using JetBrains.Annotations;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    [SerializeField] private float fireDelay = 0.5f;

    private Vector2 targetDirection;
    private GameObject targetEnemy;
    private Collider2D[] enemies;

    //Clamping
    private float localClampMin;
    private float localClampMax;
    private float angle;

    private Vector2 originalDirection;

    [Header("Flamethrower Settings")]
    [SerializeField] private bool canRotate;
    [SerializeField] private float minRotationZ;
    [SerializeField] private float maxRotationZ;
    [SerializeField] private float rotationTime;

    private float currentRotationTime;
    private bool reverseDirection;
    private Coroutine instance;
    [SerializeField] private Unit unit;

    [Header("Audio Settings")]
    [SerializeField] private AudioPitcherSO firingPitcher;

    private AudioSource source;
    private float waitTime;

    private bool canFire = true;
    void Start()
    {
        source = GetComponent<AudioSource>();

        unit = transform.parent.parent.GetComponent<Unit>();

        //Set target enemy layer based on current layer
        if (unit.gameObject.layer == 3)
            enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        if (unit.gameObject.layer == 7)
            enemyLayer = 1 << LayerMask.NameToLayer("Player");

        originalDirection = transform.right;

        //Local clamp
        localClampMin = transform.eulerAngles.z + minRotationZ;
        localClampMax = transform.eulerAngles.z + maxRotationZ;
    }
    
    private void Update()
    {
        if (unit.IsPlayerShip)
            enemyLayer = 1 << LayerMask.NameToLayer("Enemy");

        if (unit.MyShipUnitState == Unit.ShipUnitState.Detatched) 
        {
            StopAllCoroutines();
            instance = null;
            return;
        }

        //Logic for tracking and firing at nearest enemy
        if (isAutoAimEnabled)
        {
            //Find Closest Enemy in range
            enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyLayer);
            float closestEnemy = Mathf.Infinity;
            if (enemies != null)
            {
                foreach (Collider2D enemy in enemies)
                {
                    Unit instanceUnit = enemy.GetComponent<Unit>();
                    //If unit is detached and target untarget
                    if (instanceUnit.MyShipUnitState == Unit.ShipUnitState.Detatched && targetEnemy == enemy.gameObject)
                    {
                        targetEnemy = null;
                    }
                    //If unit is detachted return
                    if (instanceUnit.MyShipUnitState == Unit.ShipUnitState.Detatched) return;

                    //Find closest enemy
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

                /*float dotResult = Vector2.Dot(transform.right, targetEnemy.transform.position.normalized);

                Debug.Log(dotResult);

                if (dotResult > -0.6)
                {
                    
                }*/

                gameObject.transform.right = targetDirection;

                //Debug
                Debug.DrawRay(transform.position, targetDirection * 3f, Color.yellow);
            }

            //Spawn projectiles
            if (instance != null) return;
            if (Time.time > waitTime)
            {
                waitTime = Time.time + fireDelay;
                SpawnProjectiles(projectileStats);
            }
        }
        else
        {
            gameObject.transform.right = originalDirection;

            //Spawn projectiles
            if (instance != null) return;
            if (Time.time > waitTime)
            {
                waitTime = Time.time + fireDelay;
                SpawnProjectiles(projectileStats);
            }
        }
    }
    /// <summary>
    /// Spawns a certain number of projectiles at certain intervals
    /// </summary>
    /// <param name="projectileNum"></param>
    /// <param name="delayBetween"></param>
    /// <param name="projectileSpeed"></param>
    public void SpawnProjectiles(ProjectileStats stats)
    {
        instance = StartCoroutine(SpawnPattern(stats));
    }

    IEnumerator SpawnPattern(ProjectileStats stats)
    {
        WaitForSeconds waitTime = new WaitForSeconds(stats.DelayBetween);

        for (int i = 0; i < stats.ProjectileNumber; i++)
        {
            //Create projectile instance
            Projectile instance = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();

            firingPitcher.Play(source);

            //Set Stats
            stats.Direction = transform.right;
            stats.EnemyLayer = Mathf.RoundToInt(Mathf.Log(enemyLayer.value, 2));
            stats.ParentObject = transform.parent.parent.parent.gameObject;

            Debug.Log("Shoot");

            //Set Projectile speed
            instance.SetStats(stats);
            yield return waitTime;
        }
        yield return null;
        instance = null;
    }

    #region Defunct
    /*IEnumerator RotateTo(Vector3 startEuler, Vector3 targetEuler)
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
    }*/

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



    /*           else
            {
                *//*StopAllCoroutines();
                instance = null;
                Debug.Log("No Enemies");
                //Reset rotation
                gameObject.transform.right = originalDirection;*//*
            }*/
    #endregion
}

[System.Serializable]
public struct ProjectileStats
{
    public float Speed;
    public float LifeTime;
    [HideInInspector] public Vector2 Direction;
    public int EnemyLayer;
    public GameObject ParentObject;
    public int Damage;
    public int ProjectileNumber;
    public float DelayBetween;

    public ProjectileStats(float speed, float lifeTime, Vector2 direction, int enemyLayer, GameObject parentObject, int damage, int projectileNum, float delayBetween)
    {
        Speed = speed;
        LifeTime = lifeTime;
        Direction = direction;
        EnemyLayer = enemyLayer;
        ParentObject = parentObject;
        Damage = damage;
        ProjectileNumber = projectileNum;
        DelayBetween = delayBetween;
    }
}
