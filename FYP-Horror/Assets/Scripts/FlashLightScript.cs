using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLightScript : MonoBehaviour
{
    private Light flashLight;
    private float flashLightTimer;
    public float maxFlashlightTimer;
    public float flashlightDecreaseAmount;
    public float flashlightIncreaseAmount;
    private bool isBatteryDepleted;
    public float depletedBatteryIncreaseAmount;
    private const float MAX_RGB_VALUE = 1;
    private float redRGBValue;

    [SerializeField] private Image flashlightUI;
    // Start is called before the first frame update
    void Start()
    {
        flashLight = GetComponent<Light>();
        flashLightTimer = maxFlashlightTimer;
        isBatteryDepleted = false;
        redRGBValue = MAX_RGB_VALUE;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBatteryDepleted == false)
        {
            if (Input.GetKey("f") && flashLightTimer >= 0)
            {
                flashlightOn();
                checkIsBatteryDepleted();
            }
            else
            {
                flashlightOff(flashlightIncreaseAmount);
            }
        }
        else
        {
            flashlightOff(depletedBatteryIncreaseAmount);
        }

        flashlightUI.fillAmount = flashLightTimer / maxFlashlightTimer;

        setFlashlightMeterColor();
    }

    void flashlightOn()
    {
        flashLight.enabled = true;
        flashLightTimer -= flashlightDecreaseAmount;
    }

    void flashlightOff(float _batteryIncrease)
    {
        flashLight.enabled = false;
        if (flashLightTimer <= maxFlashlightTimer)
        {
            flashLightTimer += _batteryIncrease;
        }
        else if (flashLightTimer >= maxFlashlightTimer)
        {
            isBatteryDepleted = false;
        }
    }

    void checkIsBatteryDepleted()
    {
        if (flashLightTimer <= 0)
        {
            isBatteryDepleted = true;
        }
    }

    void setFlashlightMeterColor()
    {
        redRGBValue = flashLightTimer / maxFlashlightTimer;
        redRGBValue = MAX_RGB_VALUE * redRGBValue;
        flashlightUI.color = new UnityEngine.Color(MAX_RGB_VALUE, redRGBValue, redRGBValue);
    }
}
