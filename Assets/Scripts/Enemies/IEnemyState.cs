public interface IEnemyState
{
    void Enter(Enemy enemy); // Se llama al entrar al estado
    void Update();            // Se llama cada frame
    void Exit();              // Se llama al salir del estado
}
