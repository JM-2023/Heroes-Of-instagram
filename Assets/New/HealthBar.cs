using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    private float targetFillAmount;

    void Update()
    {
        // Smoothly interpolate the fill amount
        if (fillImage.fillAmount != targetFillAmount)
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetFillAmount, Time.deltaTime * 10f);
        }
    }

    public void SetHealth(float currentHealth, float maxHealth)
    {
        targetFillAmount = currentHealth / maxHealth;
    }
}
