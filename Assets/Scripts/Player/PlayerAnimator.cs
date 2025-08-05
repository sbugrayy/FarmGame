using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator playerAC;
    void Start()
    {
        
    }

    void Update()
    {

    }

    public void ManageAnimations(Vector3 move)
    {
        if (move.magnitude > 0)
        {
            PlayRunAnimation();

            playerAC.transform.forward = move.normalized; // Karakterin y�n�n� hareket y�n�ne g�re ayarl�yoruz
        }
        else
        {
            PlayIdleAnimation();
        }
    }

    private void PlayRunAnimation()
    {
        playerAC.Play("RUN");
    }

    private void PlayIdleAnimation()
    {
        playerAC.Play("IDLE");
    }
}
