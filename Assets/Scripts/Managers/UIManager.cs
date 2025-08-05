using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField]  private TextMeshProUGUI coinCountText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCoinCountOnScreen(int coins)
    {
        coinCountText.text = coins.ToString();
    }
}
