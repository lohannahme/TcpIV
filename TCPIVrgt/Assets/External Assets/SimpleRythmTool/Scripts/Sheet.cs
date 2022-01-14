using System;
using System.Collections;
using UnityEngine;

public class Sheet : MonoBehaviour
{
    public int BPM;
    public int trackCount;
    public int beatCount;

    public Track[] tracks;
    
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
}
