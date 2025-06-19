using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Guest : MonoBehaviour
{

    private ItemData firstChoice;
    private ItemData secondChoice;

    [Header("References")]
    [SerializeField] private Image firstChoiceImage;
    [SerializeField] private Image secondChoiceImage;
    [SerializeField] private CraftableItemList itemList;
    [SerializeField] private Inventory inventory;

    [Header("Guest Settings")]
    [SerializeField] private float leaveTime = 2f;
    [SerializeField] private float fadeOutDuration = 1f;

    [Header("Feedback")]
    [SerializeField] private GameObject satisfactionEffect;
    [SerializeField] private GameObject disappointmentEffect;
    [SerializeField] private GameObject neutralEffect;
    [SerializeField] private AudioClip satisfactionSound;
    [SerializeField] private AudioClip disappointmentSound;
    [SerializeField] private AudioClip neutralSound;


    private bool hasReceivedItem = false;
    private bool isLeaving = false;
    private Transform occupiedChair;
    private GuestSpawner spawner;
    private AudioSource audioSource;

    //events
    public event Action FirstChoiceSatisfaction;
    public event Action SecondChoiceSatisfaction;
    public event Action IncorrectChoiceSatysfaction;
    public event Action BeholdItHasSlimeInIt;

    private void Awake()
    {
        
        GenerateFirstSecondChoice();
        audioSource = GetComponent<AudioSource>();
        
        spawner = FindFirstObjectByType<GuestSpawner>();
        
        FindOccupiedChair();
    }

    private void FindOccupiedChair()
    {
        GameObject[] chairs = GameObject.FindGameObjectsWithTag("Chair");
        if (chairs.Length == 0)
        {
            return;
        }

        float closestDistance = float.MaxValue;
        
        foreach (GameObject chair in chairs)
        {
            float distance = Vector3.Distance(transform.position, chair.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                occupiedChair = chair.transform;
            }
        }
    }

    private void GenerateFirstSecondChoice()
    {
        if (itemList == null)
        {
            return;
        }

        if (itemList.craftableItems == null || itemList.craftableItems.Count == 0)
        {
            return;
        }

        int index1 = Random.Range(0, itemList.craftableItems.Count);
        firstChoice = itemList.craftableItems[index1];

        //second choice cant be the same as the first 
        int index2;

        do
        {
            index2 = Random.Range(0, itemList.craftableItems.Count);
        } while (index2 == index1 && itemList.craftableItems.Count > 1);

        secondChoice = itemList.craftableItems[index2];
        
        //set ui elements
        if (firstChoiceImage != null && firstChoice != null)
            firstChoiceImage.sprite = firstChoice.iconPrefab;

        if (secondChoiceImage != null && secondChoice != null)
            secondChoiceImage.sprite = secondChoice.iconPrefab;
    }

    public void ReceiveItem(ItemData receivedItem)
    {

        if (receivedItem == null || hasReceivedItem || isLeaving)
        {
            return;
        }

        hasReceivedItem = true;
            
        if (receivedItem == firstChoice)
        {
            FirstChoiceSatisfaction?.Invoke();
            ReactToFirstChoiceItem(receivedItem);
        }

        else if(receivedItem == secondChoice)
        {
            SecondChoiceSatisfaction?.Invoke();
            ReactToSecondChoiceItem(receivedItem);
        }

        else
        {
            IncorrectChoiceSatysfaction?.Invoke();
            ReactToIncorrectItem(receivedItem);
        }
        if (receivedItem.hasSlime)
        {
            BeholdItHasSlimeInIt?.Invoke();
        }

        StartCoroutine(LeaveAfterRecivingOrder());
        inventory.RemoveItem(receivedItem,1);
    }

    private void ReactToFirstChoiceItem(ItemData item)
    {
        if (audioSource != null && satisfactionSound != null)
        {
            audioSource.PlayOneShot(satisfactionSound);
        }
        
        //if (satisfactionEffect != null)
        //{
        //    GameObject effect = Instantiate(satisfactionEffect, transform.position, transform.rotation);
        //    Destroy(effect, 3f);
        //}
        
    }

    private void ReactToSecondChoiceItem(ItemData item)
    {
        if (audioSource != null && neutralSound != null)
        {
            audioSource.PlayOneShot(neutralSound);
        }
        //if (satisfactionEffect != null)
        //{
        //    GameObject effect = Instantiate(neutralEffect, transform.position, transform.rotation);
        //    Destroy(effect, 3f);
        //}
    }

    private void ReactToIncorrectItem(ItemData item)
    {
        if (audioSource != null && disappointmentSound != null)
        {
            audioSource.PlayOneShot(disappointmentSound);
        }
        
        //if (disappointmentEffect != null)
        //{
        //    GameObject effect = Instantiate(disappointmentEffect, transform.position, transform.rotation);
        //    Destroy(effect, 3f);
        //}
    }

    private IEnumerator LeaveAfterRecivingOrder()
    {
        isLeaving = true;
        yield return new WaitForSeconds(leaveTime);
        yield return StartCoroutine(FadeOutAndDestroy());
    }


    private IEnumerator FadeOutAndDestroy()
    {
        List<Material> materials = CloneAllMaterials();

        float time = 0f;

        while (time < fadeOutDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, time / fadeOutDuration);
            SetMaterialsAlpha(materials, alpha);

            time += Time.deltaTime;
            yield return null;
        }

        if (spawner != null && occupiedChair != null)
        {
            spawner.FreeChair(occupiedChair);
        }
        SatisfactionManager.Instance?.UnregisterGuest(this);
        Destroy(gameObject);
    }

    private List<Material> CloneAllMaterials()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        List<Material> clonedMaterials = new();

        foreach (Renderer renderer in renderers)
        {
            Material[] originalMats = renderer.materials;
            Material[] newMats = new Material[originalMats.Length];

            for (int i = 0; i < originalMats.Length; i++)
            {
                newMats[i] = new Material(originalMats[i]);
                clonedMaterials.Add(newMats[i]);
            }

            renderer.materials = newMats;
        }

        return clonedMaterials;
    }

    private void SetMaterialsAlpha(List<Material> materials, float alpha)
    {
        foreach (Material mat in materials)
        {
            if (mat.HasProperty("_BaseColor"))
            {
                Color color = mat.GetColor("_BaseColor");
                color.a = alpha;
                mat.SetColor("_BaseColor", color);
            }
        }
    }

    public bool CanReceiveItem()
    {
        bool canReceive = !hasReceivedItem && !isLeaving;
        return canReceive;
    }

}