using System;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] private PowerUpData powerUpData;
    [SerializeField] private int LockedUnitID;
    bool isPowerUpUsed;
    private string powerUpStatusKey = "powerUpStatusKey";
    
    void Start()
    {
        isPowerUpUsed = GetPowerUpStatus();
    }

    void Update()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (powerUpData.powerUpType == PowerUpType.bagBooster && !isPowerUpUsed)
            {
                isPowerUpUsed = true;
                BagController bagController = other.GetComponent<BagController>();
                if (bagController == null)
                    bagController = other.GetComponentInParent<BagController>();
                if (bagController == null)
                    bagController = other.GetComponentInChildren<BagController>();
                if (bagController == null)
                    Debug.LogError("BagController bulunamadı!");
                bagController.BoostBagCapacity(powerUpData.boostCount);
                AudioManager.instance.PlayAudio(AudioClipType.grapClip);
                PlayerPrefs.SetString(powerUpStatusKey, "used");
            }
        }
    }

    private bool GetPowerUpStatus()
    {
        string status = PlayerPrefs.GetString(powerUpStatusKey, "ready");
        if (status.Equals("ready"))
        {
            return false;
        }
        return true;
    }
}
