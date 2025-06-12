using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject ingredientList;
    [SerializeField] private GameObject ingredientRowPrefab;
    

    private CraftingRecipe recipe;

    public void Setup(CraftingRecipe recipe)
    {
        this.recipe = recipe;

        iconImage.sprite = recipe.result.iconPrefab;
        nameText.text = recipe.result.itemName;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        ingredientList.SetActive(true);
        print("hover");
        LoadIngredientsList();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ingredientList.SetActive(false);
        print("no hover");
        ClearIngredientsList();
    }

    private void LoadIngredientsList()
    {
        print("method activated");
        foreach (var ingredient in recipe.ingredients)
        {
            GameObject row = Instantiate(ingredientRowPrefab, ingredientList.transform);

            TMP_Text rowText = row.GetComponentInChildren<TMP_Text>();
            if (rowText != null)
            {
                rowText.text = $"{ingredient.item.itemName} x{ingredient.quantity}";
            }
            else
            {
                Debug.LogWarning("TMP_Text component not found in ingredientRowPrefab.");
            }
            rowText.text = $"{ingredient.item.itemName} x{ingredient.quantity}";
        }
    }

    private void ClearIngredientsList()
    {
        foreach (Transform child in ingredientList.transform)
        {
            Destroy(child.gameObject);
            print("cleared ingredients");
        }
    }
}
