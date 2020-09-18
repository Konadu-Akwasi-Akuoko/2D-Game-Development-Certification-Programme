using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private int _laserSpeed;
    
    // Update is called once per frame
    void Update()
    {
        //Using comparetags to check if there a laser is from enemy or player.
        if (gameObject.CompareTag("Laser"))
        {
            MoveUp();
        }
        else if (gameObject.CompareTag("EnemyLaser"))
        {
             MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

        if (transform.position.y >= 8)
        {
            Destroy(gameObject);
        }
        
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);
        if (transform.position.y <= -4.5)
        {
            Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }

}
