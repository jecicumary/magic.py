using UnityEngine;
using UnityEngine.UI;

public class TextInput : MonoBehaviour
{
    public InputField inputField; // Drag and drop your InputField object into this field in the Inspector
    public Text displayText; // Drag and drop your Text object into this field in the Inspector
    public float proximityDistance = 5f; // Adjust the proximity distance as needed
    public float inputCooldown = 5f; // Adjust the cooldown time as needed

    private GameObject player;
    private float lastInputTime = 0f;
    private bool isInInputMenu = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInputMenu();
        }

        if (isInInputMenu && IsPlayerNearby())
        {
            // Check for user input
            if (Input.GetKeyDown(KeyCode.Return) && CanOpenInputMenu())
            {
                // Handle text input when the Enter key is pressed
                HandleTextInput();
                lastInputTime = Time.time;
            }
        }
    }

    void ToggleInputMenu()
    {
        isInInputMenu = !isInInputMenu;

        if (!isInInputMenu)
        {
            inputField.text = ""; // Clear the input field when exiting
        }
    }

    bool IsPlayerNearby()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            return distance <= proximityDistance;
        }
        return false;
    }

    bool CanOpenInputMenu()
    {
        return Time.time - lastInputTime >= inputCooldown;
    }

    void HandleTextInput()
    {
        // Get the text from the input field
        string userInput = inputField.text;

        // Do something with the user input (you can replace this with your logic)
        DisplayUserInput(userInput);

        // Clear the input field after processing
        inputField.text = "";
    }

    void DisplayUserInput(string userInput)
    {
        // Display the user input in the Text object
        displayText.text = "User Input: " + userInput;
    }
}
