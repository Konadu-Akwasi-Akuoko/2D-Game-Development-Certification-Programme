using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speedOfPowerUp;
    [SerializeField]
    private int powerUpId;
    [SerializeField]
    private AudioClip _powerUpClip;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector3.up * _speedOfPowerUp * Time.deltaTime);

        if(transform.position.y <= -6.8f)
        {
            Destroy(this.gameObject);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_powerUpClip, transform.position);
            if (player != null)
            {
                switch (powerUpId)
                {

                    //using switch statements to see which powerup is collected 


                    case 0:

                        player.TrippleShotActive();
                        Destroy(this.gameObject);

                        break;
                    case 1:

                        player.SpeedActive();
                        Destroy(this.gameObject);

                        break;
                    case 2:
                        player.ShieldActive();
                        Destroy(this.gameObject);
                        break;
                    case 3:
                        player.AddAmmo();
                        Destroy(this.gameObject);
                        break;
                    case 4:
                        player.AddLives();
                        Destroy(this.gameObject);
                        break;

                }
            }         
           
        }
    }
}
