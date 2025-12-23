using UnityEngine;

public enum ArrowType
{
    Up,
    Down,
    Left,
    Right
}

public class ArrowKey : MonoBehaviour
{
    public ArrowType type;

    public bool Check(ArrowType input)
    {
        if (input == type)
        {
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}
