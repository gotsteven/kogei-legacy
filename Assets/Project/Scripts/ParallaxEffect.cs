using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public GameObject cam;
    public float parallaxEffect;

    [Header("自動スクロール（雲など）")]
    [Tooltip("0なら動かない。数値を入れれば勝手に動く")]
    public float autoMoveSpeed = 0f;

    private float length;
    private float startPos;

    void Start()
    {
        if (cam == null) cam = Camera.main.gameObject;
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void LateUpdate()
    {
        startPos += autoMoveSpeed * Time.deltaTime;

        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}