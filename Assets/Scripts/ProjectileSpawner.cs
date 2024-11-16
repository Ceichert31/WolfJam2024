using System.Collections;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    //Current Projectile prefab
    [SerializeField] private GameObject projectile;


    void Start()
    {
        SpawnProjectiles(3, 1f, 5f);
    }

    /// <summary>
    /// Spawns a certain number of projectiles at certain intervals
    /// </summary>
    /// <param name="projectileNum"></param>
    /// <param name="delayBetween"></param>
    /// <param name="projectileSpeed"></param>
    void SpawnProjectiles(int projectileNum, float delayBetween, float projectileSpeed)
    {
        StartCoroutine(SpawnPattern(projectileNum, delayBetween, projectileSpeed));
    }

    IEnumerator SpawnPattern(int projectileNum, float delayBetween, float projectileSpeed)
    {
        WaitForSeconds waitTime = new WaitForSeconds(delayBetween);

        for (int i = 0; i < projectileNum; i++)
        {
            //Create projectile instance
            Projectile instance = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();

            //Set Projectile speed
            instance.SetSpeed(projectileSpeed);
            yield return waitTime;
        }
        yield return null;
    }
}
