using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public GameObject cam;
    public float parallaxEffect;

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
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}