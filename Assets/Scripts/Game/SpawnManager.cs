using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _enemyConatainer;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private float _spawnSpeed;
    [SerializeField]
    private GameObject[] powerUpRandomPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //startSpawning();
    }

    public void startSpawning()
    {
        StartCoroutine(spawnEnemyRoutine());
        StartCoroutine(spawnPowerUpTripleRoutine());
    }

    //function for spawning enemies after 3secs of destroyiny the asteriod
    IEnumerator spawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (true && _player != null)
        {
            GameObject newEnemy = Instantiate(_enemy);
            newEnemy.transform.parent = _enemyConatainer.transform;
            yield return new WaitForSeconds(_spawnSpeed);
        }
        
    }

    //function for spawning power ups
    IEnumerator spawnPowerUpTripleRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        while (true && _player != null)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9, 9), 6, 0);

            //creating a probability system to handle the random rate at which power ups are spawn.
            
            int probabilitySpawn, commonPowerUp;
            //a number is chosen from 1 to 95, if the number falls between 1 and 90 a common power up is spawned
            //the probability of it spawning is 95%, compared to the other 5% for the rare powerup.
            probabilitySpawn = Random.Range(1, 101);
            commonPowerUp = 95;
            
            if(probabilitySpawn <= commonPowerUp)
            {
                Instantiate(powerUpRandomPrefab[Random.Range(5,6)], posToSpawn, Quaternion.identity);
            }
            else
            {
                Instantiate(powerUpRandomPrefab[5], posToSpawn, Quaternion.identity);
            }
 
            yield return new WaitForSeconds(Random.Range(10,11));
        }
       
    }

}
