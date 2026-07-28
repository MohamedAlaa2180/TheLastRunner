using TMPro;
using UnityEngine;

public class FrameRateDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text fpsText;
    [SerializeField] float updateInterval = 0.25f;

    float timer;
    int frameCount;

    void Awake()
    {
        if (fpsText == null)
            fpsText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        frameCount++;
        timer += Time.unscaledDeltaTime;

        if (timer < updateInterval)
            return;

        fpsText.text = Mathf.RoundToInt(frameCount / timer).ToString();
        frameCount = 0;
        timer = 0f;
    }
}
