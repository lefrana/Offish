using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    private BubbleType bubbleType;
    private Dialogue dialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BubbleKey key = GetComponent<BubbleKey>();

        if (key != null)
        {
            bubbleType = key.type;
        }

        //bubble prefab finds dialogue object automatically
        dialogue = Object.FindFirstObjectByType<Dialogue>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowText()
    {
        if (dialogue == null)
        {
            Debug.LogError("DialogueUI is not assigned on " + gameObject.name);
            return;
        }

        string message = "";
        switch (bubbleType)
        {
            case BubbleType.Correct: message = "correct bubble"; break;
            case BubbleType.False1: message = "false bubble 1"; break;
            case BubbleType.False2: message = "false bubble 2"; break;
            default: message = "false bubble 3"; break;
        }

        dialogue.SetDialogue(new string[] { message });
    }
}
