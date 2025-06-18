using UnityEngine;
using System.Collections.Generic;

public class SatisfactionManager : MonoBehaviour
{
    public static SatisfactionManager Instance { get; private set; }

    public int satisfactionScore;
    private readonly int happy = 100;
    private int neutral = 50;
    private int disappointed = -50;
    private readonly Dictionary<Guest, System.Action> firstChoiceHandlers = new();
    private readonly Dictionary<Guest, System.Action> secondChoiceHandlers = new();
    private readonly Dictionary<Guest, System.Action> incorrectChoiceHandlers = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void RegisterGuest(Guest guest)
    {
        if (guest == null) return;

        void firstHandler() => AddSatisfaction(happy);
        void secondHandler() => AddSatisfaction(neutral);
        void incorrectHandler() => AddSatisfaction(disappointed);

        guest.FirstChoiceSatisfaction += firstHandler;
        guest.SecondChoiceSatisfaction += secondHandler;
        guest.IncorrectChoiceSatysfaction += incorrectHandler;

        firstChoiceHandlers[guest] = firstHandler;
        secondChoiceHandlers[guest] = secondHandler;
        incorrectChoiceHandlers[guest] = incorrectHandler;
    }

    public void UnregisterGuest(Guest guest)
    {
        if (guest == null) return;

        if (firstChoiceHandlers.TryGetValue(guest, out var first))
        {
            guest.FirstChoiceSatisfaction -= first;
            firstChoiceHandlers.Remove(guest);
        }

        if (secondChoiceHandlers.TryGetValue(guest, out var second))
        {
            guest.SecondChoiceSatisfaction -= second;
            secondChoiceHandlers.Remove(guest);
        }

        if (incorrectChoiceHandlers.TryGetValue(guest, out var incorrect))
        {
            guest.IncorrectChoiceSatysfaction -= incorrect;
            incorrectChoiceHandlers.Remove(guest);
        }
    }

    private void AddSatisfaction(int amount)
    {
        satisfactionScore += amount;
        Debug.Log("Satisfaction Updated: " + satisfactionScore);
    }
}
