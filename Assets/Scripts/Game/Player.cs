using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed=5.0f;

    //Here we created a variable from 1 to 10 thus the _thrusterScale, it's primary job is to reduce(--)
    //when the leftshift key is pressed and to replenish when the left key comes up.
    //And also we have _thrusterImg array, an image UI was used to visualize the scaling bar( thus _thrusterImg),
    //the _thrusterImg was used to show the scale at which the _thrusterScale was
    [SerializeField]
    private int _thrusterScale = default;
    [SerializeField]
    private Image[] _thrusterImg;


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
    //if this is true then you can instantite multishot.
    public bool _multiShotActive;
    [SerializeField]
    private GameObject _multishot;
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

    private int _laserCount = 15;
    [SerializeField]
    private Text _laserText;

    private UIManager _uiManager;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _noAmmoSound;
    [SerializeField]
    private AudioClip _laserSound;

    // camera shake script
    private CameraShake _camerashake;


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

        //laser count is always 15 at the start.
        _laserCount = 15;
        _laserText.text = "Ammo: " + _laserCount.ToString();

        //_number of hits is always = 0
        _numberOfHits = 0;

        // scaleThrusters is always 10 at start.
        _thrusterScale = 10;

        //accessing the camera shake script.
        _camerashake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        if (_camerashake == null)
        {
            Debug.LogError("Could not find CameraShake script");
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Calling the function responsible for the palyer movement.
        PlayerMovementCalculation();

        //creating a cooldown time for the laser
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canfire)
        {
            FireLaser();
        }

    }

    void PlayerMovementCalculation()
    {
        if (_sppedCheck == false)
        {
            //player movement in normal 
            playerSpeed = 5;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

            //Creating the thrusters, when the left shift key is pressed,
            //I created a courotine called ThrusterAdd/ThrusterSubstract, what it basically does is 
            //when the shift key is pressed it substracts 1 from _thrusterScale every 0.5secs
            //when realesed it also adds 1 to _thrusterScale every 1secs
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StopCoroutine(nameof(ThrusterAdd));
                StartCoroutine(nameof(ThrusterSubstract));
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                StopCoroutine(nameof(ThrusterSubstract));
                StartCoroutine(nameof(ThrusterAdd));
            }

            //the speed of the player is changed to 8, when the leftshift key is pressed,
            //when released it goes back to 5
            if (_thrusterScale > 0 && Input.GetKey(KeyCode.LeftShift))
            {
                playerSpeed = 8;
            }
            else
                playerSpeed = 5;

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

    void FireLaser()
    {
        //making _canfire greater than time.time so that, 
        //this will wait until time.time is greater than _canfire again
        _canfire = Time.time + fireRate;
        if (_trippleShotCheck == false && _multiShotActive == false) //instantiate normal laser
        {
            if(_laserCount <= 15 && _laserCount != 0)
            {
                _laserCount--;
                _laserText.text = "Ammo: " + _laserCount;
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), transform.rotation);

                //playing the audio source, when the fire key is pressed
                _audioSource.Play();
            }
            else if(_laserCount == 0)
            {
                _laserText.text = "Ammo: No Ammo";
                _audioSource.PlayOneShot(_noAmmoSound, 1);
            }
           
        }
        else if(_trippleShotCheck==true && _multiShotActive== false) //instantiate triple laser
        {
            TrippleShotActive();
            Instantiate(_trippleShot, transform.position + new Vector3(-0.18f, 0, 0), Quaternion.identity);
            //playing the audio source, when the fire key is pressed
            _audioSource.Play();
        }

        //if the bool turns true, instantiate all multishot
        else if(_multiShotActive == true)
        {
            StartCoroutine(MultiShotSwitch());
            GameObject clone = Instantiate(_multishot, transform.position + new Vector3(-2.131f, 2.25f, -3.341f), Quaternion.identity);

            //destroy cloned object in 7 seconds, by that time it would have leave the screen where the player sees
            if(clone != null)
            {
                Destroy(clone, 7.0f);
            }

        }
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
        
        //calling a function to call the courotine
        _camerashake.StartShake();

        if (_playerHealth == 0)
        {
            Destroy(this.gameObject);
        }

        //we need to check if palyer health is 2, if true-we need to randomly spawn a playerhurt.
        else if (_playerHealth == 2)
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

    public void MultiShotActive()
    {
        _multiShotActive = true;
    }

    public void AddScore()
    {
        _score+= 10;
        _uiManager.ScoreUpdater(_score);
    }

    public void AddAmmo()
    {
        int addAmmo;
        int substaract = 15;
        addAmmo = substaract - _laserCount;
        _laserCount += addAmmo;
        _laserText.text = "Ammo: " + _laserCount;
    }



    //this function adds player life
    public void AddLives()
    {
        if(_playerHealth == 3)
        {
            return;
        }

        _playerHealth++;
        Debug.Log(_playerHealth);
        _uiManager.UpdateLives(_playerHealth);

        if (_playerHealth == 2)
        {
            playerHurtVisualizer[1].SetActive(false);
        }
        else if (_playerHealth == 3)
        {
            playerHurtVisualizer[0].SetActive(false);
        }

    }


    //makes the tripple shot switch false
    IEnumerator TrippleShotSwitch()
    {
        yield return new WaitForSeconds(5.0f);
        _trippleShotCheck = false;
    }

    //makes the speed shot switch false
    IEnumerator SpeedSwitch()
    {
        yield return new WaitForSeconds(3.0f);
        _sppedCheck = false;
    }

    //this will make MultiShot stop working after 5 seconds
    IEnumerator MultiShotSwitch()
    {
        yield return new WaitForSeconds(5.0f);
        _multiShotActive = false;
    }


    // when this courotine is called, it substracts 1 from _thrusterScale
    //the for loop also updates the _thrusterImg, by using the SetActive function which can turn it on or off
    IEnumerator ThrusterSubstract()
    {
        while (true)
        {
            if (_thrusterScale > 0)
            {
                _thrusterScale -= 1;
            }

            for (int i = 0; i < 10; i++)
            {
                if (i <= _thrusterScale)
                {
                    _thrusterImg[i].gameObject.SetActive(true);
                }
                else
                    _thrusterImg[i].gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    // when this courotine is called, it adds 1 to _thrusterScale
    //the for loop also updates the _thrusterImg, by using the SetActive function which can turn it on or off
    IEnumerator ThrusterAdd()
    {
        while (true)
        {
            if (_thrusterScale < 10)
            {
                _thrusterScale += 1;
            }
            else
                yield break;

            for (int i = 0; i < 10; i++)
            {
                if (i <= _thrusterScale)
                {
                    _thrusterImg[i].gameObject.SetActive(true);
                }
                else
                    _thrusterImg[i].gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(1.0f);
        }
      
    }

    //calling damage function when we are hit by an enemy laser.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyLaser"))
        {
            Damage();
            Destroy(other.gameObject);
        }
    }

}
