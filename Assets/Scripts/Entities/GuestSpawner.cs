using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuestSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject guestPrefab;
    [SerializeField] private float spawnInterval = 20f;

    private List<Transform> allChairs = new();
    private HashSet<Transform> occupiedChairs = new();

    private void Start()
    {
        GameObject[] chairs = GameObject.FindGameObjectsWithTag("Chair");
        foreach (GameObject chair in chairs)
        {
            allChairs.Add(chair.transform);
        }

        StartCoroutine(SpawnGuestLoop());
    }

    private IEnumerator SpawnGuestLoop()
    {
        while (true)
        {
            SpawnGuest();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnGuest()
    {
        List<Transform> availableChairs = new();
        foreach (Transform chair in allChairs)
        {
            if (!occupiedChairs.Contains(chair))
            {
                availableChairs.Add(chair);
            }
        }

        if (availableChairs.Count == 0)
        {
            Debug.Log("All chairs are occupied. No guest spawned.");
            return;
        }

        Transform chosenChair = availableChairs[Random.Range(0, availableChairs.Count)];
        GameObject guest = Instantiate(guestPrefab, chosenChair.position, chosenChair.rotation);

        occupiedChairs.Add(chosenChair);

        StartCoroutine(FadeInGuest(guest));
    }

    private IEnumerator FadeInGuest(GameObject guest)
    {
        Renderer[] renderers = guest.GetComponentsInChildren<Renderer>();
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

        float duration = 1.5f;
        float time = 0f;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, time / duration);
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

        foreach (Material mat in materials)
        {
            if (mat.HasProperty("_BaseColor"))
            {
                Color color = mat.GetColor("_BaseColor");
                color.a = 1f;
                mat.SetColor("_BaseColor", color);
            }
        }
    }

}
