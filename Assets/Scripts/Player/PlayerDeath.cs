using System.Collections.Generic;
using UnityEngine;

// Notifica a los observadores cuando el jugador muere (patrón Observer)
public class PlayerDeath : MonoBehaviour, ISubject
{
    private List<IObserver> observers = new List<IObserver>();
    private bool isDead = false;

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        NotifyObservers();
    }

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach (IObserver observer in observers)
        {
            observer.OnNotify();
        }
    }
}
