using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryController : MonoBehaviour
{
    [Header("Battery")]
    private float maxBatteryLvl = 100;
    private float currentBatteryLvl;
    public float standarSingleUseDecrease;
    public float standarOvertimeUseDecrease;
    public float TimerUseDecrease;

    // Start is called before the first frame update
    void Start()
    {
        currentBatteryLvl = maxBatteryLvl;
    }

    public void DecreaseSingleTimeBattery()
    {
        if(currentBatteryLvl - standarSingleUseDecrease < 0)
        {
            currentBatteryLvl = 0;
        }
        else
        {
            currentBatteryLvl = currentBatteryLvl - standarSingleUseDecrease;
        }

        GameManager.GM.IM.UpdateBattery(currentBatteryLvl);
    }

    public void DecreaseOvertimeBattery()
    {
        StartCoroutine(DecreaseOverTimeCoroutine());
    }

    public void StopUseOverTimeBattery()
    {
        StopCoroutine(DecreaseOverTimeCoroutine());
    }

    IEnumerator DecreaseOverTimeCoroutine()
    {
        if (currentBatteryLvl - standarSingleUseDecrease < 0)
        {
            currentBatteryLvl = 0;
        }
        else
        {
            currentBatteryLvl = currentBatteryLvl - standarSingleUseDecrease;
        }

        GameManager.GM.IM.UpdateBattery(currentBatteryLvl);

        yield return new WaitForSeconds(TimerUseDecrease);
    }
}
