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
        yield return new WaitForSeconds(3.0f);
        while (true && _player != null)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9, 9), 6, 0);
            Instantiate(powerUpRandomPrefab[Random.Range(0,3)], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(10,11));
        }
       
    }

}
