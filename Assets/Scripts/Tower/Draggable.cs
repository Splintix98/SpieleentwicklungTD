using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Plane = UnityEngine.Plane;
using Vector3 = UnityEngine.Vector3;

public class Draggable : MonoBehaviour
{
    public static bool Enable { get; set; } = true;

    private Plane plane = new Plane(Vector3.down, 1.0f);
    public Transform buildArea;
    public Transform buildAreaHalf;
    private float oldx, oldz;
    private Transform currentTowerBlock = null;
    public ConstructionMenu constructionMenu;
    TowerController towerController;
    
    public AudioSource createTowerAudioSource;
    void Start()
    {
        towerController = gameObject.GetComponent<TowerController>();
        TowerController.Clickable = false;
    }


    void Update()
    {
        if (!Enable) return;
       
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 np = ray.GetPoint(distance);
            np.x = Mathf.RoundToInt(np.x + 0.5f) - 0.5f;
            np.z = Mathf.RoundToInt(np.z + 0.5f) - 0.5f;


            if (np.x != oldx || np.z != oldz)
            {
                float height = 1;
                Transform selectedBlock = null;
                foreach (Transform o in buildArea.GetComponentsInChildren<Transform>())
                {
                    if (o.transform.position.x == np.x && o.transform.position.z == np.z && o.parent == buildArea)
                    {
                        if (o.GetComponentsInChildren<Transform>().Length == 1)
                        {
                            selectedBlock = o;
                        }
                    }
                }
                foreach (Transform o in buildAreaHalf.GetComponentsInChildren<Transform>())
                {
                    if (o.transform.position.x == np.x && o.transform.position.z == np.z && o.parent == buildAreaHalf)
                    {
                        if (o.GetComponentsInChildren<Transform>().Length == 1)
                        {
                            selectedBlock = o;
                        }
                        height = 1.5f;
                    }
                }

                oldx = np.x;
                oldz = np.z;
                np.y = height;
                if (selectedBlock != null)
                {
                    currentTowerBlock = selectedBlock;
                    gameObject.transform.position = new Vector3(np.x, np.y, np.z);
                    towerController.EnableLineRender = true;
                }
            }


            if (currentTowerBlock != null)
            {
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    gameObject.transform.parent = currentTowerBlock;
                    TowerController.Clickable = true;
                    ConstructionMenu.AllowNewTowerConstruction = true;
                    towerController.EnableLineRender = false;
                    towerController.EnableShoot = true;
                    createTowerAudioSource.transform.position = gameObject.transform.position;
                    createTowerAudioSource.Play();
                    PlayerStats.Instance.SpendCoins(towerController.ConstructionCosts);
                    Destroy(this);
                }

                if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
                {
                    ConstructionMenu.AllowNewTowerConstruction = true;
                    TowerController.Clickable = true;
                    Destroy(gameObject);
                    Destroy(this);
                }
            }
        }
    }
}


