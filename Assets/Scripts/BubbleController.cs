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
        if (shotInside && Input.GetKeyDown(KeyCode.X))
        {
            if (isOnTop())
            {
                Freeze();

                // Tell the generator to STOP the automatic arrow reset
                ShotGenerator sg = Object.FindFirstObjectByType<ShotGenerator>();
                if (sg != null)
                {
                    sg.isWaitingForDialogue = true;
                }

                ShowText();

                GameObject shot = GameObject.FindGameObjectWithTag("Shot");
                if (shot != null)
                {
                    Destroy(shot);
                }
            }
        }

    }

    void ShowText()
    {
        // Try to find Level 1
        LevelManager1 manager1 = Object.FindFirstObjectByType<LevelManager1>();
        if (manager1 != null)
        {
            manager1.CheckAnswer(type, gameObject);
            return; // Stop here if found
        }

        LevelManager2 manager2 = Object.FindFirstObjectByType<LevelManager2>();
        if (manager2 != null)
        {
            manager2.CheckAnswer(type, gameObject);
            return;
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

    // Add these to your BubbleController class

    public void Freeze()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false; // Optional: prevents it from being pushed by others while frozen
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
