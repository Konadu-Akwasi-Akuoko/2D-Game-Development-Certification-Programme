using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//explosion for the asteriod

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        //the audio source is played on wake
        //thus when this explosion becomes a gameobject it automically play.

        Destroy(this.gameObject, 3);
    }

}
