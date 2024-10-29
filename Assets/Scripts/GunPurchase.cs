using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunPurchase : MonoBehaviour
{
    [SerializeField]
    int gunCost;

    [SerializeField] private GameObject buyWeaponText;
    private TextMeshProUGUI _costText;

    [SerializeField] private GameObject gunPrefab;

    private bool _isPlayerInRange = false;
    private Player _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _costText = buyWeaponText.GetComponent<TextMeshProUGUI>();
        buyWeaponText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _costText.text = "Pressione [F] para comprar por " + gunCost.ToString();
        buyWeaponText.SetActive(true);
            
        _player = other.gameObject.GetComponent<Player>();
        _isPlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        buyWeaponText.SetActive(false);
            
        _player = null;
        _isPlayerInRange = false;
    }

    private void BuyWeapon()
    {
        bool purchased = _player.SpendMoney(gunCost);

        if (purchased && !_player.HasWeapon)
        {
            GameObject newGun = Instantiate(gunPrefab, _player.transform.position, Quaternion.identity);
            newGun.transform.SetParent(_player.transform); // Posiciona a arma no jogador
            _player.HasWeapon = true;
        }
        else
        {
            print("Não comproule");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPlayerInRange || !Keyboard.current.fKey.wasPressedThisFrame) return;
        
        BuyWeapon();
    }
}