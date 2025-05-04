using System.Collections.Generic;
using UnityEngine;

// Este script gestiona las llaves que ha recogido el jugador
public class PlayerInventory : MonoBehaviour
{
    private List<KeyData> keys = new List<KeyData>();

    public void AddKey(KeyData key)
    {
        if (!keys.Contains(key))
        {
            keys.Add(key);
            Debug.Log($"Llave añadida al inventario: {key.name}");
        }
    }


    public bool HasKey(KeyData key)
    {
        return keys.Contains(key);
    }
}
