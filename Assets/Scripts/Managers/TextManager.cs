using TMPro;
using UnityEngine;
using static UnityEditor.Searcher.Searcher.AnalyticsEvent;
using UnityEngine.InputSystem.LowLevel;

public enum TextEvent
{
    ItemPickup,
    GiveOrder
}

public class TextManager : MonoBehaviour
{
    public static TextManager Instance { get; private set; }
    private InventoryInputHandler inventoryInputHandler;
    private PlayerController playerController;
    private TMP_Text communicatText;
    [Header("References")]
    [SerializeField] private GameObject communicatUI;

    private string itemPickupText = "To pick up item press F";
    private string giveOrderText = "To give order press G";
    private void Awake()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            inventoryInputHandler = player.GetComponent<InventoryInputHandler>();
            playerController=player.GetComponent<PlayerController>();
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
        inventoryInputHandler.InItemTrigger += () => ShowCommunicatText(TextEvent.ItemPickup);
        playerController.CanGiveOrder += () => ShowCommunicatText(TextEvent.GiveOrder);
        inventoryInputHandler.LeaveItemTrigger += ResetCommunicatText;
        playerController.LeftOrderTrigger += ResetCommunicatText;
    }
    private void OnDisable()
    {
inventoryInputHandler.InItemTrigger -= () => ShowCommunicatText(TextEvent.ItemPickup);
        inventoryInputHandler.LeaveItemTrigger -= ResetCommunicatText;
        playerController.CanGiveOrder -= () => ShowCommunicatText(TextEvent.GiveOrder);
        playerController.LeftOrderTrigger -= ResetCommunicatText;
    }

    private void ShowCommunicatText(TextEvent eventType)
    {
        switch (eventType)
        {
            case TextEvent.ItemPickup:
                communicatText.text = itemPickupText;
                break;

            case TextEvent.GiveOrder:
                communicatText.text = giveOrderText;
                break;

            default:
                communicatText.text = "";
                break;
        }
    }
    
    private void ResetCommunicatText()
    {
        communicatText.text = "";
    }

}
