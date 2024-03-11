using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class TextInputElement
{
    public string keyword;
    public UnityEvent<string> onKeywordMatch;
}

public class TextInputSystem : MonoBehaviour
{
    public TextInputElement[] elements;
    public UnityEvent<string> onNoMatch;

    public InputField inputField; // Make the InputField public
    public Canvas menuCanvas; // Reference to the Canvas component

    private bool menuOpen = false;
    private bool canCallEvent = true;

    void Start()
    {
        // Hide UI and lock cursor at the start
        HideUI();
        LockCursor();
        Debug.Log("Game started. UI is hidden, cursor is locked.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleMenu();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            CheckAndCallEventOnce();
        }
    }

    void ToggleMenu()
    {
        menuOpen = !menuOpen;

        if (menuOpen)
        {
            ShowUI();
            UnlockCursor();
            Debug.Log("Menu opened. UI shown, cursor unlocked.");
            CheckInput(); // Call CheckInput when the menu opens
        }
        else
        {
            HideUI();
            LockCursor();
            Debug.Log("Menu closed. UI hidden, cursor locked.");
        }

        Debug.Log("Menu is now " + (menuOpen ? "open" : "closed"));
    }

    void ShowUI()
    {
        if (menuCanvas != null)
        {
            menuCanvas.enabled = true;
            Debug.Log("UI shown.");
        }
        else
        {
            Debug.LogError("menuCanvas not set. Please assign a Canvas through the Unity Editor.");
        }
    }

    void HideUI()
    {
        if (menuCanvas != null)
        {
            menuCanvas.enabled = false;
            Debug.Log("UI hidden.");
        }
        else
        {
            Debug.LogError("menuCanvas not set. Please assign a Canvas through the Unity Editor.");
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Debug.Log("Cursor locked.");
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Cursor unlocked.");
    }

    void CheckInput()
    {
        if (inputField != null)
        {
            string userInput = inputField.text;
            Debug.Log("Checking input: " + userInput);
            bool matchFound = false;

            foreach (var element in elements)
            {
                if (userInput.ToLower() == element.keyword.ToLower())
                {
                    element.onKeywordMatch.Invoke(userInput);
                    matchFound = true;
                    Debug.Log("Match found for keyword: " + element.keyword);
                    break;
                }
            }

            if (!matchFound)
            {
                onNoMatch.Invoke(userInput);
                Debug.Log("No match found for input: " + userInput);
            }
        }
        else
        {
            Debug.LogError("InputField not set. Please assign an InputField through the Unity Editor.");
        }
    }

    void CheckAndCallEventOnce()
    {
        if (canCallEvent)
        {
            CheckInput(); // Call the event checking method
            StartCoroutine(DelayEventCall());
        }
    }

    IEnumerator DelayEventCall()
    {
        canCallEvent = false;
        yield return new WaitForSeconds(1f);
        canCallEvent = true;
    }
}
