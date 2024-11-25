using TMPro;
using UnityEngine;

public class GunPurchase : MonoBehaviour
{
    [SerializeField] int gunCost;
    [SerializeField] GameObject gunImage;
    [SerializeField] GameObject buyWeaponText;
    TextMeshProUGUI _costText;

    [SerializeField] GameObject gunPrefab;

    bool _isPlayerInRange = false;
    Player _player;
    
    void Start()
    {
        _costText = buyWeaponText.GetComponent<TextMeshProUGUI>();
        buyWeaponText.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _costText.text = "Pressione [F] para comprar por " + gunCost.ToString();
        buyWeaponText.SetActive(true);
            
        _player = other.gameObject.GetComponent<Player>();
        _isPlayerInRange = true;
        _player.OnInteractAction += BuyWeapon; // Adiciona o método BuyWeapon ao evento de interação
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        buyWeaponText.SetActive(false);
        
        _player.OnInteractAction -= BuyWeapon; // Remove o método BuyWeapon do evento de interação
        _player = null;
        _isPlayerInRange = false;
    }

    void BuyWeapon()
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
            print("Dinheiro insuficiente para comprar/recarregar!");
        }
    }
}