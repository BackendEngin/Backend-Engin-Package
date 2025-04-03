using backendEngin;
using UnityEngine;
using UnityEngine.UI;

// This class handles player registration in the Unity game.
public class RegisterPlayers : MonoBehaviour
{
    // UI elements for registration
    public Button registerButton;      // Button to trigger registration
    public InputField email;           // Input field for the user's email
    public InputField password;        // Input field for the user's password
    public InputField username;        // Input field for the user's username
    public InputField location;        // Input field for the user's location
    public InputField phoneNumber;     // Input field for the user's phone number

    // This method is called when the script is first run
    private void Start()
    {
        // Add a listener to the register button to call OnRegisterButtonClick when clicked
        registerButton.onClick.AddListener(OnRegisterButtonClick);
    }

    // This method is called when the register button is clicked
    private async void OnRegisterButtonClick()
    {
        // Call the Register method from the registerPlayer class, passing the input values
        // Wait for the registration to complete and get the player's ID
        int playerID = await RegisterPlayer.Register(
            email.text,           // User's email from input field
            password.text,        // User's password from input field
            location.text,        // User's location from input field
            username.text,        // User's username from input field
            phoneNumber.text,     // User's phone number from input field
            "label",              // A placeholder for label, can be adjusted based on requirements
            "tags"                // A placeholder for tags, can be adjusted based on requirements
        );

        // Log the player's ID to the console after successful registration
        Debug.Log("Player ID: " + playerID);
    }
}