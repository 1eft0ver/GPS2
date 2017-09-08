using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRotate : MonoBehaviour {

    private void Start()
    {
        transform.RotateAround(Camera.main.transform.position, Vector3.up, 90);
    }

    private void Update()
    {
        //transform.RotateAround(Camera.main.transform.position, Vector3.up, 10 * Time.deltaTime);
    }
}
