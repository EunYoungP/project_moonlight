using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    private Camera shakeCamera;
    private float shakeTime;
    private float shakeIntensity;

    private PlayerController playerController;

    private static ShakeCamera instance;
    public static ShakeCamera Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("ShakeCamera", typeof(ShakeCamera));
                DontDestroyOnLoad(obj);
                instance = obj.GetComponent<ShakeCamera>();
            }
            return instance;
        }
    }

    public ShakeCamera()
    {
        playerController = Player.Instance.playerController;
    }

    public void OnShakeCamera(float shakeTime, float shakeIntensity)
    {
        shakeCamera = Camera.main;
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StartCoroutine(ShakeCameraByRotation());
    }

    IEnumerator ShakeCameraByRotation()
    {
        Vector3 startRotation = shakeCamera.transform.eulerAngles;
        float shakePower = 20f;

        while(shakeTime > 0)
        {
            playerController.OnShakeCamera = true;

            float x = 0;
            float y = 0;
            float z = Random.Range(-1, 1);

            shakeCamera.transform.rotation = Quaternion.Euler(startRotation + new Vector3(x, y, z) * shakeIntensity * shakePower);

            shakeTime -= Time.deltaTime;
            yield return null;
        }
        shakeCamera.transform.rotation = Quaternion.Euler(startRotation);

        playerController.OnShakeCamera = false;
    }
}
