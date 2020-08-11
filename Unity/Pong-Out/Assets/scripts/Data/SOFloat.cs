using UnityEngine;

[CreateAssetMenu(menuName = "SOValues/float")]
public class SOFloat : ScriptableObject
{
    public float value;

    public static implicit operator float(SOFloat sofloat)
    {
        return sofloat.value;
    }
}
