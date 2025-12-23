using UnityEngine;

public class ShotController : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, v, 0.0f);
        transform.position += move * moveSpeed * Time.deltaTime;
    }
}
