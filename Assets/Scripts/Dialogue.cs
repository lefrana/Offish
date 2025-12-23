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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textComponent.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }
    public void SetDialogue(string[] newLines)
    {
        gameObject.SetActive(true);

        lines = newLines;
        textComponent.text = string.Empty;
        index = 0;
        
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        charaAnim.SetBool("isTalking", true); //start talking animation

        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        charaAnim.SetBool("isTalking", false);
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
