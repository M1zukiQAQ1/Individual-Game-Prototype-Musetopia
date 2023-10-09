using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreGeneratorWithBars : MonoBehaviour
{
    public List<Note_New>[] bars = new List<Note_New>[200];

    [Header("Bar Generation")]
    public TMP_Dropdown barSelection;
    public TMP_Dropdown beatSelection;
    public Button initializeBtn;
    public TMP_InputField tempo;

    [Header("Beat Generation")]
    public TMP_InputField beatDivisionField;
    public TMP_Text noteReferenceText;

    [Header("Misc")]
    public TMP_Text musicTime;
    public AudioSource music;

    public Button testBarTimeBtn;
    public TMP_InputField barDivisionField;

    public Image barIndicator;

    private List<double> barTimeInMusic = new();
    private List<double> beatTimeInBar = new();
    private bool isPlayingFromBar = false;
    private int barTimeIndex;
    private int barDivision;

    public void Initialize()
    {
        barTimeInMusic.Clear();
        beatTimeInBar.Clear();

        double time = 0;
        while (time <= music.clip.length)
        {
            barTimeInMusic.Add(time);
            time += (60 / Convert.ToDouble(tempo.text)) * (Convert.ToDouble(barDivisionField.text));
        }

        barDivision = Convert.ToInt32(barDivisionField);
        UpdateBarSelectionDropdown();
    }

    public void ShiftBarTimeForward()
    {
        for(int i = 0; i < barTimeInMusic.Count; i++)
        {
            barTimeInMusic[i] += (60 / Convert.ToDouble(tempo.text)) / 4;
        }
        UpdateBarSelectionDropdown();
    }

    public void PlayFromBar()
    {
        music.time = (float)barTimeInMusic[barSelection.value] - 1f;
        barTimeIndex = barSelection.value;
        music.Play();
        Debug.Log("Playing from " + music.time);
        isPlayingFromBar = true;
    }

    public void AddNote()
    {
        ScoreGenerationSceneController.sgsc.AddNote(beatTimeInBar[beatSelection.value]);
    }
    public void DivideBeatInBar()
    {
        beatSelection.options.Clear();
        beatTimeInBar.Clear();

        double timeIntervalPerBeat = (barTimeInMusic[barSelection.value + 1] - barTimeInMusic[barSelection.value]) / Convert.ToDouble(beatDivisionField.text);
        for(int i = 0; i < Convert.ToInt32(beatDivisionField.text); i++)
        {
            beatTimeInBar.Add(timeIntervalPerBeat * i + barTimeInMusic[barSelection.value]);
            beatSelection.options.Add(new TMP_Dropdown.OptionData((timeIntervalPerBeat * i + barTimeInMusic[barSelection.value]).ToString("0.00")));
        }
    }

    public void TestBarTime()
    {
        music.Play();
        barTimeIndex = 0;
    }

    public void StopTestingBarTime()
    {
        music.Stop();
        music.time = 0f;
        barTimeIndex = 0;
    }

    private IEnumerator BlinkIndicator()
    {
        barIndicator.color = Color.red;
        yield return new WaitForSeconds(0.07f);
        barIndicator.color = Color.white;
    }

    private void UpdateBarSelectionDropdown()
    {
        barSelection.options.Clear();
        foreach (double noteTime in barTimeInMusic)
        {
            barSelection.options.Add(new TMP_Dropdown.OptionData(noteTime.ToString("0.00")));
        }
    }

    public void ClearNoteReferenceText()
    {
        noteReferenceText.text = "Press Space to Get Reference";
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(noteReferenceText.text.Equals("Press Space to Get Reference"))
            {
                noteReferenceText.text = "";
            }

            noteReferenceText.text += music.time.ToString("0.000") + " ";
        }

        if(barTimeInMusic.Count != 0)
        {
            if (music.time > barTimeInMusic[barTimeIndex])
            {
                barTimeIndex++;
                StartCoroutine(nameof(BlinkIndicator));
            }
        }

        if (isPlayingFromBar)
        {
            if(music.time > (float)barTimeInMusic[barSelection.value + 1] + 1f)
            {
                Debug.Log("Music Stopped!");
                music.Stop();
                isPlayingFromBar = false;
            }
        }

        musicTime.text = music.time.ToString();
    }
}
