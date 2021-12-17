using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Sheet : MonoBehaviour
{
    public int BPM;
    public int trackCount;
    public int beatCount;

    public Track[] tracks;

    public void SetBPM(int value)
    {
        if (value == BPM) return;
        Undo.RecordObject(this, "Change BPM");
        BPM = value;
        EditorUtility.SetDirty(this);
    }
    public void SetTrackCount(int value)
    {
        if (value == trackCount) return;
        Undo.RecordObject(this, "Change trackCount");
        trackCount = value;
        EditorUtility.SetDirty(this);
        /*foreach (Track t in tracks)
            t.arrSize = trackCount;*/
    }
    public void SetBeatCount(int value)
    {
        if (value == beatCount) return;
        Undo.RecordObject(this, "Change beatCount");
        beatCount = value;
        EditorUtility.SetDirty(this);
    }

#if UNITY_EDITOR
    public void Validate()
    {
        if (tracks == null)
            tracks = new Track[trackCount];

        if (trackCount != tracks.Length)
            System.Array.Resize(ref tracks, trackCount);

        foreach (Track t in tracks)
        {
            if (t.arrSize != beatCount)
            {
                t.arrSize = beatCount;
                t.OnChange();
            }
        }
    }
#endif

    /*private void OnValidate()
    {
        if (trackCount != tracks.Length)
        {
            Array.Resize(ref tracks, trackCount);
        }
    }*/
}
