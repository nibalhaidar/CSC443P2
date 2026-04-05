using System.Collections;
using UnityEngine;

public class SlowMo : MonoBehaviour
{
    public static SlowMo Instance { get; private set; }

    [SerializeField] private float slowTimeScale = 0.2f;  // how slow it gets
    [SerializeField] private float slowDuration = 1.5f;   // how long it lasts
    [SerializeField] private float resumeSpeed = 2f;  
        // how fast it returns to normal

    private void Awake()
    {
        Instance = this;
    }

    public void TriggerSlowMo()
{
    Debug.Log("SlowMo triggered, setting timeScale to: " + slowTimeScale);
    StartCoroutine(DoSlowMo());
}

    private IEnumerator DoSlowMo()
{
    Time.timeScale = slowTimeScale;
    Time.fixedDeltaTime = 0.02f * slowTimeScale;

    float elapsed = 0f;
    while (elapsed < slowDuration)
    {
        elapsed += Time.unscaledDeltaTime;
        yield return null;
    }

    // gradually return to normal
    while (Time.timeScale < 1f)
    {
        Time.timeScale += Time.unscaledDeltaTime * resumeSpeed;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        yield return null;
    }

    Time.timeScale = 1f;
    Time.fixedDeltaTime = 0.02f;
}
}