using UnityEngine;
using UnityEngine.InputSystem;

public class GunSwitcher : MonoBehaviour
{
    public InputActionProperty selectGun1Action; // Expects a button.
    public InputActionProperty selectGun2Action; // Expects a button.

    public GameObject gun1 = null;
    public GameObject gun2 = null;

    public GameObject currentGun = null;

    void Update()
    {
        if (selectGun1Action.action.WasPerformedThisFrame())
            selectGun1();

        if (selectGun2Action.action.WasPerformedThisFrame())
            selectGun2();
    }

    void selectGun1()
    {
        if (gun1 == null)
            return;

        gun1.SetActive(true);
        currentGun = gun1;

        if (gun2 != null)
            gun2.SetActive(false);
    }

    void selectGun2()
    {
        if (gun2 == null)
            return;

        gun2.SetActive(true);
        currentGun = gun2;

        if (gun1 != null)
            gun1.SetActive(false);
    }
}
