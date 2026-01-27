using UnityEngine;
using UnityEngine.UI;

public class NPCUI : MonoBehaviour
{
    public Health NPCHealth;
    public GameObject cameraObject;
    public Slider healthSlider;

    void Start()
    {
        healthSlider.value = NPCHealth.value;
    }

    void LateUpdate()
    {
        if (cameraObject == null)
        {
            Destroy(gameObject);
            return;
        }

        var eulerAngles = transform.eulerAngles;
        transform.eulerAngles = new Vector3(eulerAngles.x, cameraObject.transform.eulerAngles.y, eulerAngles.z);

        healthSlider.value = NPCHealth.value;
    }
}
