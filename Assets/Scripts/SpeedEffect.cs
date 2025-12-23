using UnityEngine;

public class SpeedEffect : MonoBehaviour
{
    [Header("References")]
    public Rigidbody playerRb; // Oyuncunun hızı lazım
    private ParticleSystem speedLinesPS;
    private ParticleSystem.EmissionModule emissionModule;

    [Header("Settings")]
    public float minSpeedToStart = 15f; // Efekt hangi hızda başlasın?
    public float maxSpeedForFullEffect = 40f; // Efekt hangi hızda tavan yapsın?
    public float maxEmissionRate = 50f; // Tavandayken saniyede kaç çizgi çıksın?

    void Start()
    {
        speedLinesPS = GetComponent<ParticleSystem>();
        // Emission modülüne erişim farklıdır, bu şekilde alınır:
        emissionModule = speedLinesPS.emission;

        // Eğer playerRb atamayı unuttuysan otomatik bulmaya çalış (Opsiyonel)
        if (playerRb == null)
            playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (playerRb == null) return;

        // Oyuncunun şu anki hızı (Magnitude vektörün uzunluğunu yani hızı verir)
        float currentSpeed = playerRb.linearVelocity.magnitude;

        // Hızı 0 ile 1 arasında bir orana dönüştür (InverseLerp)
        // Eğer hız 15'ten küçükse 0, 40'tan büyükse 1, arasındaysa 0.5 gibi bir değer döner.
        float speedRatio = Mathf.InverseLerp(minSpeedToStart, maxSpeedForFullEffect, currentSpeed);

        // Oranı maksimum parçacık sayısıyla çarp
        emissionModule.rateOverTime = speedRatio * maxEmissionRate;
    }
}