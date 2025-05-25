using System.Collections.Generic;
using UnityEngine;

// Gestiona las llaves recogidas por el jugador
public class PlayerInventory : MonoBehaviour
{
    private List<KeyData> keys = new List<KeyData>();

    public void AddKey(KeyData key)
    {
        if (!keys.Contains(key))
        {
            keys.Add(key);
        }
    }

    public bool HasKey(KeyData key)
    {
        return keys.Contains(key);
    }
}
