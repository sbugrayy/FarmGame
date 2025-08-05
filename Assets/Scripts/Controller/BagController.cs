using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BagController : MonoBehaviour
{
    [SerializeField] private Transform bag; // çanta objesinin referansı
    public List<ProductData> productDataList; // çantadaki ürünlerin listesi
    private Vector3 productSize; // ürünlerin boyutu
    [SerializeField] TextMeshPro maxText; // Maksimum ürün sayısını gösteren metin
    int maxBagCapacity; 
    private string bagCapacityKey = "bagCapacityKey";
    void Start()
    {
        maxBagCapacity = LoadBagCapacity(); // çantanın maksimum kapasitesi
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShopPoint"))
        {
            PlayShopSound();
            for (int i = productDataList.Count - 1; i >= 0; i--)
            {
                SellProductsToShop(productDataList[i]);
                Destroy(bag.transform.GetChild(i).gameObject); // Çantadaki tüm ürünleri sil
                productDataList.RemoveAt(i); // Ürün listesinden sil
            }

            ControlBagCapacity(); // çantanın kapasitesini kontrol et
        }

        if (other.CompareTag("UnlockBakeryUnit")) 
        {
            UnlockBakeryUnitController bakeryUnit = other.GetComponent<UnlockBakeryUnitController>();
            
            ProductType neededType = bakeryUnit.GetNeededProductType();

            for (int i = productDataList.Count - 1; i >= 0; i--)
            {
                if (productDataList[i].productType == neededType) // Çantada en yukarıdaki ürün beklenen ürün mü?
                {
                    if (bakeryUnit.StoreProduct() == true) // Pastanede bu ürünü saklayacak yer var mı?
                    {
                        Destroy(bag.transform.GetChild(i).gameObject); 
                        productDataList.RemoveAt(i);
                    }
                }
            }
            
            StartCoroutine(PutProductsInOrder()); // Üstte kalan kasaları aşağıya indir
            ControlBagCapacity(); // max yazısını güncelle
        }
    }

    private void SellProductsToShop(ProductData productData)
    {
        // cashManager'a söyle ürün satıldı.
        CashManager.instance.ExchangeProduct(productData);
    }

    public void AddProductToBag(ProductData productData)
    {
        GameObject boxProduct = Instantiate(productData.productPrefab, Vector3.zero, Quaternion.identity); // ürünü sahneye ekle
        boxProduct.transform.SetParent(bag, true);

        CalculateObjectSize(boxProduct); // ürünün boyutunu hesapla
        float yPos = CalculateNewYPositionOfBox(); // y konumunu hesapla
        boxProduct.transform.localRotation = Quaternion.identity;
        boxProduct.transform.localPosition = Vector3.zero; // ürünü çantanın içine yerleştir
        boxProduct.transform.localPosition = new Vector3(0, yPos, 0);
        productDataList.Add(productData);
        ControlBagCapacity(); // çantanın kapasitesini kontrol et
    }

    private float CalculateNewYPositionOfBox()
    {
        // ürünün sahnede yüksekliği * ürün sayısı
        float newYPos = productSize.y * productDataList.Count;
        return newYPos;
    }

    private void CalculateObjectSize(GameObject gameObject)
    {
        if (productSize == Vector3.zero)
        {
            MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
            productSize = meshRenderer.bounds.size;
        }
    }

    private void ControlBagCapacity()
    {
        if (productDataList.Count == maxBagCapacity)
        {
            // MAX yazısını göster ve daha fazla ürün almasını engelle
            SetMaxTextOn();
        }
        else
        {
            SetMaxTextOff();
        }
    }

    private void SetMaxTextOn()
    {
        if (!maxText.isActiveAndEnabled)
        {
            maxText.gameObject.SetActive(true);
        }
        
    }
    private void SetMaxTextOff()
    {
        if (maxText.isActiveAndEnabled)
        {
            maxText.gameObject.SetActive(false);
        }
    }

    public bool IsEmptySpace()
    {
        if (productDataList.Count < maxBagCapacity)
        {
            return true;
        }
        return false;
    }

    private IEnumerator PutProductsInOrder() // Ürünler çantadan eksildiğinde ürünleri aşağıya indirir
    {
        yield return new WaitForSeconds(0.15f);
        for (int i = 0; i < bag.childCount; i++)
        {
            float newYPosition = productSize.y * i;
            bag.GetChild(i).localPosition = new Vector3(0, newYPosition, 0);
        }
    }

    private void PlayShopSound() // Ürün satıldığında çıkacak ses
    {
        if (productDataList.Count > 0)
        {
            AudioManager.instance.PlayAudio(AudioClipType.shopClip);
        }
    }

    public void BoostBagCapacity(int boostCount) // Çanta kapasitesini artıran boost fonksiyonu - bkz. PowerUpController()
    {
        maxBagCapacity += boostCount;
        PlayerPrefs.SetInt(bagCapacityKey, maxBagCapacity); // Çanta kapasitesi hafızada tutuluyor
        ControlBagCapacity(); // Max yazısı kontrolü 
    }

    private int LoadBagCapacity()
    {
        int maxBag = PlayerPrefs.GetInt(bagCapacityKey, 5);
        return maxBag;
    }
}
