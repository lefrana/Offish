using UnityEngine;
using TMPro;
using System.Collections;

public class NPCDialogue : MonoBehaviour
{
    public GameObject npc;
    public GameObject textBox;

    public TextMeshProUGUI textComponent;
    public float textSpeed = 0.05f;

    private string[] lines;
    private int index;
    private bool isTalking = false;

    public AudioSource audioSource;

    void Start()
    {
        if (!isTalking)
        {
            HideNPC();
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void SetDialogue(string[] newLines)
    {
        StopAllCoroutines();

        gameObject.SetActive(true);

        if (newLines == null || newLines.Length == 0 || string.IsNullOrEmpty(newLines[0]))
        {
            HideNPC();
            return;
        }

        isTalking = true;

        if (npc != null)
        {
            npc.SetActive(true);
        }

        if (textBox != null)
        {
            textBox.SetActive(true);
        }

        lines = newLines;
        textComponent.text = string.Empty;
        index = 0;

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        textComponent.text = string.Empty;
        isTalking = true;

        if (audioSource != null)
        {
            audioSource.Play();
        }

        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        isTalking = false;

        yield return new WaitForSeconds(2.0f);
        NextLine();
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            HideNPC();
        }
    }

    public void HideNPC()
    {
        isTalking = false;
        if (npc != null) { npc.SetActive(false); }
        if (textBox != null) { textBox.SetActive(false); }
        textComponent.text = string.Empty;

        gameObject.SetActive(false);
    }
}