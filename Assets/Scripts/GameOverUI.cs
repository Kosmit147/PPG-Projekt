using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public InputActionProperty restartGameAction; // Expects a button.

    void Update()
    {
        if (restartGameAction.action.WasPerformedThisFrame())
            SceneManager.LoadScene("MainScene");
    }
}
