using UnityEngine;

public class FloatingAnimation : MonoBehaviour
{
    [Header("動きの設定")]
    public float speed = 2.0f;
    public float range = 10.0f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * speed) * range;

        transform.localPosition = startPos + new Vector3(0, newY, 0);
    }
}