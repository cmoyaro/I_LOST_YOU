using UnityEngine;

// Script para definir una llave única
[CreateAssetMenu(fileName = "NewKeyData", menuName = "Keys/Key Data")]

public class KeyData : ScriptableObject
{
    public string keyID;      // ID única de la llave (Key_A, etc.)
    public string description; // Descripción opcional (apartado todavía no terminado)
}
