using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int enemySpeed = 4;
    [SerializeField]
    private GameObject _laserPfefab;
    private Player _player;
    private Animator _anim;
    private AudioSource _enemyExplosion;
    private float _fireRate = 3.0f;
    private float _canFire = -1.0f;

    private SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        
        transform.position = new Vector3(Random.Range(-9.0f, 9.0f), 7, transform.position.z);
        
        // Rotating the enemy with an angle, this makes the enemy a little bit slanted.
        // The enemy also goes forward, but also at an angle.
        transform.Rotate (transform.rotation.x, transform.rotation.y, Random.Range(-45, 45));

        _player = GameObject.Find("Player").GetComponent<Player>();
        
        if(_player == null)
        {
            Debug.LogError("The player is NULL");
        }
        

        _anim = GetComponent<Animator>();
        if(_anim == null)
        {
            Debug.LogError("The animator is NULL");
        }

        _enemyExplosion = GetComponent<AudioSource>();

        //
        _spawnManager = GameObject.FindWithTag("Spawn Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL");
        }

    }
    // Update is called once per frame
    void Update()
    {
        //_spawnManager.EnemyKilled();
        CalculateMovementg();

        EnemyFireLaser();
    }

    void CalculateMovementg()
    {
                transform.Translate(-Vector3.up * enemySpeed * Time.deltaTime);

                if (transform.position.y <= -5.5f)
                {
                    transform.position = new Vector3(Random.Range(-9.0f, 9.0f), 7, transform.position.z);
                }

    }

    //At every 3 to 7 seconds a laser is fired.
    void EnemyFireLaser()
    {
        if (gameObject != null)
        {
            // noticed that the enemy still fires even when dead because we desteroy the collider first,
            //checking, when collider is null don't fire
            if(gameObject.GetComponent<Collider2D>() != null && Time.time > _canFire)
            {
                    _fireRate = Random.Range(3.0f, 7.0f);
                    _canFire = Time.time + _fireRate;
                    Instantiate(_laserPfefab, transform.position, transform.rotation);
            }
            
        }
      
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("We collided");
        if (other.gameObject.CompareTag("Player"))
        {
            //make enemy stop
            enemySpeed = 0;

            //Calling this function on SpwanManager.cs to increment the value of enemies killed
            _spawnManager.EnemyKilled();

            //Debug.Log("If is called");
            if (other != null)
            {
                _player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");

            _enemyExplosion.Play();

            //
            //

            //first destroy the collider before the actual destroy 
            Destroy(GetComponent<Collider2D>());

            Destroy(this.gameObject,2.8f);
        }

        else if (other.gameObject.CompareTag("Laser"))
        {
            enemySpeed = 0;
            _spawnManager.EnemyKilled();
            Destroy(other.gameObject);

            if(_player != null)
            {
                _player.AddScore();
            }

            //calls this trigger when a laser collides with our object and plays the animation
            _anim.SetTrigger("OnEnemyDeath");
            _enemyExplosion.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject,2.8f);
        }

    }

}
