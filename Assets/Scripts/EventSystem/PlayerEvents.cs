using System;

public class PlayerEvents
{
    public event Action<int> onLevelIncremented;

    public void LevelIncremented(int level)
    {
        onLevelIncremented?.Invoke(level);
    }

    public event Action onSceneChanged;

    public void SceneChanged()
    {
        UnityEngine.Debug.Log("Scene Changed Invoked");
        onSceneChanged?.Invoke();
    }

}
