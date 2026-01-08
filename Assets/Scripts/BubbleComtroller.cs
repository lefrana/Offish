using TMPro;
using UnityEngine;

public enum BubbleType { Correct, False1, False2, False3 }

public class BubbleController : MonoBehaviour
{
    [Header("movement")]
    public float moveSpeed = 3.0f;
    private Rigidbody2D rb;

    private bool shotInside = false;
    //private BubbleScript bubbleScript;

    private SpriteRenderer spriteRenderer;

    public BubbleType type;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Random.insideUnitCircle.normalized * moveSpeed;
        //bubbleScript = GetComponent<BubbleScript>();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //x button (keyboard)
        if (shotInside && Input.GetKeyDown(KeyCode.X))
        {
            if(isOnTop())
            {
                ShowText();

                GameObject shot = GameObject.FindGameObjectWithTag("Shot");
                if (shot != null)
                {
                    Destroy(shot);
                }

                Destroy(gameObject);
            }
        }

    }

    void ShowText()
    {
        LevelManager manager = Object.FindFirstObjectByType<LevelManager>();

        if (manager != null)
        {
            manager.CheckAnswer(type);
        }
        else
        {
            Debug.LogError("LevelManager not found");
        }
    }

    bool isOnTop()
    {
        //colliders at the bubbles center point
        Collider2D[] touches = Physics2D.OverlapPointAll(transform.position);

        int myOrder = spriteRenderer.sortingOrder;

        foreach (Collider2D hit in touches)
        {
            //compare layers with other bubbles
            if (hit.CompareTag("FloatingBubbles") && hit.gameObject != gameObject)
            {
                SpriteRenderer otherSR = hit.GetComponent<SpriteRenderer>();
                if (otherSR != null)
                {
                    if (otherSR.sortingOrder > myOrder)
                    {
                        return false;
                    }
                }
            }
        }

        //true if on the highest layer
        return true;
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
