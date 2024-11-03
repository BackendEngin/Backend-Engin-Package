using backendEngin;
using UnityEngine;
using UnityEngine.UI;

// This class handles player login in the Unity game.
public class LoginPlayers : MonoBehaviour
{
    // UI elements for login
    public Button loginButton;        // Button to trigger login
    public InputField emailOrPhone;   // Input field for the user's email or phone number
    public InputField password;       // Input field for the user's password
    public Toggle useEmailToggle;     // Toggle to choose between email and phone number login

    // Called when the script is first run
    private void Start()
    {
        // Add a listener to the login button to call OnLoginButtonClick when clicked
        loginButton.onClick.AddListener(OnLoginButtonClick);
    }

    // Called when the login button is clicked
    private async void OnLoginButtonClick()
    {
        // Determine if we're using email or phone for login
        bool isUsingEmail = useEmailToggle.isOn;

        // Call the Login method from the LoginPlayer class, passing the input values
        int playerID = await LoginPlayer.Login(
            emailOrPhone.text,   // User's email or phone number from input field
            password.text,       // User's password from input field
            isUsingEmail         // Boolean to indicate whether we're using email or phone number
        );

        // Check if login was successful (playerID > 0 means success)
        if (playerID > 0)
        {
            Debug.Log("Login successful! Player ID: " + playerID);
        }
        else
        {
            Debug.Log("Login failed. Please check your credentials.");
        }
    }
}