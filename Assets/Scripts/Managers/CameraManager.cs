using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject target;

    private Quaternion firstRot;
    private Quaternion lastRot;

    private float camSmoothness = 6;
    private float xPos = 0;
    private float startMoveTimer = 0;
    private float startMoveDuration = 2;

    public bool cameraLocated;
    #region SINGLETON
    private static CameraManager instance;
    public static CameraManager Instance => instance ??= FindObjectOfType<CameraManager>();
    #endregion
    private void Start()
    {
        firstRot = transform.rotation;
        lastRot= Quaternion.Euler(0, 100, 0);
    }
    private void LateUpdate()
    {
        xPos = Mathf.Lerp(transform.position.x, target.transform.position.x, Time.deltaTime * camSmoothness);
        transform.position = new Vector3(xPos, transform.position.y, target.transform.position.z);
        if (Player.Instance.end && !cameraLocated)
        {
            StartMove();
        }
    }

    private void StartMove()
    {
        if (startMoveTimer <= startMoveDuration)
        {
            startMoveTimer += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(firstRot, lastRot, startMoveTimer / startMoveDuration);
        }
        else
        {
            cameraLocated = true;
            startMoveTimer = 0;
        }
    }
    public void CameraReset() 
    {
        transform.position = new Vector3(0, 1, 5);
        transform.rotation = firstRot;
        cameraLocated = false;
        startMoveTimer = 0;
    }
}
