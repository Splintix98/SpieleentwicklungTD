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
    public bool allowInformationDisplay { get; set; } = false;
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
        if (allowInformationDisplay)
        {
            towerMenu.ShowTowerInformation(gameObject);
        }

    }

}
