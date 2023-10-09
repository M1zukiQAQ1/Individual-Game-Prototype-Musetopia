using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Collections;

[Serializable]
public class Note_New
{
    public double timeToPlay;
    public bool isLongPress;

    public Note_New(double t, bool lp)
    {
        timeToPlay = t;
        isLongPress = lp;
    }
}

class NoteComparer : IComparer<Note_New>
{
    public int Compare(Note_New x, Note_New y)
    {
        if(x.timeToPlay != y.timeToPlay)
        {
            return x.timeToPlay.CompareTo(y.timeToPlay);
        }
        else
        {
            return x.isLongPress.CompareTo(y.isLongPress);
        }
    }
}

[Serializable]
public class Score_New
{
    public List<Note_New> notes_new = new();
}

[Serializable]
public class FullScore
{
    public List<Score_New> scores = new();
    public string name;
}

public class ScoreGenerationSceneController : MonoBehaviour
{
    protected FullScore fs;
    public static ScoreGenerationSceneController sgsc;

    [Header("Note Generator Panel")]
    public TMP_InputField filename;
    public TMP_InputField tempo;
    public TMP_InputField musicName;
    public TMP_InputField timeToForwardField;
    public TMP_Text timeText;
    public TMP_Dropdown playerSelection;
    public TMP_Dropdown noteSelection;
    public List<TMP_Text> noteTexts = new();
    public Toggle noteIndicatorToggle;
    public Image noteIndicator;

    [Header("Tempo Detection Panel")]
    public TMP_Text tempoDetectionText;
    public TMP_Text tempoDetectedText;
    private List<double> tempoTimeList = new();

    [Header("Misc")]
    public TMP_Text musicTimeScale;
    public AudioSource music;
    public double acceptedOffsetValue;
    public int playerNum;
    public List<RectTransform> panels = new();
    public string playSceneName = "SampleScene";

    private int noteIndex = 0;
    private Stack<int> playerAddedHistory = new();
    private Stack<Note_New> noteAddedHistory = new();
    private NoteComparer comparer = new();
    private double indicatorTimer;
    private double tempoSaved;

    private void Start() 
    {
        fs = new();
        noteIndicator.color = Color.white;
        for(int i = 0; i < playerNum; i++)
        {
            fs.scores.Add(new Score_New());
        }
        if(sgsc == null)
        {
            sgsc = this;
        }
    }

    private IEnumerator BlinkIndicator()
    {
        noteIndicator.color = Color.red;
        yield return new WaitForSeconds(0.07f);
        noteIndicator.color = Color.white;
    }

    public void SwitchToPlayScene()
    {
        SceneManager.LoadScene(playSceneName);
    }

    private void Update()
    {
        if (panels[0].gameObject.activeSelf)
        {
            UpdateNoteText();
            timeText.text = music.time.ToString("0.00");
            if(music.isPlaying && Input.GetKeyDown(KeyCode.Space))
            {
                AddNote(music.time);
            }

            if (noteIndicatorToggle.isOn && music.isPlaying)
            {

                if (fs.scores[playerSelection.value].notes_new.Count - 1 >= noteIndex && fs.scores[playerSelection.value].notes_new[noteIndex].timeToPlay <= music.time)
                {
                    StartCoroutine(nameof(BlinkIndicator));
                    noteIndex++;
                }
                
            }
        }

        else if(panels[1].gameObject.activeSelf)
        {
            if(music.isPlaying && Input.GetKeyDown(KeyCode.Space))
            {
                tempoTimeList.Add(music.time);
                DetectTempo();
            }

            UpdateTempoDetectionText();
        }
    }

    public void ResetTempoDetection()
    {
        tempoDetectedText.text = "";
        tempoDetectionText.text = "";
        tempoTimeList.Clear();
    }

    public void RemoveRepetitiveNote()
    {
        for(int i = 0; i < fs.scores[playerSelection.value].notes_new.Count - 1; i++)
        {
            if(Math.Abs(fs.scores[playerSelection.value].notes_new[i].timeToPlay - fs.scores[playerSelection.value].notes_new[i + 1].timeToPlay) < 0.01f)
            {
                Debug.Log("Found notes: " + fs.scores[playerSelection.value].notes_new[i].timeToPlay + " " + fs.scores[playerSelection.value].notes_new[i + 1].timeToPlay);
                fs.scores[playerSelection.value].notes_new.RemoveAt(i);
            }
        }
        SortNoteList();
    }

    public void OnToggledNoteIndicator()
    {
        noteIndex = 0;
        if (noteIndicator.gameObject.activeInHierarchy)
        {
            noteIndicator.gameObject.SetActive(false);
            return;
        }
        else
        {
            noteIndicator.gameObject.SetActive(true);
        }
    }

    public void ShiftRefListForward()
    {
        for(int i = 0; i < fs.scores[(int)ROLE.NONE].notes_new.Count; i++)
        {
            fs.scores[(int)ROLE.NONE].notes_new[i].timeToPlay += (60 / tempoSaved) / 4;
        }
        tempo.text = "Shifted!";
    }

    public void GenerateReferenceScore()
    {
        fs.scores[(int)ROLE.NONE].notes_new.Clear();
        double timeInterval = (60 / Convert.ToDouble(tempo.text)) / 2;
        for(double timeCc = 0; timeCc < music.clip.length; timeCc += timeInterval)
        {
            fs.scores[(int)ROLE.NONE].notes_new.Add(new Note_New(timeCc, false));
        }
        tempoSaved = Convert.ToDouble(tempo.text);
        tempo.text = "Generated!";
    }
    
    public void DetectTempo()
    {
        double averageTimeInterval = 0.0f;
        int cc = 0;
        for(int i = 0; i < tempoTimeList.Count - 1; i++)
        {
            averageTimeInterval += Math.Abs(tempoTimeList[i] - tempoTimeList[i + 1]);
            cc++;
        }
        averageTimeInterval /= cc;
        tempoDetectedText.text = (60 / averageTimeInterval).ToString("0.00");
    }

    public void UpdateTempoDetectionText()
    {
        string str = "";
        for(int i = 0; i < tempoTimeList.Count; i++)
        {
            str += tempoTimeList[i].ToString("0.0") + "  ";
        }
        tempoDetectionText.text = str;
    }

    public void ForwardToTime()
    {
        noteIndex = 0;
        music.time = (float)Convert.ToDouble(timeToForwardField.text);
    }

    public void SortNoteList()
    {
        fs.scores[playerSelection.value].notes_new.Sort(comparer);
    }

    public void SwitchPanel()
    {
        for(int i = 0; i < panels.Count; i++)
        {
            if (panels[i].gameObject.activeInHierarchy)
            {
                panels[i].gameObject.SetActive(false);
                panels[(i + 1) % panels.Count].gameObject.SetActive(true);
                return;
            }
        }
    }

    public void ReadData()
    {
        string json;
        string filepath = Application.streamingAssetsPath + "/" + filename.text;

        //Resource Management
        using (System.IO.StreamReader sr = new System.IO.StreamReader(filepath))
        {
            json = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
        }
        fs = JsonUtility.FromJson<FullScore>(json);
        musicName.text = fs.name;
        UpdateNoteText();
        UpdateNoteDropDown();
    }

    public void SaveData()
    {
        fs.name = musicName.text;
        string json = JsonUtility.ToJson(fs, true);
        string filepath = Application.streamingAssetsPath + "/" + filename.text;

        using (System.IO.StreamWriter sw = new System.IO.StreamWriter(filepath))
        {
            sw.WriteLine(json);
            sw.Close();
            sw.Dispose();
        }
        filename.text = "Saved!";
    }
    public void SlowDownMusicSpeed()
    {
        music.pitch -= 0.05f;
        musicTimeScale.text = music.pitch.ToString();
    }

    public void SpeedUpMusicSpeed()
    {
        music.pitch += 0.05f;
        musicTimeScale.text = music.pitch.ToString();
    }

    public void AdjustAccordingToRefList()
    {
        double timeDifference = double.MaxValue;
        for (int i = 0; i < fs.scores[(int)ROLE.NONE].notes_new.Count; i++)
        {

            for(int m = 0; m < fs.scores[playerSelection.value].notes_new.Count; m++)
            {
                if(Math.Abs(fs.scores[playerSelection.value].notes_new[m].timeToPlay - fs.scores[(int)ROLE.NONE].notes_new[i].timeToPlay) <= acceptedOffsetValue && Math.Abs(fs.scores[playerSelection.value].notes_new[m].timeToPlay - fs.scores[(int)ROLE.NONE].notes_new[i].timeToPlay) > 0.00001f && Math.Abs(fs.scores[playerSelection.value].notes_new[m].timeToPlay - fs.scores[(int)ROLE.NONE].notes_new[i].timeToPlay) <= timeDifference)
                {
                    Debug.Log("Found notes: " + fs.scores[playerSelection.value].notes_new[m].timeToPlay + "  " + fs.scores[(int)ROLE.NONE].notes_new[i].timeToPlay);
                    timeDifference = Math.Abs(fs.scores[playerSelection.value].notes_new[m].timeToPlay - fs.scores[(int)ROLE.NONE].notes_new[i].timeToPlay);
                    fs.scores[playerSelection.value].notes_new[m].timeToPlay = fs.scores[(int)ROLE.NONE].notes_new[i].timeToPlay;
                }

            }
            timeDifference = double.MaxValue;
        }
    }

    //public void AdjustAllNote()
    //{
    //    for(int i = 0; i < fs.scores[(int)ROLE.NONE].notes_new.Count; i++)
    //    {
    //        AdjustNoteSelected(fs.scores[(int)ROLE.NONE].notes_new[i]);
    //    }
    //}

    //private void AdjustNoteSelected(Note_New note)
    //{
    //    double timeDifference = double.MaxValue;
    //    for (int i = 0; i < fs.scores.Count; i++)
    //    {
    //        if (i == playerSelection.value)
    //        {
    //            continue;
    //        }

    //        for (int j = 0; j < fs.scores[i].notes_new.Count; j++)
    //        {
    //            if (Math.Abs(fs.scores[i].notes_new[j].timeToPlay - note.timeToPlay) <= acceptedOffsetValue && Math.Abs(fs.scores[i].notes_new[j].timeToPlay - note.timeToPlay) > 0.00001f && Math.Abs(fs.scores[i].notes_new[j].timeToPlay - note.timeToPlay) <= timeDifference)
    //            {
    //                Debug.Log("Found notes: " + note.timeToPlay + "  " + fs.scores[i].notes_new[j].timeToPlay);
    //                timeDifference = Math.Abs(fs.scores[i].notes_new[j].timeToPlay - note.timeToPlay);
    //                fs.scores[i].notes_new[j].timeToPlay = note.timeToPlay;
    //            }
    //        }

    //    }
    //}

    //public void AdjustNoteSelected()
    //{
    //    double timeDifference = double.MaxValue;
    //    for (int i = 0; i < fs.scores.Count; i++)
    //    {
    //        if (i == playerSelection.value)
    //        {
    //            continue;
    //        }
    //        for (int j = 0; j < fs.scores[i].notes_new.Count; j++)
    //        {
    //            if (Math.Abs(fs.scores[i].notes_new[j].timeToPlay - fs.scores[playerSelection.value].notes_new[noteSelection.value].timeToPlay) <= acceptedOffsetValue && Math.Abs(fs.scores[i].notes_new[j].timeToPlay - fs.scores[playerSelection.value].notes_new[noteSelection.value].timeToPlay) <= timeDifference)
    //            {
    //                Debug.Log("Found notes: " + fs.scores[playerSelection.value].notes_new[noteSelection.value].timeToPlay + "  " + fs.scores[i].notes_new[j].timeToPlay);
    //                timeDifference = Math.Abs(fs.scores[i].notes_new[j].timeToPlay - fs.scores[playerSelection.value].notes_new[noteSelection.value].timeToPlay);
    //                fs.scores[i].notes_new[j].timeToPlay = fs.scores[playerSelection.value].notes_new[noteSelection.value].timeToPlay;
    //            }
    //        }

    //    }
    //}

    public void UpdateNoteDropDown()
    {
        noteSelection.ClearOptions();
        for(int i = 0; i < fs.scores[playerSelection.value].notes_new.Count; i++)
        {
            noteSelection.options.Add(new TMP_Dropdown.OptionData(fs.scores[playerSelection.value].notes_new[i].timeToPlay.ToString("0.00")));
        }
    }

    public void RemoveAtIndex()
    {
        fs.scores[playerSelection.value].notes_new.RemoveAt(noteSelection.value);
        UpdateNoteDropDown();
    }

    public void FastFoward()
    {
        music.time += 0.5f;
    }

    public void Rewind()
    {
        music.time -= 0.5f;
    }

    public void RemoveLatest()
    {
        int playerIndex = playerAddedHistory.Pop();
        Note_New noteToRemove = noteAddedHistory.Pop();

        for(int i = 0; i < fs.scores[playerIndex].notes_new.Count; i++)
        {
            if(fs.scores[playerIndex].notes_new[i].timeToPlay == noteToRemove.timeToPlay)
            {
                fs.scores[playerIndex].notes_new.RemoveAt(i);       
            }
        }
    }

    public void RemoveAll()
    {
        fs.scores[playerSelection.value].notes_new.RemoveRange(0, fs.scores[playerSelection.value].notes_new.Count);
    }

    public void ResetMusic()
    {
        music.Stop();
        music.time = 0;
        noteIndex = 0;
    }

    public void PuaseMusic()
    {
        music.Pause();
    }

    public void StartPlaying()
    {
        music.Play();
    }

    private void UpdateNoteText()
    {
        List<string> strs = new();
        string temp = "";

        for(int i = 0; i < fs.scores.Count; i++)
        {
            for (int j = 0; j < fs.scores[i].notes_new.Count; j++)
            {
                temp += $"{fs.scores[i].notes_new[j].timeToPlay:0.0} ";
            }
            strs.Add(temp);
            temp = "";
        }

        for(int i = 0; i < fs.scores.Count; i++)
        {
            if(i == 3)
            {
                break;
            }

            if(strs[i] == "")
            {
                noteTexts[i].text = "Press Space To Add Note";
                continue;
            }

            noteTexts[i].text = strs[i];
        }

        for(int i = 0; i < noteTexts.Count; i++)
        {
            if(playerSelection.value == i)
            {
                noteTexts[i].color = new Color(0, 191, 255);
            }
            else
            {
                noteTexts[i].color = Color.white;
            }
        }

    }

    public void AddNote(double time)
    {
        Debug.Log(playerSelection.value);

        Note_New noteToAdd = new(time, false);
        fs.scores[playerSelection.value].notes_new.Add(noteToAdd);

        playerAddedHistory.Push(playerSelection.value);
        noteAddedHistory.Push(noteToAdd);

        SortNoteList();

        UpdateNoteText();
    }

    /*
    public void AddNote()
    {
        fs.notes_new.Add(new Note_New
        {
            timeToPlay = Convert.ToDouble(input.text),
            isLongPress = false
        });
        text.text = text.text + " " + notes.notes_new[notes.notes_new.Count - 1].timeToPlay.ToString();
    }

    public void AddNote(double time)
    {
        notes.notes_new.Add(new Note_New
        {
            timeToPlay = time,
            isLongPress = false
        });
        text.text = text.text + " " + notes.notes_new[notes.notes_new.Count - 1].timeToPlay.ToString();
    }
    */
}