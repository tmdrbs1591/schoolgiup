using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera maincamera;
    Vector3 cameraPos;
    [SerializeField]
    [Range(0.1f, 0.5f)] float ShakeRange = 0.5f;
    [SerializeField]
    [Range(0.1f, 1f)] float duration = 0.1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shake()
    {
        cameraPos = maincamera.transform.position;
        InvokeRepeating("StartShake", 0f, 0.005f);
        Invoke("StopShake", duration);
    }
    void StartShake()
    {
        float CameraPosX = Random.value * ShakeRange * 2 - ShakeRange;
        float CameraPosY = Random.value * ShakeRange * 2 - ShakeRange;
        Vector3 cameraPos = maincamera.transform.position;
        cameraPos.x = CameraPosX;
        cameraPos.y = CameraPosY;
        maincamera.transform.position = cameraPos;

    }
    void StopShake()
    {
        CancelInvoke("StartShake");
        maincamera.transform.position = cameraPos;
    }
}
