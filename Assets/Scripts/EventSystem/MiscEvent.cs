using System;
public class MiscEvent
{
    public event Action onEnemyKilled;
    public void EnemyKilled()
    {
        onEnemyKilled?.Invoke();
    }

    public event Action onFinishedDungeon;

    public void FinishDungeon()
    {
        onFinishedDungeon?.Invoke();
    }

}
