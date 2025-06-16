using System.Threading;
using UnityEngine;

public class Picto : MonoBehaviour, Trapped
{
    Vector3 originalScale;
    private float timer = 1;

    public bool IsBeingCaptured { get; set; } = false;

    void Awake()
    {
        originalScale = transform.localScale;
    }
    // Update is called once per frame
    public bool CaptureAnim()
    {
        IsBeingCaptured = true;
        transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, timer);
        timer -= Time.deltaTime * 1;
        if (timer <= 0)
        {
            return false;
        }

        return true;
    }

    public int Points()
    {
        return 1;
    }
    void Update()
    {
        if (IsBeingCaptured) return;
        float jiggle = Mathf.Sin(Time.time * 20f) * 0.1f;
        transform.localScale = new Vector3(jiggle, jiggle, jiggle) + originalScale;
    }
}