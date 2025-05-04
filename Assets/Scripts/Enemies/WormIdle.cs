using UnityEngine;

// Este script hace que el gusano se mueva o tiemble un poco para dar sensación de vida.
// Se puede ajustar para que haga un movimiento más suave o más inquietante según lo que busquemos.
public class WormIdle : MonoBehaviour
{
    public float moveAmplitude = 0.1f;  // Cuánto se mueve (pequeño)
    public float moveSpeed = 1f;        // Velocidad de la oscilación

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Movimiento suave en el eje Y (sube y baja ligeramente)
        float offsetY = Mathf.Sin(Time.time * moveSpeed) * moveAmplitude;
        transform.position = startPos + new Vector3(0, offsetY, 0);
    }
}
