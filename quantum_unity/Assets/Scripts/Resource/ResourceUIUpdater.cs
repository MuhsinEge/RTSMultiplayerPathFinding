using Quantum;
using ServiceLocator;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceUIUpdater : MonoBehaviour
{
    public TextMeshProUGUI player1ResourceTxt;
    public TextMeshProUGUI player2ResourceTxt;
    ResourceDataService _resourceDataService;
    // Start is called before the first frame update
    private void Awake()
    {
        _resourceDataService = Locator.Instance.Get<ResourceDataService>();
    }
    private void Start()
    {
        _resourceDataService.playerResourceEvent += OnResourceEvent;
        StartCoroutine(InitialUpdate());
    }

    IEnumerator InitialUpdate()
    {
        yield return new WaitUntil(() => QuantumRunner.Default.Game.GetLocalPlayers().Length > 0 == true);
        UpdateResourceText(0, 0);
        UpdateResourceText(1, 0);
    }

    private void OnResourceEvent(object sender, PlayerResourceEventData eventData)
    {
        UpdateResourceText(eventData.teamId, eventData.resourceAmount);
    }

    private void UpdateResourceText(int id, int amount)
    {
        var ownerTxt = QuantumRunner.Default.Game.GetLocalPlayers()[0] == id ? "Your Wood : " : "Opponent Wood :";
        if (id == 0)
        {

            player1ResourceTxt.text = ownerTxt + amount.ToString();
        }
        else
        {

            player2ResourceTxt.text = ownerTxt + amount.ToString();
        }
    }
}
