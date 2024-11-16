using System.Collections;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    //Current Projectile prefab
    [SerializeField] private GameObject projectile;


    void Start()
    {
        SpawnProjectiles(3, 1);
    }

    void SpawnProjectiles(int projectileNumber, float delayBetweenProjectiles)
    {
        StartCoroutine(SpawnPattern(projectileNumber, delayBetweenProjectiles));
    }

    IEnumerator SpawnPattern(int projectileNumber, float delayBetweenProjectiles)
    {
        WaitForSeconds waitTime = new WaitForSeconds(delayBetweenProjectiles);

        for (int i = 0; i < projectileNumber; i++)
        {
            //Create projectile instance
            GameObject instance = Instantiate(projectile, transform.position, Quaternion.identity);
            yield return waitTime;
        }
        yield return null;
    }
}
