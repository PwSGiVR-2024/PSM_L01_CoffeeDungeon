using UnityEngine;

public class TextManager : MonoBehaviour
{
    public TextManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null & instance != this) {
        Destroy(this);
        }
        else
        {
            instance = this;
        }
    }



}
