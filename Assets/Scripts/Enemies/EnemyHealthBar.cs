using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private Image healthImag;

    private float maxHealthSize = 200;

    void Start()
    {
        RectTransform rectTransform = healthImag.GetComponent<RectTransform>();
        maxHealthSize = rectTransform.sizeDelta.x;
        canvas.SetActive(false);
    }


        // Update is called once per frame
    void Update()
    {

        gameObject.transform.rotation = Camera.main.transform.rotation;
        gameObject.transform.Rotate(0, 180f, 0);
    }



    public void updateEnemyHealthBar(float health, float startHealth) {
        canvas.SetActive(true);
        RectTransform rectTransform = healthImag.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2((health / startHealth) * maxHealthSize, rectTransform.sizeDelta.y);
    }


}
