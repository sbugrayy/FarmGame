using System.Collections;
using UnityEngine;
using TMPro;
public class LockedUnitController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int price;
    [SerializeField] private int ID;
    
    [Header("Objects")]
    [SerializeField] private TextMeshPro priceText;
    [SerializeField] private GameObject lockedUnit;
    [SerializeField] private GameObject unlockedUnit;
    private bool isPurchased;
    private string keyUnit = "KeyUnit";
    private Vector3 lockedOriginalScale;
    private Vector3 unlockedOriginalScale;
    void Start()
    {
        priceText.text = price.ToString();
        LoadUnit();
        
        // locked objenin scale'ını kaydet
        if (lockedUnit != null)
            lockedOriginalScale = lockedUnit.transform.localScale;
        if (unlockedUnit != null)
            unlockedOriginalScale = unlockedUnit.transform.localScale;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPurchased)
        {
            UnlockUnit();
        }
    }

    private void UnlockUnit()
    {
        if (CashManager.instance.TryBuyThisUnit(price))
        {
            AudioManager.instance.PlayAudio(AudioClipType.shopClip);
            StartCoroutine(UnlockAnimation());
            SaveUnit();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator UnlockAnimation()
    {
        isPurchased = true;

        if (lockedUnit != null)
        {
            lockedUnit.transform.LeanScale(Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack);
            yield return new WaitForSeconds(0.5f);
            lockedUnit.SetActive(false);
        }

        if (unlockedUnit != null)
        {
            unlockedUnit.SetActive(true);
            unlockedUnit.transform.localScale = Vector3.zero;
            unlockedUnit.transform.LeanScale(unlockedOriginalScale, 0.5f).setEase(LeanTweenType.easeInOutBack);
        }
    }
    
    private void SaveUnit()
    {
        string key = keyUnit + ID.ToString();
        PlayerPrefs.SetString(key, "saved");
    }
    
    private void LoadUnit()
    {
        string key = keyUnit + ID.ToString();
        string status = PlayerPrefs.GetString(key);

        if (status.Equals("saved"))
        {
            StartCoroutine(UnlockAnimation());
        }
    }
}
