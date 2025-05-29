using UnityEngine;

public class TextManager : MonoBehaviour
{
    public TextManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null & Instance != this) {
        Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }



}
