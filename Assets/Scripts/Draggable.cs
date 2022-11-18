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
    private Transform selectedBlock = null;
    public ConstructionMenu constructionMenu;

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
                selectedBlock = null;
                float height = 1;
                foreach (Transform o in buildArea.GetComponentsInChildren<Transform>())
                {
                    if (o.transform.position.x == np.x && o.transform.position.z == np.z && o.parent == buildArea)
                    {
                        if (o.GetComponentsInChildren<Transform>().Length == 1) selectedBlock = o;
                        Debug.Log(o.GetComponentsInChildren<Transform>().Length + "   ");
                    }
                }
                foreach (Transform o in buildAreaHalf.GetComponentsInChildren<Transform>())
                {
                    if (o.transform.position.x == np.x && o.transform.position.z == np.z && o.parent == buildAreaHalf)
                    {
                        if (o.GetComponentsInChildren<Transform>().Length == 1) selectedBlock = o;
                        height = 1.5f;
                    }
                }

                oldx = np.x;
                oldz = np.z;
                np.y = height;
                if (selectedBlock != null)
                {
                    transform.position = new Vector3(np.x, np.y, np.z);
                }
            }


            if (selectedBlock != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    gameObject.transform.parent = selectedBlock;
                    Clickable clickable = gameObject.GetComponent<Clickable>();
                    clickable.AllowInformationDisplay = true;
                    constructionMenu.AllowNewTowerConstruction = true;
                    Destroy(this);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    Destroy(gameObject);
                    Destroy(this);
                }
            }
                



                



        }





    }



}

