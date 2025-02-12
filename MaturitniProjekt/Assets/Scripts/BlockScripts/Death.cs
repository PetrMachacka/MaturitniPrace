using UnityEngine;

public class Death : MonoBehaviour
{
    [SerializeField] private GameObject deathUI;

    private void OnTriggerEnter(Collider other)
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (deathUI != null)
        {
            deathUI.SetActive(true);
        }
    }
}