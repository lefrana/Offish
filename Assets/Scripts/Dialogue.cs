using UnityEngine;
using TMPro;
using System.Collections;

public class Dialogue : MonoBehaviour
{
    public Animator charaAnim;

    public TextMeshProUGUI textComponent;
    private string[] lines;
    public float textSpeed;

    private int index;

    void Start()
    {
        textComponent.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetDialogue(string[] newLines)
    {
        // 1. CRITICAL FIX: Kill any typing or waiting currently happening
        StopAllCoroutines();

        gameObject.SetActive(true);

        lines = newLines;
        textComponent.text = string.Empty;
        index = 0;

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        textComponent.text = string.Empty; // Clear text again just to be safe
        charaAnim.SetBool("isTalking", true);

        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        charaAnim.SetBool("isTalking", false);

        // 2. The Auto-Advance Timer
        // If SetDialogue is called again during these 2 seconds, 
        // StopAllCoroutines() above will prevent NextLine() from being called twice.
        yield return new WaitForSeconds(2.0f);
        NextLine();
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
            charaAnim.SetBool("isTalking", false);
        }
    }
}
