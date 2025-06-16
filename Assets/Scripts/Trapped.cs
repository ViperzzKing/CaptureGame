using UnityEngine;

public interface Trapped
{

    public bool IsBeingCaptured { get; set; }

    public bool CaptureAnim();

    public int Points();
}
