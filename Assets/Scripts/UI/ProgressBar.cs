using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

    public Image currProgressBar;

    private float currProgress;
    private float endPoint;
    private float ratio;
    private float incrementStep;
    private float duration;

    private bool paused;
    private bool ended;

    // Initialization
    void Start () {
        currProgress = 0;
        ended = false;
        RectTransform rt = (RectTransform)gameObject.transform;
        endPoint = rt.rect.width;
        incrementStep = 0;
	}
	
	// Update is called once per frame
	void Update () {

        incrementStep = endPoint / 100 / duration;

        if (!paused && !ended)
        {

            if (currProgress + incrementStep > endPoint)
            {
                currProgress = endPoint;
                ratio = 1.0f;
                ended = true;
            }
            else
            {
                currProgress += incrementStep;

                ratio = currProgress / endPoint;
                currProgressBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
            }
        }
	}

    public void TogglePauseGame()
    {
        paused = !paused;
    }

    public float getCurrPercentage()
    {
        return ratio;
    }

    public void setDuration(int durationInMilliSeconds)
    {
        this.duration = durationInMilliSeconds;
        paused = false;
    }
}
