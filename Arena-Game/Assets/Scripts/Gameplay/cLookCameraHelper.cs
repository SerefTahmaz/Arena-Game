using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cLookCameraHelper : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = Camera.main.transform.eulerAngles;
        // rot.y += 180;
        rot.x = 0;
        rot.z = 0;
        transform.eulerAngles = rot;
    }
}
