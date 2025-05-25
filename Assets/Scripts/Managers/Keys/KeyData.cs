using UnityEngine;

// ScriptableObject que representa una llave única dentro del juego
[CreateAssetMenu(fileName = "NewKeyData", menuName = "Keys/Key Data")]
public class KeyData : ScriptableObject
{
    public string keyID;
    public string description;
}
