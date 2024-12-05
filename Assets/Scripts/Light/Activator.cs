using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Activator : MonoBehaviour
{
    private AllActivators _allActivators;
    private TextMeshProUGUI _warningText;
    private string _message = "";
    private bool _activated;
    Player _player;

    private void Start()
    {
        _allActivators = GetComponentInParent<AllActivators>();
        _warningText = GameObject.FindWithTag("Warning").GetComponent<TextMeshProUGUI>();
    }

    private void UpdateMessage()
    {
        string oldMessage = _message;
        
        _message = _activated
            ? "Energia ativada (" + _allActivators.ActivatedNum + "/3)\n"
            : "Pressione [F] para ativar energia\n";

        string text = _warningText.text;
        
        _warningText.text = oldMessage == "" ? text + _message : text.Replace(oldMessage, _message);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        UpdateMessage();
        
        _player = collision.gameObject.GetComponent<Player>();
        _player.OnInteractAction += Activate;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        string text = _warningText.text;
        _warningText.text = text.Replace(_message, "");
        _message = "";
        
        _player.OnInteractAction -= Activate;
        _player = null;
    }

    private void Activate()
    {
        _activated = true;
        _allActivators.ActivateOne();
        UpdateMessage();
    }
}
