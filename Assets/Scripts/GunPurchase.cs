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
    [SerializeField] private GameObject gunImage;
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
        _player.OnInteractAction += BuyWeapon; // Adiciona o método BuyWeapon ao evento de interação
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        buyWeaponText.SetActive(false);
        
        _player.OnInteractAction -= BuyWeapon; // Remove o método BuyWeapon do evento de interação
        _player = null;
        _isPlayerInRange = false;
    }

    private void BuyWeapon()
    {
        if (!_isPlayerInRange) return;
        
        bool purchased = _player.SpendMoney(gunCost);

        if (purchased)
        {
            if (!_player.HasWeapon)
            {
                gunImage.SetActive(true);
                _player.HasWeapon = true;
                _player.Weapon = gunPrefab.GetComponent<Pistol>();
                _player.Weapon.spawnerBulletPos = _player.spawnerBulletPos;
            }

            
            _player.Weapon.BuyAmmo();
        }
        else
        {
            Debug.Log("Dinheiro insuficiente para comprar/recarregar!");
        }
    }
}