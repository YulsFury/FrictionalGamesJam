using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryController : MonoBehaviour
{
    [Header("Battery Usage")]
    public float timerUseDecrease;
    [Header("Single Time Usage Amounts")]
    public float standardSingleUseDecrease;
    public float scannerSingleUseDecrease;
    [Header("Overtime Usage Amounts")]
    public float standardOvertimeUseDecrease;
    public float doorOvertimeUseDecrease;
    public float radarOvertimeUseDecrease;
    [Header("Discount Multiplyer")]
    public float discountPerUsage = 1;
    public float maxDiscount = 0.1f;
    public int usagesWithoutDiscount = 1;

    public enum singleTimeSources { Standard, Scanner }
    public enum overtimeSources { Standard, Radar, Door}

    private float batterySpent = 0;

    private float maxBatteryLvl = 100;
    private float currentBatteryLvl;

    private IEnumerator decreaseOverTimeCoroutine;

    // Start is called before the first frame update

    void Awake()
    {
        currentBatteryLvl = maxBatteryLvl;
        decreaseOverTimeCoroutine = DecreaseOverTimeCoroutine(); //This is made so we know the exact coroutine that was started when we want to stop it.
    }

    /// <summary>
    /// Reduces de battery ammount a single time.
    /// </summary>
    public void DecreaseSingleTimeBattery(singleTimeSources singleTimeSouceUsed)
    {
        float amount = 0;

        switch (singleTimeSouceUsed)    
        {
            case singleTimeSources.Standard:
                amount = amount + standardSingleUseDecrease;
                break;
            case singleTimeSources.Scanner:
                amount = amount + scannerSingleUseDecrease;
                break;
            default:
                break;
        }

        if(currentBatteryLvl - amount < 0)
        {
            currentBatteryLvl = 0;

            GameManager.GM.GameOver(false);

            StopCoroutine(decreaseOverTimeCoroutine);
        }
        else
        {
            currentBatteryLvl = currentBatteryLvl - amount;
        }

        GameManager.GM.IM.UpdateBatteryLevelContinuous(currentBatteryLvl);
    }

    /// <summary>
    /// Tells the battery that one element has starte using electricity.
    /// </summary>
    public void DecreaseOvertimeBattery(overtimeSources overtimeSourceUsed)
    {
        switch (overtimeSourceUsed)
        {
            case overtimeSources.Standard:
                batterySpent = batterySpent + standardOvertimeUseDecrease;
                break;
            case overtimeSources.Radar:
                batterySpent = batterySpent + radarOvertimeUseDecrease;
                break;
            case overtimeSources.Door:
                batterySpent = batterySpent + doorOvertimeUseDecrease;
                break;
            default:
                break;
        }

        CrossSceneInfo.elementsUsingBattery++;

        GameManager.GM.IM.UpdateBatteryUsage(CrossSceneInfo.elementsUsingBattery);

        if (CrossSceneInfo.elementsUsingBattery == 1)
        {
            StartCoroutine(decreaseOverTimeCoroutine);
            AudioManager.instance.PlayBatteryOvertime();         
        }
    }

    /// <summary>
    /// Tells the battery that one element has stoped using electricity.
    /// </summary>
    public void StopUseOverTimeBattery(overtimeSources overtimeSourceUsed)
    {
        switch (overtimeSourceUsed)
        {
            case overtimeSources.Standard:
                batterySpent = batterySpent - standardOvertimeUseDecrease;
                break;
            case overtimeSources.Radar:
                batterySpent = batterySpent - radarOvertimeUseDecrease;
                break;
            case overtimeSources.Door:
                batterySpent = batterySpent - doorOvertimeUseDecrease;
                break;
            default:
                break;
        }
        CrossSceneInfo.elementsUsingBattery--;

        GameManager.GM.IM.UpdateBatteryUsage(CrossSceneInfo.elementsUsingBattery);

        if (CrossSceneInfo.elementsUsingBattery == 0)
        {
            StopCoroutine(decreaseOverTimeCoroutine);
            AudioManager.instance.StopBatteryOvertime();
        }
    }

    public void HardStopOverTimeBattery()
    {
        CrossSceneInfo.elementsUsingBattery = 0;
        AudioManager.instance.StopBatteryOvertime();
        batterySpent = 0;
        StopCoroutine(decreaseOverTimeCoroutine);
    }
    public void PauseBatteryUsage()
    {
        StopCoroutine(decreaseOverTimeCoroutine);
    }
    public void ContinueBatteryUsage()
    {
        StartCoroutine(decreaseOverTimeCoroutine);
    }

    /// <summary>
    /// Reduces the battery ammount depending on how many elements are using electricity.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DecreaseOverTimeCoroutine()
    {
        while (true)
        {
            if ((currentBatteryLvl - (batterySpent * CalculateDiscount())) < 0)
            {
                currentBatteryLvl = 0;

                GameManager.GM.GameOver(false);
                
                StopCoroutine(decreaseOverTimeCoroutine);
            }
            else if (currentBatteryLvl - (2 * batterySpent * CalculateDiscount()) < 0)
            {
                AudioManager.instance.PlayUIWarning();
                currentBatteryLvl = currentBatteryLvl - (batterySpent * CalculateDiscount());         
            }
            else
            {
                currentBatteryLvl = currentBatteryLvl - (batterySpent * CalculateDiscount());
            }

            GameManager.GM.IM.UpdateBatteryLevelContinuous(currentBatteryLvl);

            yield return new WaitForSeconds(timerUseDecrease);
        }
    }

    private float CalculateDiscount()
    {        
        float discount;
        if(Mathf.Pow(discountPerUsage, CrossSceneInfo.elementsUsingBattery - usagesWithoutDiscount) < maxDiscount)
        {
            discount = maxDiscount;
        }
        else
        {
            discount = Mathf.Pow(discountPerUsage, CrossSceneInfo.elementsUsingBattery - usagesWithoutDiscount);
        }

        return discount;
    }
}
