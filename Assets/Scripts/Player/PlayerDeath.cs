using System.Collections.Generic;
using UnityEngine;

// Este script va en el jugador y se encarga de notificar a otros cuando el jugador muere
public class PlayerDeath : MonoBehaviour, ISubject
{
    private List<IObserver> observers = new List<IObserver>(); // Lista de objetos que se enteran cuando muero
    private bool isDead = false; // Para evitar que se notifique más de una vez

    // Método público que se llama cuando el jugador muere
    public void Die()
    {
        if (isDead) return; // Si ya está muerto, no volvemos a notificar
        isDead = true;

        Debug.Log("El jugador ha muerto. Notificando"); 
        NotifyObservers(); // Avisamos a todos los observadores
    }

    // Añade un nuevo observador a la lista
    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
        Debug.Log("Se ha registrado un observador. Total: " + observers.Count);
    }

    // Elimina un observador de la lista
    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    // Llama a OnNotify() en cada observador
    public void NotifyObservers()
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotify();
        }
    }
}
