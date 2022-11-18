using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using Plane = UnityEngine.Plane;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Clickable : MonoBehaviour
{
    //private Plane plane = new Plane(Vector3.down, 1.0f);
    public bool AllowInformationDisplay { get; set; } = false;
    public TowerMenu towerMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseDown()
    {

        //Vector3 np = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        //print("X: " + np.x + "  Y: " + np.y + "  Z: " + np.z);
        //objectmenu.transform.position = new Vector2(np.x, np.y + 65);
        if (AllowInformationDisplay)
        {
            towerMenu.showTowerInformation(gameObject);
        }

    }

}
