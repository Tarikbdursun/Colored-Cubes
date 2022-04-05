using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeToStart : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Instance.GameStart();
        }
    }
}
