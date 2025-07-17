using UnityEngine;

[CreateAssetMenu(menuName = "SOValues/int")]
public class SOInt : ScriptableObject
{
    public int value;

    public static implicit operator int(SOInt v)
    {
        return v.value;
    }
}
