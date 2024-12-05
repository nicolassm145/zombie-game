using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunPurchase : MonoBehaviour
{
    [SerializeField] int gunCost;
    [SerializeField] Texture2D gunImage;
    [SerializeField] GameObject gunPrefab;
    
    TextMeshProUGUI _costText;
    private TextMeshProUGUI _weaponLevel;

    bool _isPlayerInRange = false;
    Player _player;
    private Image _gunSlot;
    
    void Start()
    {
        _costText = GameObject.FindWithTag("Warning").GetComponent<TextMeshProUGUI>();
        _gunSlot = GameObject.FindWithTag("GunSlot").GetComponent<Image>();
        _weaponLevel =  GameObject.FindWithTag("WeaponLevel").GetComponent<TextMeshProUGUI>();
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
            if (_player.Weapon != gunPrefab.GetComponent<Weapon>())
            {
                // Atribui a imagem ao slot na UI
                _gunSlot.sprite = Sprite.Create(
                    gunImage,
                    new Rect(0, 0, gunImage.width, gunImage.height),
                    new Vector2(0.5f, 0.5f)
                );
                _gunSlot.color = new Color(1f, 1f, 1f, 1f);
                
                // Equipar a arma no jogador
                _player.EquipWeapon(gunPrefab);

                _weaponLevel.text = "";
            }
            _player.Weapon.BuyAmmo();
        }
        else
        {
            print("Dinheiro insuficiente para comprar/recarregar!");
        }
    }
}