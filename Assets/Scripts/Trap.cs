using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Trap : MonoBehaviour
{
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Trapped>(out Trapped critters))
        {
            if (critters.IsBeingCaptured) return;
            HighscoreManager.instance?.IncreaseScore(critters.Points());
            StartCoroutine(Capture(critters, other.gameObject));
        }
    }
    IEnumerator Capture(Trapped critters, GameObject go)
    {
        bool IsAnimationPlaying = true;
        while(IsAnimationPlaying)
        {
            rb.isKinematic = true;
            transform.rotation = Quaternion.AngleAxis(Time.deltaTime, Vector3.up);

            IsAnimationPlaying = critters.CaptureAnim();
            yield return null;
        }

        Destroy(go);
    }
}
