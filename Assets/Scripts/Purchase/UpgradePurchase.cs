using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradePurchase : MonoBehaviour
{
    [SerializeField] private int maxUpgradeLevel = 3;
    
    private int _upgradeCost;
    private TextMeshProUGUI _costText;
    private TextMeshProUGUI _weaponLevel;
    
    private bool _isPlayerInRange;
    private Player _player;

    private void Start()
    {
        _costText = GameObject.FindWithTag("Warning").GetComponent<TextMeshProUGUI>();
        _weaponLevel = GameObject.FindWithTag("WeaponLevel").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        
        _player = other.gameObject.GetComponent<Player>();
        
        if(!_player.HasWeapon) return;
        if(_player.Weapon.NextUpgradeLevel > maxUpgradeLevel) return;
        
        _isPlayerInRange = true;

        _upgradeCost = _player.Weapon.upgradeBaseCost * _player.Weapon.NextUpgradeLevel;
        _costText.text += "Pressione [F] para melhorar a sua arma por " + _upgradeCost + " dinheiros\n";
        
        _player.OnInteractAction += Upgrade;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        _isPlayerInRange = false;

        string warningText = "Pressione [F] para melhorar a sua arma por " + _upgradeCost + " dinheiros\n";
        
        string text = _costText.text;
        text = text.Replace(warningText, "");
        _costText.text = text;
        
        _player.OnInteractAction -= Upgrade;
        _player = null;
    }

    private void Upgrade()
    {
        if(!_isPlayerInRange) return;

        Weapon weapon = _player.Weapon;

        bool purchased = _player.SpendMoney(weapon.upgradeBaseCost * weapon.NextUpgradeLevel);

        if (purchased)
        {
            _weaponLevel.text = weapon.NextUpgradeLevel.ToString();
            
            weapon.damage += weapon.damage / 2;
            weapon.maxAmmo += weapon.maxAmmo / 5;
            weapon.maxMagazineAmmo += weapon.maxMagazineAmmo / 3;
            weapon.NextUpgradeLevel++;
            weapon.BuyAmmo();
            
            string warningText = "Pressione [F] para melhorar a sua arma por " + _upgradeCost + " dinheiros\n";
            _upgradeCost = weapon.upgradeBaseCost * weapon.NextUpgradeLevel;
            string newWarningText = "Pressione [F] para melhorar a sua arma por " + _upgradeCost + " dinheiros\n";
        
            string text = _costText.text;
            text = text.Replace(warningText, newWarningText);
            _costText.text = text;
        }
    }
}
