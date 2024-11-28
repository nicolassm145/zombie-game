using TMPro;
using UnityEngine;

public class GunPurchase : MonoBehaviour
{
    [SerializeField] int gunCost;
    [SerializeField] GameObject gunImage;
    TextMeshProUGUI _costText;

    [SerializeField] GameObject gunPrefab;

    bool _isPlayerInRange = false;
    Player _player;
    
    void Start()
    {
        _costText = GameObject.FindWithTag("Warning").GetComponent<TextMeshProUGUI>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        _costText.text += "Pressione [F] para comprar por " + gunCost.ToString() + " dinheiros\n";
            
        _player = other.gameObject.GetComponent<Player>();
        _isPlayerInRange = true;
        _player.OnInteractAction += BuyWeapon; // Adiciona o método BuyWeapon ao evento de interação
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        string buyWarning = "Pressione [F] para comprar por " + gunCost.ToString() + " dinheiros\n";
        string text = _costText.text;
        int startIndex = text.IndexOf(buyWarning); // Localiza o índice inicial do trecho a ser removido

        if (startIndex != -1)
        { 
            text = text.Remove(startIndex, buyWarning.Length);  // Remove o trecho
        }
        
        _costText.text = text;  // Atribui a atualização ao texto
        
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
                _player.Weapon.SpawnerBulletPos = _player.spawnerBulletPos;
            }
            _player.Weapon.BuyAmmo();
        }
        else
        {
            print("Dinheiro insuficiente para comprar/recarregar!");
        }
    }
}