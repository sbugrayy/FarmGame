using Unity.Hierarchy;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public RectTransform joystickOutline;
    public RectTransform joystickButton;
    public float moveFactor;
    private Vector3 move;

    private bool canControlJoystick;
    private Vector3 tapPosition;
    void Start()
    {
        HideJoystick();
    }
    public void TappedOnJoystickZone()
    {
        tapPosition = Input.mousePosition;
        joystickOutline.position = tapPosition;
        // ekrana dokunulduðu algýlanacak ve joystick ekranda gözükecek
        ShowJoystick();
    }
    private void ShowJoystick()
    {
        joystickOutline.gameObject.SetActive(true);
        canControlJoystick = true;
    }
    private void HideJoystick()
    {
        joystickOutline.gameObject.SetActive(false);
        canControlJoystick = false;
        move = Vector3.zero;
    }
    public void ControlJoystick()
    {
        Vector3 currentPosition = Input.mousePosition;
        Vector3 direction = currentPosition - tapPosition; // YÖN

        float canvasYScale = GetComponentInParent<Canvas>().GetComponent<RectTransform>().localScale.y;
        float moveMagnitude = direction.magnitude * moveFactor * canvasYScale;
        moveMagnitude = Mathf.Min(moveMagnitude, joystickOutline.rect.width / 2 * canvasYScale); // Kullanýlan cihazýn çözünürlüðüne göre hareketi ölçeklendiriyoruz

        //float moveMagnitude = direction.magnitude * moveFactor / Screen.width;
        //moveMagnitude = Mathf.Min(moveMagnitude, joystickOutline.rect.width / 2);

        move = direction.normalized * moveMagnitude;

        Vector3 targetPos = tapPosition + move;
        joystickButton.position = targetPos;

        // joystick ile karakter kontrol edilecek

        if (Input.GetMouseButtonUp(0))
        {
            HideJoystick();
        }
    }

    public Vector3 GetMovePosition()
    {
        return move / 1.75f;
    }

    void Update()
    {
        if (canControlJoystick)
        {
            ControlJoystick();
        }
    }
}
