// Interfaz que usarán todos los scripts que quieran reaccionar a un evento, como la muerte del jugador
public interface IObserver
{
    void OnNotify(); // Este método se ejecutará cuando el sujeto (el jugador en este caso) notifique algo
}
