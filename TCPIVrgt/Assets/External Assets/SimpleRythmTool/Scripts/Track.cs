using System;
using UnityEngine;

[Serializable]
public class Track : PropertyAttribute
{
    public int arrSize = 5;
    public string name;
    public bool[] beats;
    public UnityEngine.Events.UnityEvent OnBeat;
    public bool foldout;

    public Track()
    {
        beats = new bool[arrSize];
    }

#if UNITY_EDITOR
    public void OnChange()
    {
        if (beats == null)
            beats = new bool[arrSize];

        if (arrSize != beats.Length)
            Array.Resize(ref beats, arrSize);
    }
#endif
}