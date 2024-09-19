using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad; // Name of the scene to load
    // This method is called when the player enters the trigger

    private NetworkingManager networkingManager;
    private bool isObjectFullyInside = false;
    private Collider triggerCollider;
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


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ManualCar"))
        {
            if (IsFullyInside(other))
            {
                isObjectFullyInside = true;
                // Your logic for when the full object is inside
                Debug.Log(other.name + " is fully inside the trigger area.");
                networkingManager.NextTrial();
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!isObjectFullyInside && IsFullyInside(other))
        {
            isObjectFullyInside = true;
            // Your logic for when the full object is inside
            Debug.Log(other.name + " has now become fully inside the trigger area.");
            networkingManager.NextTrial();
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