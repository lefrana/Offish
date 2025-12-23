using UnityEngine;

public class BubbleGenerator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //public GameObject   textBubble;
    public float moveSpeed = 3.0f;
    private Rigidbody2D rb;

    private bool shotInside = false;
    //private Vector2 direction;

    //private int screenMaxX = 1920;
    //private int screenMaxY = 1080;

    void Start()
    {
        //direction = Random.insideUnitCircle.normalized;
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Random.insideUnitCircle.normalized * moveSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        // X button (keyboard)
        if (shotInside && Input.GetKeyDown(KeyCode.X))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        rb.linearVelocity = randomDir * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Shot"))
        {
            shotInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Shot"))
        {
            shotInside = false;
        }
    }
}
