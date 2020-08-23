using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class AsteriodBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed=20.0f;
    [SerializeField]
    private GameObject _explosion;
    private SpawnManager spawnManager;
    private void Start()
    {
        spawnManager = GameObject.Find("_Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * _rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Laser"))
        {
            //when the laser collides
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            spawnManager.startSpawning();
            Destroy(this.gameObject);           
        }
    }

}
