using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhoneCall : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private Button[] numberButtons; // 0-9, *, # (12 buttons)
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button callButton;
    [SerializeField] private GameObject callPanel; // Panel to show on call
    [SerializeField] private TextMeshProUGUI callDisplayText; // TMP field for dialed number
    [SerializeField] private GameObject Dialogue;
    private string phoneNumber = "";

    void Start()
    {
        // Assign button listeners
        for (int i = 0; i < numberButtons.Length; i++)
        {
            string buttonValue = i < 10 ? i.ToString() : (i == 10 ? "*" : "#");
            numberButtons[i].onClick.AddListener(() => AddDigit(buttonValue));
        }

        deleteButton.onClick.AddListener(DeleteLastDigit);
        callButton.onClick.AddListener(MakeCall);

        // Ensure call panel is hidden initially
        if (callPanel != null)
        {
            callPanel.SetActive(false);
        }
    }

    void AddDigit(string digit)
    {
        phoneNumber += digit;
        UpdateDisplay();
    }

    void DeleteLastDigit()
    {
        if (phoneNumber.Length > 0)
        {
            phoneNumber = phoneNumber.Substring(0, phoneNumber.Length - 1);
            UpdateDisplay();
        }
    }

    void MakeCall()
    {
        if (string.IsNullOrEmpty(phoneNumber))
        {
            Debug.Log("No number entered!");
            return;
        }

        Debug.Log($"Calling: {phoneNumber}");

        // Check for special number
        if (phoneNumber == "16778338")
        {
            OnSpecialNumberCalled();
        }

        // Show the call panel and update the call display text
        if (callPanel != null)
        {
            callPanel.SetActive(true);
        }
        if (callDisplayText != null)
        {
            callDisplayText.text = phoneNumber;
        }
    }

    void UpdateDisplay()
    {
        displayText.text = phoneNumber;
    }

    void OnSpecialNumberCalled()
    {
        Dialogue.SetActive(true);
    }
}