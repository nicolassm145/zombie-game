using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class AllActivators : MonoBehaviour
{
    public int ActivatedNum { get; private set; } = 0;

    [SerializeField] private Light2D globalLight; // Referência à Global Light 2D
    [SerializeField] private float targetIntensity = 1.5f; // Intensidade desejada
    [SerializeField] private float increaseSpeed = 0.1f; // Velocidade do aumento da intensidade

    private void Start()
    {
        if (globalLight == null)
        {
            Debug.LogError("Global Light 2D não está atribuída!");
        }
    }

    public void ActivateOne()
    {
        ActivatedNum++;

        if (ActivatedNum == 3)
        {
            ActivateLight();
        }
    }

    private void ActivateLight()
    {
        if (globalLight == null) return;

        // Inicia a corrotina para aumentar a intensidade gradualmente
        StartCoroutine(IncreaseLightIntensity());
    }

    private IEnumerator IncreaseLightIntensity()
    {
        while (globalLight.intensity < targetIntensity)
        {
            globalLight.intensity += increaseSpeed * Time.deltaTime;
            yield return null; // Espera até o próximo frame
        }
        
        globalLight.intensity = targetIntensity; // Garante que alcance o valor exato
    }
}