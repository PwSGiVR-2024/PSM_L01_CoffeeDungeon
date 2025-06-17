using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance { get; private set; }
    private InventoryInputHandler inventoryInputHandler;
    private TMP_Text communicatText;
    [Header("References")]
    [SerializeField] private GameObject communicatUI;
    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            inventoryInputHandler = player.GetComponent<InventoryInputHandler>();
        }

        if (Instance != null && Instance != this) {
        Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        communicatText = communicatUI.GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        inventoryInputHandler.InItemTrigger += ShowCommunicatText;
        inventoryInputHandler.LeaveItemTrigger += ResetCommunicatText;
    }
    private void OnDisable()
    {
        inventoryInputHandler.InItemTrigger -= ShowCommunicatText;
        inventoryInputHandler.LeaveItemTrigger -= ResetCommunicatText;
    }

    private void ShowCommunicatText()
    {
        communicatText.text = "Pick Up Item: F";
    }
    
    private void ResetCommunicatText()
    {
        communicatText.text = "";
    }

}
