namespace Interfaces.Fire
{
    public interface IFireModel
    {
        float MaxHealth { get; set; }
        float CurrentHealth { get; set; }
        float HitScore { get; set; }
        float PercantagePlayerHealthRecovery { get; set; }
    }
}
