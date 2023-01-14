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

    // Start is called before the first frame update
    void Start()
    {
        currentBatteryLvl = maxBatteryLvl;
    }

    public void DecreaseSingleTimeBattery(float _decreaseAmount)
    {
        currentBatteryLvl = currentBatteryLvl - _decreaseAmount;

        //TODO: Update Interface.
    }

    public IEnumerator DecreaseOvertimeBattery(float _decreaseAmount)
    {
        yield return 0;

        //TODO: Update Interface
    }
}
