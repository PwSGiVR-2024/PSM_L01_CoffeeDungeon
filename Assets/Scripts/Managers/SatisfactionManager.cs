using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public enum SatisfactionLevel
{
    neutral,
    happy,
    disappointed
}

public class SatisfactionManager : MonoBehaviour
{
    public static SatisfactionManager Instance { get; private set; }

    private int satisfactionScore=0;
    private readonly int happy = 100;
    private readonly int neutral = 50;
    private readonly int disappointed = -50;
    private readonly int slimeBonus = 50;
    private readonly Dictionary<Guest, System.Action> firstChoiceHandlers = new();
    private readonly Dictionary<Guest, System.Action> secondChoiceHandlers = new();
    private readonly Dictionary<Guest, System.Action> incorrectChoiceHandlers = new();
    private readonly Dictionary<Guest, System.Action> slimeHandlers = new();
    private SatisfactionLevel satisfactionLevel;

    [Header("References")]
    [SerializeField] private Image satisfactionIcon;
    [SerializeField] private GameObject endRunCanvas;

    [Header("Sprites")]
    [SerializeField] private Sprite happySprite;
    [SerializeField] private Sprite neutralSprite;
    [SerializeField] private Sprite disappointedSprite;

    public event Action ScoreChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        WhatSatisfaction();
    }

    public void RegisterGuest(Guest guest)
    {
        if (guest == null) return;

        void firstHandler() => AddSatisfaction(happy);
        void secondHandler() => AddSatisfaction(neutral);
        void incorrectHandler() => AddSatisfaction(disappointed);
        void slimeHandler() => AddSatisfaction(slimeBonus);

        guest.FirstChoiceSatisfaction += firstHandler;
        guest.SecondChoiceSatisfaction += secondHandler;
        guest.IncorrectChoiceSatysfaction += incorrectHandler;
        guest.BeholdItHasSlimeInIt += slimeHandler;

        firstChoiceHandlers[guest] = firstHandler;
        secondChoiceHandlers[guest] = secondHandler;
        incorrectChoiceHandlers[guest] = incorrectHandler;
        slimeHandlers[guest] = slimeHandler;
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
        if (slimeHandlers.TryGetValue(guest, out var slime))
        {
            guest.BeholdItHasSlimeInIt -= slime;
            slimeHandlers.Remove(guest);
        }
    }

    private void AddSatisfaction(int amount)
    {
        satisfactionScore += amount;
        WhatSatisfaction();
        ScoreChanged?.Invoke();
    }

    public int GetScore()
    {
        return satisfactionScore;
    }

    public SatisfactionLevel GetSatisfacionLevel()
    {
        return satisfactionLevel;
    }

    private void WhatSatisfaction()
    {
        if (satisfactionScore > 1000)
        {
            satisfactionLevel = SatisfactionLevel.happy;
            satisfactionIcon.sprite = happySprite;
        }
        else if (satisfactionScore < 0)
        {
            satisfactionLevel = SatisfactionLevel.disappointed;
            satisfactionIcon.sprite = disappointedSprite;
        }
        else
        {
            satisfactionLevel = SatisfactionLevel.neutral;
            satisfactionIcon.sprite = neutralSprite;
        }
    }

}
