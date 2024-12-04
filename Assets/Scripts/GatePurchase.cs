using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GatePurchase : MonoBehaviour
{
    [SerializeField] int gateCost;
    [SerializeField] private AudioClip doorUnlocked;
    AudioSource _audioSource;
    TextMeshProUGUI _costText;
    bool _isPlayerInRange;
    
    Player _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _costText = GameObject.FindWithTag("Warning").GetComponent<TextMeshProUGUI>();
        _audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        _isPlayerInRange = true;
        _player = other.gameObject.GetComponent<Player>();
        _costText.text += "Pressione [F] para abrir por " + gateCost.ToString() + " dinheiros\n";
        _player.OnInteractAction += OpenGate;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        string buyWarning = "Pressione [F] para abrir por " + gateCost.ToString() + " dinheiros\n";
        string text = _costText.text;
        int startIndex = text.IndexOf(buyWarning); // Localiza o índice inicial do trecho a ser removido
        
        if (startIndex != -1)
        { 
            text = text.Remove(startIndex, buyWarning.Length);  // Remove o trecho
        }
        
        _costText.text = text;  // Atribui a atualização ao texto
        _isPlayerInRange = false;
        _player.OnInteractAction -= OpenGate;
        _player = null;
    }

    void OpenGate()
    {
        if (!_isPlayerInRange) return;
    
        bool purchased = _player.SpendMoney(gateCost);

        if (purchased)
        {
            // Toca o som de portão destrancado
            if (doorUnlocked != null && _audioSource != null)
            {
                AudioSource.PlayClipAtPoint(doorUnlocked, transform.position); // Reproduz o som em um objeto temporário na posição atual
            }
        
            // Destruir o objeto pai após um pequeno atraso para garantir que o som seja tocado
            float delay = doorUnlocked != null ? doorUnlocked.length : 0f;

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject, delay);
            }
            else
            {
                Destroy(gameObject, delay);
            }
        }
        else
        {
            print("Dinheiro insuficiente para abrir portão!");
        }
    }

}
