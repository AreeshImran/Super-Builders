using UnityEngine;
using TMPro; // Use 'using UnityEngine.UI;' if not using TextMeshPro
using System.Collections;

public class PlayerChat : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField chatInputField; // Drag your dummy text box here
    public GameObject bubbleObject;        // Drag the 'ChatBubble' object here
    public TextMeshProUGUI bubbleText;     // Drag the Text component inside the bubble here

    [Header("Settings")]
    public float displayDuration = 3f;     // How long the bubble stays visible

    private Coroutine hideCoroutine;

    public void OnSendButtonClick()
    {
        // 1. Get the text from the input field
        string message = chatInputField.text;

        // 2. Don't do anything if the message is empty
        if (!string.IsNullOrWhiteSpace(message))
        {
            ShowBubble(message);
        }

        // 3. Clear the input field and keep it selected for next time
        chatInputField.text = "";
        chatInputField.ActivateInputField();
    }

    private void ShowBubble(string text)
    {
        // Stop any existing timer so they don't overlap
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);

        bubbleText.text = text;
        bubbleObject.SetActive(true);

        // Start the timer to hide the bubble
        hideCoroutine = StartCoroutine(HideBubbleAfterTime());
    }

    IEnumerator HideBubbleAfterTime()
    {
        yield return new WaitForSeconds(displayDuration);
        bubbleObject.SetActive(false);
    }

    void LateUpdate() // LateUpdate is better for UI following
{
    // Force the chat canvas scale to always be positive even if player is flipped
    if (bubbleObject.transform.parent != null)
    {
        Vector3 parentScale = transform.localScale;
        bubbleObject.transform.parent.localScale = new Vector3(
            Mathf.Abs(bubbleObject.transform.parent.localScale.x), 
            bubbleObject.transform.parent.localScale.y, 
            bubbleObject.transform.parent.localScale.z
        );
    }
}
}