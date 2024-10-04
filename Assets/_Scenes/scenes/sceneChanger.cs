using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI; // For UI elements\
using TMPro; // Import for TextMeshPro

public class SceneChanger : MonoBehaviour
{
    
    // This method is called when the player enters the trigger
    private string succesString = "<size=8>Congratulations!\n <size=6>You trusted the car enough to let it park itself.\n<size=4>The next scene will load in 15 seconds, or press ENTER to skip.";

    private string spaceTriggerString = "<size=8>Woops!\n <size=6>You didn't trust the car enough to let it park itself.\n<size=4>The next scene will load in 15 seconds, or press ENTER to skip.";

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
        // if (Input.GetKeyDown(KeyCode.Space) && !isTriggered)
        // {   
        //     Debug.Log(isTriggered);
        //     isTriggered=true;
        //     TriggerPopup(spaceTriggerString);
            
        // }
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
        
        popupPanel.SetActive(true); // Show the popup UI
        Time.timeScale=0;
        StartCoroutine(PopupTimer(displayDuration)); // Start the timer
        
        
    }

    IEnumerator PopupTimer(float duration)
    {
        float remainingTime = duration;
        // Optionally, allow pressing a key to skip the timer and proceed immediately
        while (remainingTime > 0)
        {
            if (Input.GetKeyDown(KeyCode.Return)) // Press space to skip timer and proceed
            {
                break;
            }

            remainingTime -= Time.unscaledDeltaTime;
            yield return null;
        }

        // Hide the popup and load the next scene after the timer is done
        networkingManager.NextTrial();
        popupPanel.SetActive(false);
        Time.timeScale = 1;
        isTriggered=false;
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