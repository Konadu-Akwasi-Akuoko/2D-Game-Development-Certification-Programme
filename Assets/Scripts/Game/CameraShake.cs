using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake
	[SerializeField]
	private Transform cameraTransform = default;

	// How long the object should shake for.
	private float shakeDuration = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	private float shakeAmount = 0.2f;

    //the original position of the camera.
	private Vector3 originalPos;

	void Start()
	{
		if (cameraTransform == null)
		{
			cameraTransform = GetComponent<Transform>();
		}
		originalPos = cameraTransform.localPosition;
	}

	public void StartShake()
    {
		StartCoroutine(CameraShaker());
    }

	IEnumerator CameraShaker()
	{
		//set shakeduration or time to 0.2 seconds everytime this courotine is called.
		shakeDuration = 0.2f;
		shakeAmount = 0.2f;
		while (true)
        {
			if (shakeDuration > 0)
			{
				//moves the camera randomly to a point inside a circle with radius 1
				//but because we multiplies it wieh the shakeamount the radius now becomes 0.2f
				cameraTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
				//subtracting time.deltatime from the shake duration.
				shakeDuration -= Time.deltaTime;
			}

			else
			{
				shakeDuration = 0f;

				// returns the camer to its original posetion.
				cameraTransform.localPosition = originalPos;
				
				//yield break stops the courotine when shakeduration = 0.
				yield break;
			}

			//waits for the next frame to update the while loop.
			yield return null;
		}
    }

}