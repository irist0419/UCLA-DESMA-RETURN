using UnityEngine;
using UnityEngine.UI;

public class ShowObjectsOnText : MonoBehaviour
{
    // Reference to the UI Text component
    public Text uiText;

    // List of GameObjects to show or hide
    public GameObject[] objectsToShow;

    void Update()
    {
        // Check if the UI text is equal to "3"
        if (uiText.text == "3")
        {
            // Show the objects
            SetObjectsActive(true);
        }
        else
        {
            // Hide the objects
            SetObjectsActive(false);
        }
    }

    // Helper function to set the active state of the objects
    private void SetObjectsActive(bool isActive)
    {
        foreach (GameObject obj in objectsToShow)
        {
            obj.SetActive(isActive);
        }
    }
}

