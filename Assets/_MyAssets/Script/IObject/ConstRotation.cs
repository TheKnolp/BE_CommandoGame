using UnityEngine;

public class ConstRotation : MonoBehaviour
{
    public float rotationsPerMinute = 10f;
    void Update()
    {
        transform.Rotate(0, 0f, 6f * rotationsPerMinute * Time.deltaTime);
    }
}
