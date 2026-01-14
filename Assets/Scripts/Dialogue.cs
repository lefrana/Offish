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
        gameObject.SetActive(true);

        lines = newLines;
        textComponent.text = string.Empty;
        index = 0;
        
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        charaAnim.SetBool("isTalking", true); //start fish talking animation

        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        charaAnim.SetBool("isTalking", false);

        //dialogue advances automatically
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
