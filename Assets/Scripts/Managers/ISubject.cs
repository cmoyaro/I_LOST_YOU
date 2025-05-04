using System.Collections.Generic;

// Interfaz que define las funciones básicas que debe tener un sujeto: añadir, quitar y notificar observadores
public interface ISubject
{
    void AddObserver(IObserver observer);   // Para registrar un nuevo observador
    void RemoveObserver(IObserver observer); // Para eliminar un observador registrado
    void NotifyObservers();                 // Llama al método OnNotify() de todos los observadores
}
