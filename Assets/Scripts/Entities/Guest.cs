using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guest : MonoBehaviour
{
    public ItemData firstChoice;
    public ItemData secondChoice;

    [Header("References")]
    [SerializeField] private Image firstChoiceImage;
    [SerializeField] private Image secondChoiceImage;
    [SerializeField] private CraftableItemList itemList;

    [Header("Guest Settings")]
    [SerializeField] private float satisfactionTime = 3f;
    [SerializeField] private float disappointmentTime = 2f;
    [SerializeField] private float fadeOutDuration = 1f;

    [Header("Feedback")]
    [SerializeField] private GameObject satisfactionEffect;
    [SerializeField] private GameObject disappointmentEffect;
    [SerializeField] private AudioClip satisfactionSound;
    [SerializeField] private AudioClip disappointmentSound;


    private bool hasReceivedItem = false;
    private bool isLeaving = false;
    private Transform occupiedChair;
    private GuestSpawner spawner;
    private AudioSource audioSource;

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

        int index2;
        do
        {
            index2 = Random.Range(0, itemList.craftableItems.Count);
        } while (index2 == index1 && itemList.craftableItems.Count > 1);

        secondChoice = itemList.craftableItems[index2];
        
        if (firstChoiceImage != null && firstChoice != null)
            firstChoiceImage.sprite = firstChoice.iconPrefab;

        if (secondChoiceImage != null && secondChoice != null)
            secondChoiceImage.sprite = secondChoice.iconPrefab;
    }

    public void ReceiveItem(ItemData receivedItem)
    {

        if (receivedItem == null)
        {
            return;
        }

        if (hasReceivedItem)
        {
            return;
        }

        if (isLeaving)
        {
            return;
        }

        hasReceivedItem = true;
            
        if (receivedItem == firstChoice || receivedItem == secondChoice)
        {
            ReactToCorrectItem(receivedItem);
        }
        else
        {
            ReactToIncorrectItem(receivedItem);
        }
    }

    private void ReactToCorrectItem(ItemData item)
    {
        
        if (audioSource != null && satisfactionSound != null)
        {
            audioSource.PlayOneShot(satisfactionSound);
        }
        
        if (satisfactionEffect != null)
        {
            GameObject effect = Instantiate(satisfactionEffect, transform.position, transform.rotation);
            Destroy(effect, 3f);
        }
        
        RateOrder(true);
        Pay(true);
        StartCoroutine(LeaveAfterSatisfaction());
    }

    private void ReactToIncorrectItem(ItemData item)
    {
        print("not happy :<");
        if (audioSource != null && disappointmentSound != null)
        {
            audioSource.PlayOneShot(disappointmentSound);
        }
        
        if (disappointmentEffect != null)
        {
            GameObject effect = Instantiate(disappointmentEffect, transform.position, transform.rotation);
            Destroy(effect, 3f);
        }
        
        RateOrder(false);
        Pay(false);
        StartCoroutine(LeaveAfterDisappointment());
    }

    private void RateOrder(bool isPositive)
    {
        //just ui emoji maybe?
    }

    private void Pay(bool isCorrectOrder)
    {
       //still to do
    }

    private IEnumerator LeaveAfterSatisfaction()
    {
        isLeaving = true;
        yield return new WaitForSeconds(satisfactionTime);
        yield return StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator LeaveAfterDisappointment()
    {
        isLeaving = true;
        yield return new WaitForSeconds(disappointmentTime);
        yield return StartCoroutine(FadeOutAndDestroy());
    }

    private IEnumerator FadeOutAndDestroy()
    {
        
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        List<Material> materials = new();

        foreach (Renderer renderer in renderers)
        {
            Material[] mats = renderer.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = new Material(mats[i]);
            }
            renderer.materials = mats;
            materials.AddRange(mats);
        }

        float time = 0f;

        while (time < fadeOutDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, time / fadeOutDuration);
            foreach (Material mat in materials)
            {
                if (mat.HasProperty("_BaseColor"))
                {
                    Color color = mat.GetColor("_BaseColor");
                    color.a = alpha;
                    mat.SetColor("_BaseColor", color);
                }
            }

            time += Time.deltaTime;
            yield return null;
        }

        if (spawner != null && occupiedChair != null)
        {
            spawner.FreeChair(occupiedChair);
        }
        Destroy(gameObject);
    }


    public bool CanReceiveItem()
    {
        bool canReceive = !hasReceivedItem && !isLeaving;
        return canReceive;
    }

    public ItemData[] GetPreferences()
    {
        return new ItemData[] { firstChoice, secondChoice };
    }
}