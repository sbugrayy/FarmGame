using System.Collections;
using UnityEngine;

public class ProductPlantController : MonoBehaviour
{
    private bool isReadyToPick;
    private Vector3 originalScale;
    [SerializeField] private ProductData productData;
    private BagController bagController;
    void Start()
    {
        isReadyToPick = true;
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isReadyToPick)
        {
            bagController = other.GetComponent<BagController>();

            if (bagController.IsEmptySpace())
            {
                AudioManager.instance.PlayAudio(AudioClipType.grapClip);
                bagController.AddProductToBag(productData);
                //Debug.Log("Fideye dokunuldu");
                isReadyToPick = false;
                StartCoroutine(ProductsPicked());
            }
        }
    }

    // LeanTween ile animasyon kodu
    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator ProductsPicked()
    {
        Vector3 targetScale = originalScale / 3;
        transform.gameObject.LeanScale(targetScale, 1f);

        yield return new WaitForSeconds(5f);

        transform.gameObject.LeanScale(originalScale, 1f).setEase(LeanTweenType.easeOutElastic);
        isReadyToPick = true;
    }

    // Elle yaz�lan animasyon kodu
    // e�ya al�nd�ktan sonra k���lt ve yava�ca b�y�s�n
    /*
    IEnumerator ProductPicked()
    {
        float duration = 1f;
        float timer = 0;

        Vector3 targetScale = originalScale / 3;

        while (timer < duration)
        {
            float t = timer / duration;
            Vector3 newScale = Vector3.Lerp(originalScale, targetScale, t);
            transform.localScale = newScale;
            timer += Time.deltaTime;
            yield return null;
        }
        // art�k fide k���ld�
        yield return new WaitForSeconds(5f);

        timer = 0f;
        float growBackDuration = 1f;
        while (timer < growBackDuration)
        {
            float t = timer / growBackDuration;
            Vector3 newScale = Vector3.Lerp(targetScale, originalScale, t);
            transform.localScale = newScale;
            timer += Time.deltaTime;
            yield return null;
        }

        isReadyToPick = true;
        yield return null;
    }
    */
}
