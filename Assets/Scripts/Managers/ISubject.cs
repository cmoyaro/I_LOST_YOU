using System.Collections.Generic;

// Interfaz para sujetos observables en el patrón Observer
public interface ISubject
{
    void AddObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObservers();
}
