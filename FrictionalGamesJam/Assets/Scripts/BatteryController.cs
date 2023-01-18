using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryController : MonoBehaviour
{
    [Header("Battery Usage")]
    public float standarSingleUseDecrease;
    public float standarOvertimeUseDecrease;
    public float timerUseDecrease;

    private int elementsUsingBattery = 0;

    private float maxBatteryLvl = 100;
    private float currentBatteryLvl;

    private IEnumerator decreaseOverTimeCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        currentBatteryLvl = maxBatteryLvl;
        decreaseOverTimeCoroutine = DecreaseOverTimeCoroutine(); //This is made so we know the exact coroutine that was started when we want to stop it.
    }

    /// <summary>
    /// Reduces de battery ammount a single time.
    /// </summary>
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

    /// <summary>
    /// Tells the battery that one element has starte using electricity.
    /// </summary>
    public void DecreaseOvertimeBattery()
    {
        elementsUsingBattery++;
        if (elementsUsingBattery == 1)
        {
            StartCoroutine(decreaseOverTimeCoroutine);
        }

        //TODO: Update interface to show how many elements are using battery currently.
    }

    /// <summary>
    /// Tells the battery that one element has stoped using electricity.
    /// </summary>
    public void StopUseOverTimeBattery()
    {
        elementsUsingBattery--;

        if (elementsUsingBattery == 0)
        {
            StopCoroutine(decreaseOverTimeCoroutine);
        }

        //TODO: Update interface to show how many elements are using battery currently.
    }

    /// <summary>
    /// Reduces the battery ammount depending on how many elements are using electricity.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DecreaseOverTimeCoroutine()
    {
        while (true)
        {
            if (currentBatteryLvl - (standarOvertimeUseDecrease * elementsUsingBattery) < 0)
            {
                currentBatteryLvl = 0;

                GameManager.GM.GameOver();
                
                StopCoroutine(decreaseOverTimeCoroutine);
            }
            else
            {
                currentBatteryLvl = currentBatteryLvl - (standarOvertimeUseDecrease * elementsUsingBattery);
            }

            GameManager.GM.IM.UpdateBattery(currentBatteryLvl);

            yield return new WaitForSeconds(timerUseDecrease);
        }
    }
}
