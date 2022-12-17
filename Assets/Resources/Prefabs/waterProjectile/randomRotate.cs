using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class randomRotate : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // reset cooldown
        int Axis = Random.Range(0, 2);
        int Rotation = Random.Range(0, 10);
        if (Axis == 0)
        {
            this.transform.Rotate(new Vector4(Time.deltaTime / 5, Rotation, this.transform.rotation.y, this.transform.rotation.z));
        }
        if (Axis == 1)
        {
            this.transform.Rotate(new Vector4(Time.deltaTime / 5, this.transform.rotation.x, this.transform.rotation.y, Rotation));
        }

        //this.transform.SetPositionAndRotation(this.transform.position, Quaternion.Euler(new Vector3(this.transform.rotation.x + 30, 0, 0)));
    }
}
