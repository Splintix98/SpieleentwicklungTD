using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Draggable : MonoBehaviour
{

    private Plane plane = new Plane(Vector3.down, 1.0f);
    public Transform buildArea;
    public Transform buildAreaHalf;
    private float oldx, oldz;
    private bool allowConstruction = false;
    private Material normalMaterial;

    void Start()
    {

    }


    void Update()
    {

        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (plane.Raycast(ray, out float distance)) {
            Vector3 np = ray.GetPoint(distance);
            np.x = Mathf.RoundToInt(np.x + 0.5f) - 0.5f;
            np.z = Mathf.RoundToInt(np.z + 0.5f) - 0.5f;


            if (np.x != oldx || np.z != oldz)
            {
                Transform block = null;
                float height = 1;
                foreach (Transform o in buildArea.GetComponentsInChildren<Transform>())
                {
                    if (o.transform.position.x == np.x && o.transform.position.z == np.z) {
                        if (o.GetComponentsInChildren<Transform>().Length == 1) block = o;
                    }
                }
                foreach (Transform o in buildAreaHalf.GetComponentsInChildren<Transform>())
                {
                    if (o.transform.position.x == np.x && o.transform.position.z == np.z)
                    {
                        if (o.GetComponentsInChildren<Transform>().Length == 1) block = o;
                        height = 1.5f;
                    }
                }

                oldx = np.x;
                oldz = np.z;
                np.y = height;
                if (block != null)
                {
                    allowConstruction = true;
                    transform.position = new Vector3(np.x, np.y, np.z);

                }
                else {
                    allowConstruction = false;
                }


                


            }
        }

        if (Input.GetMouseButtonDown(0)) {
            if (allowConstruction) Destroy(this);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Destroy(gameObject);
            Destroy(this);
        }



    }



}

