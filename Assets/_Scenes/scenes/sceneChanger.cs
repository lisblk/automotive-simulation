using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI; // For UI elements\
using TMPro; // Import for TextMeshPro

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad; // Name of the scene to load, currently unused
    // This method is called when the player enters the trigger
    private string succesString = "<size=8>Congratulations!\n <size=6>You trusted the car enough to let it park itself.\n<size=2>The next scene will load in 15 seconds, or press SPACE to skip.";

    private string spaceTriggerString = "<size=8>Woops!\n <size=6>You didn't trust the car enough to let it park itself.\n<size=2>The next scene will load in 15 seconds, or press SPACE to skip.";

    private NetworkingManager networkingManager;
    private bool isObjectFullyInside = false;
    private Collider triggerCollider;
    private bool isTriggered = false;
    public GameObject popupPanel; // The popup panel that contains the message UI
    public TextMeshProUGUI popupText; // Reference to the Text UI component for the popup message
    public float displayDuration = 15f; // Time to display the popup before transitioning

    void Start()
    {
        // Find the NetworkingManager instance in the scene
        networkingManager = FindObjectOfType<NetworkingManager>();
        triggerCollider = GetComponent<Collider>();
        if (networkingManager == null)
        {
            Debug.LogError("NetworkingManager instance not found in the scene.");
        }
    }

    void Update()
    {
        // Check for space bar press to trigger different text
        if (Input.GetKeyDown(KeyCode.Space) && !isTriggered)
        {
            isTriggered=true;
            TriggerPopup(spaceTriggerString);
            isTriggered=false;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ManualCar"))
        {
            if (IsFullyInside(other))
            {
                isObjectFullyInside = true;
                // Your logic for when the full object is inside
                Debug.Log(other.name + " is fully inside the trigger area.");
                isTriggered=true;
                TriggerPopup(succesString);
                isTriggered=false;
            }
        }
    }
    

    // Show the popup and start a coroutine for timer-based transition
    void TriggerPopup(string message)
    {
        popupText.text = message; // Update the popup text based on trigger
        Time.timeScale=0;
        popupPanel.SetActive(true); // Show the popup UI
        StartCoroutine(PopupTimer(displayDuration)); // Start the timer
    }

    IEnumerator PopupTimer(float duration)
    {
        float remainingTime = duration;

        // Optionally, allow pressing a key to skip the timer and proceed immediately
        while (remainingTime > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space)) // Press space to skip timer and proceed
            {
                break;
            }

            remainingTime -= Time.deltaTime;
            yield return null;
        }

        // Hide the popup and load the next scene after the timer is done
        popupPanel.SetActive(false);
        networkingManager.NextTrial();
        Time.timeScale = 1;
    }

    void OnTriggerStay(Collider other)
    {
        if (!isObjectFullyInside && IsFullyInside(other) && other.CompareTag("ManualCar"))
        {
            isObjectFullyInside = true;
            Debug.Log(other.name + " is fully inside the trigger area.");
            // Your logic for when the full object is inside
            isTriggered=true;
            TriggerPopup(succesString);
            isTriggered=false;
            // networkingManager.NextTrial();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isObjectFullyInside)
        {
            isObjectFullyInside = false;
            // Your logic for when the object is no longer fully inside
            Debug.Log(other.name + " has exited the trigger area.");
        }
    }

    private bool IsFullyInside(Collider other)
    {
        // Get the bounds of the trigger and the other object
        Bounds triggerBounds = triggerCollider.bounds;
        Bounds objectBounds = other.bounds;

        // Check if the object's bounds are fully inside the trigger's bounds
        return triggerBounds.Contains(objectBounds.min) && triggerBounds.Contains(objectBounds.max);
    }
}

//     private void OnTriggerEnter(Collider other)
//     {
//         if (other.CompareTag("ManualCar"))
//         {
//             networkingManager.NextTrial();

//             // Destroy(gameObject);

//         }
//     }
// }