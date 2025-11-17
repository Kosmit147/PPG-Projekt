using TMPro;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public TextMeshProUGUI targetHitText = null;
    public float messageDuration = 1.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            targetHitText.text = "Target hit!";
            CancelInvoke(nameof(ClearMessage));
            Invoke(nameof(ClearMessage), messageDuration);
        }
    }

    void ClearMessage()
    {
        targetHitText.text = "";
    }
}
