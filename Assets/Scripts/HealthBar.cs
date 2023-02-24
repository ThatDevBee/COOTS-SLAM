using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image heartImage;
    private float startHeartX;
    private float startHeartY;
    public float finalHeartX;
    public float finalHeartY;
    public float waitTime;

    void Start() {
        startHeartX = heartImage.rectTransform.localScale.x;
        startHeartY = heartImage.rectTransform.localScale.y;
    }

    public void SetMaxHealth(float health) {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(float health) {
        slider.value = health;
    }

    public void DisplayHealthChange() {
        StartCoroutine(HealthChange());
    }

    IEnumerator HealthChange() {
        heartImage.rectTransform.localScale = new Vector3(startHeartX, startHeartY, 1);
        yield return new WaitForSeconds(waitTime);
        heartImage.rectTransform.localScale = new Vector3(finalHeartX, finalHeartY, 1);
        yield return new WaitForSeconds(waitTime);
        heartImage.rectTransform.localScale = new Vector3(startHeartX, startHeartY, 1);
    }
}
