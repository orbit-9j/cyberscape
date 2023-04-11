using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : HealthBar
{
    //-----
    public float totalTime;
    public float remainingTime;
    public bool timerRunning;
    private bool isCountingDown = false;

    public void StartTimer()
    {
        remainingTime = totalTime;
        SetTotalTime(totalTime);
        SetTime(remainingTime);
        //CountDown();

        if (!isCountingDown)
        {
            StartCoroutine(CountDown());
        }
    }
    //----

    public void SetTime(float time)
    {
        slider.value = time;
        //----
        UpdateText(time);
        //----
    }

    public void SetTotalTime(float time)
    {
        slider.maxValue = time;
        slider.value = time;
        //----
        UpdateText(time);
        //-----
    }

    //-----
    private void UpdateText(float time)
    {
        sliderTextbox.text = sliderText + ": " + time.ToString("0") + "/" + slider.maxValue;
        //Debug.Log(sliderText + ": " + time.ToString("0") + "/" + slider.maxValue);
    }

    private IEnumerator CountDown() //timer is counting down too fast even with delta time
    {
        isCountingDown = true;
        while (remainingTime > 0 && timerRunning)
        {
            yield return new WaitForSeconds(1.0f);
            remainingTime -= 1.0f;
            SetTime(remainingTime);
        }
        remainingTime = 0;
        timerRunning = false;
        isCountingDown = false;

    }
    /* 
        private void CountDown() //timer is counting down too fast even with delta time
        {
            remainingTime -= 1 * Time.deltaTime;
            SetTime(remainingTime);

            if (remainingTime < 0 || timerRunning == false)
            {
                remainingTime = 0;
                timerRunning = false;
            }
            else
            {
                CountDown();
            }
} */
    //-----
}
