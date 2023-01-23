namespace Interfaces.Enemy
{
    public interface IEnemyModel
    {
        float MaxHealth { get; set; }
        float CurrentHealth { get; set; }
        float MoveSpeed { get; set; }
        float Damage { get; set; }
    }
}