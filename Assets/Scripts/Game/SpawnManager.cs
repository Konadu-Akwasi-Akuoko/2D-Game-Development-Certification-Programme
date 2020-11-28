using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //Wave system
    //The total number of enemies spawned within a particular wave.
    [SerializeField]
    private int _numberOfEnemySpawned = default;

    //The wave number.
    [SerializeField]
    private int _wave = default;

    //The total number of enemies killed in a particular wave.
    [SerializeField]
    private int _enemiesKilled = default;

    //The total number of enemies to be spawned per wave.
    [SerializeField]
    private int _enemyOfWave = default;

    //The text used to display an incoming wave.
    [SerializeField]
    private Text _textOfWave = default;

    void Start()
    {
      
    }

    //startSpawning is called when the "asteriod" is destroyed LOOK(AsteriodBehaviour.cs)
    public IEnumerator StartSpawning()
    {
        //When start spawning is called, all the variables are assigned values,
        //they are asigned initiall values.
        _wave = 1;
        _enemyOfWave = 5;
        _numberOfEnemySpawned = 0;
        _enemiesKilled = 0;
        _textOfWave.gameObject.SetActive(true);
        _textOfWave.text = "Wave " + _wave;
        
        //Waits for 1.5 seconds before going on to the next line of code.
        yield return new WaitForSeconds(1.5f);

        //Start the courotines after 1.5f seconds 
        _textOfWave.gameObject.SetActive(false);
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpTripleRoutine());

        //This works as void Start(), we only use the values once, so we need to terminate the courotine.
        yield break;
    }

    //function for spawning enemies after 3secs of destroyiny the asteriod
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

            while (true && _player != null)
            {
                //Checking to see if enemies spawned is not equal to the number of enemies responsible for each wave.
                //If they are not equal continue spawning.
                if (_numberOfEnemySpawned != _enemyOfWave)
                {
                    //If the text which shows the wave we are on is set to true, then turn it of
                    if (_textOfWave.gameObject.activeSelf)
                    {   
                        _textOfWave.gameObject.SetActive(false);
                    }

                    GameObject newEnemy = Instantiate(_enemy);

                    //Always adding incerement when there is an instantiate, if the number gets equal
                    //with the enemies responsible for each wave it will stop spawning.
                    _numberOfEnemySpawned++;
                    newEnemy.transform.parent = _enemyConatainer.transform;
                }

                else
                {
                    //The number of enemies killed is incremented whenever an enemy dies from Enemy.cs.
                    //If it is the same as the particular number of enemies in each wave, initiate the next wave
                    if (_enemiesKilled == _enemyOfWave)
                    {
                        _wave++;
                        _textOfWave.text = "Wave " + _wave;
                        _textOfWave.gameObject.SetActive(true);
                        _enemyOfWave = _wave * 5;
                        _numberOfEnemySpawned = 0;
                        _enemiesKilled = 0;
                        yield return new WaitForSeconds(1.0f);
                    }
                }

                yield return new WaitForSeconds(_spawnSpeed);
            }

    }

    public void EnemyKilled()
    {
        //Incrementing the number of enemies killed, if it is the same 
        //as the specific number of enemies in a wave, the next wave will be initiated
        _enemiesKilled++;
    }

    //function for spawning power ups
    IEnumerator SpawnPowerUpTripleRoutine()
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
                Instantiate(powerUpRandomPrefab[Random.Range(1,5)], posToSpawn, Quaternion.identity);
            }
            else
            {
                Instantiate(powerUpRandomPrefab[5], posToSpawn, Quaternion.identity);
            }
 
            yield return new WaitForSeconds(Random.Range(10,11));
        }
       
    }

}
