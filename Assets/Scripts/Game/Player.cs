using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed=5.0f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private float fireRate = 0.5f;
    private float _canfire = -1.0f;

    [SerializeField]
    private int _playerHealth = 3;

    [SerializeField]
    private GameObject _trippleShot;
    private bool _trippleShotCheck;
    private bool _sppedCheck;
    private bool _shieldActive;
    [SerializeField]
    private GameObject shieldVisualizer;
    private SpriteRenderer _spriteOfShield;
    int _numberOfHits = 0;
    [SerializeField]
    private GameObject[] playerHurtVisualizer = new GameObject[2];
 
    //[SerializeField]
    //private GameObject _gameOverVisualizer;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserSound;


    // Start is called before the first frame update
    void Start()
    {
        //Getting a handle on the gameobject "Canvas" so that we can acess the script "UIManager"
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if(_uiManager == null)
        {
            Debug.LogError("Uimanager is NULL");
        }

        //Grabbing the audiosource component from ths gameobject
        _audioSource = GetComponent<AudioSource>();

        //grabbing the sprite rendrer in the shieldVisualizer
        _spriteOfShield = shieldVisualizer.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        //Calling the function responsible for the palyer movement.
        playerMovementCalculation();

        //creating a cooldown time for the laser
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canfire)
        {
            fireLaser();
        }

    }

    void playerMovementCalculation()
    {
        if (_sppedCheck == false)
        {
            //player movement in normal 
            playerSpeed = 5;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

            //Creating the thrusters, when the left shift key is pressed,
            //the speed of the player is changed to 8, when released it goes back to 5
            if (Input.GetKey(KeyCode.LeftShift))
            {
                playerSpeed = 8;
            }

            transform.Translate(direction * playerSpeed * Time.deltaTime);
        }
        else
        {
            SpeedActive();
        }
        

        // player poition constraint and wraping on the x Axis.

        if (transform.position.x > 10)
        {
            transform.position = new Vector3(-10, transform.position.y, 0);
        }
        else if (transform.position.x < -10)
        {
            transform.position = new Vector3(10, transform.position.y, 0);
        }


        // player constraints on the y Axis
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

    }

    void fireLaser()
    {
        //making _canfire greater than time.time so that, 
        //this will wait until time.time is greater than _canfire again
        _canfire = Time.time + fireRate;
        if (_trippleShotCheck == false) //instantiate normal laser
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), transform.rotation);
        }
        else //instantiate triple laser
        {
            TrippleShotActive();
            Instantiate(_trippleShot, transform.position + new Vector3(-0.18f, 0, 0), Quaternion.identity);
        }

        //playing the audio source, when the fire key is pressed
        _audioSource.Play();
    }

    public void Damage()
    {
        //if shield = true and there is a hit this damage function is called, 
        //the return at the end of the if statement returns control to the calling function,
        //making any code following the if statement obsolete.
        if (_shieldActive == true)
        {
            _numberOfHits++;

            switch (_numberOfHits)
            {
                case 1:
                    _spriteOfShield.color = Color.green;
                    break;
                case 2:
                    _spriteOfShield.color = Color.red;
                    break;
                case 3:
                    _shieldActive = false;
                    _spriteOfShield.color = Color.white;
                    _numberOfHits = 0;
                    shieldVisualizer.SetActive(false);
                    break;
            }

            return;
        }

        _playerHealth -= 1;
        _uiManager.UpdateLives(_playerHealth);
        if (_playerHealth == 0)
        {
            Destroy(this.gameObject);
        }

        //we need to check if palyer health is 2, if true-we need to randomly spawn a playerhurt.
        if (_playerHealth == 2)
        {
            playerHurtVisualizer[0].SetActive(true);
        }

        //we also need to check if player health is one, if true we need to spawn the remaining one.
        else if (_playerHealth == 1)
        {
            playerHurtVisualizer[1].SetActive(true);
        }

    }

    public void TrippleShotActive()
    {
        _trippleShotCheck = true;
        StartCoroutine(TrippleShotSwitch());
    }

    public void SpeedActive()
    {
        playerSpeed = 10;
        _sppedCheck = true;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * playerSpeed * Time.deltaTime);
        StartCoroutine(SpeedSwitch());
    }

    public void ShieldActive()
    {
        _shieldActive = true;
        shieldVisualizer.SetActive(true);
    }

    public void AddScore()
    {
        _score+= 10;
        _uiManager.ScoreUpdater(_score);
    }

    //makes the tripple shot switch false
    IEnumerator TrippleShotSwitch()
    {
        yield return new WaitForSeconds(3.0f);
        _trippleShotCheck = false;
    }

    //makes the speed shot switch false
    IEnumerator SpeedSwitch()
    {
        yield return new WaitForSeconds(3.0f);
        _sppedCheck = false;
    }

    //calling damage function when we are hit by an enemy laser.
    //NB: One of the enemyLaser's rigidbody is removed so it cant also call Danage();
    //Thus though EnemyLaser is 2 only one have arigidbody capable of colliding
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyLaser"))
        {
            Damage();
            Destroy(other.gameObject);
        }
    }

}
