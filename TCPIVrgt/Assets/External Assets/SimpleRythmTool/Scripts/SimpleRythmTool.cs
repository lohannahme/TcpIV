using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRythmTool : MonoBehaviour
{
    public int repeatZeroToLoop = 0;
    public Sheet[] sheets;

    int loopCounter = 0;
    int sheetCounter = 0;
    int beatCounter = 0;
    float runningTimePerBeat;

    int actualSheetIndex = 0;

    public float timePerBeat()
    {
        return 60f / (float)sheets[actualSheetIndex].BPM;
    }

    float timePerBeat(int sheetIndex)
    {
        actualSheetIndex = sheetIndex;
        return 60f / (float)sheets[sheetIndex].BPM;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (sheetCounter < sheets.Length)
        {
            if (beatCounter < sheets[sheetCounter].beatCount)
            {
                runningTimePerBeat += Time.deltaTime;
                float _timePerBeat = timePerBeat(sheetCounter);

                if (runningTimePerBeat >= _timePerBeat)
                {
                    runningTimePerBeat -= _timePerBeat;

                    //check events
                    for (int i = 0; i < sheets[sheetCounter].tracks.Length; i++)// (Track t in tracks)
                    {
                        if (sheets[sheetCounter].tracks[i].beats[beatCounter])//(tracks[j].beats[i])
                            if (sheets[sheetCounter].tracks[i].OnBeat != null)
                                sheets[sheetCounter].tracks[i].OnBeat.Invoke();
                    }

                    beatCounter++;
                }
            }
            else
            {
                sheetCounter++;
                beatCounter = 0;
            }
        }
        else
        {
            if (repeatZeroToLoop > 0)
            {
                if (loopCounter < repeatZeroToLoop)
                {
                    loopCounter++;
                    sheetCounter = 0;
                }
            }
            else
                sheetCounter = 0;
        }
    }
}
