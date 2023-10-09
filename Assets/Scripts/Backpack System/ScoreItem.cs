 using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Score", menuName = "Backpack/New Score")]
public class ScoreItem : ScriptableObject
{
    public AudioClip music;
    public float difficulty;

    public string musicName;
    public string musicVersionName;

    public float offsetTime = 0;
    public Sprite cover;

    [TextArea]
    public string scoreInfo;

    [Header("Preview")]
    public float previewFrom;
    public float previewTo;

    public bool Equals(ScoreItem other)
    {
        return music.Equals(other.music);
    }
}
