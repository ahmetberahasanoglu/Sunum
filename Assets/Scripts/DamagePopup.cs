using UnityEngine;
using TMPro; // TextMeshPro kullanmak için şart

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro textMesh;
    public float moveSpeed = 3f; // Yukarı kayma hızı
    public float disappearSpeed = 3f; // Kaybolma hızı
    private Color textColor;

    void Awake()
    {
        textMesh = GetComponent<TextMeshPro>();
        textColor = textMesh.color;
    }

    // Bu fonksiyonu dışarıdan çağırıp hasar miktarını vereceğiz
    public void Setup(float damageAmount)
    {
        // Sayıyı metne çevir (örn: "50")
        textMesh.text = damageAmount.ToString();
        
        // Cila: Yüksek hasarda metin daha büyük görünsün
        if(damageAmount > 50) {
            transform.localScale += Vector3.one * 0.5f;
             textMesh.color = Color.red; // Kritik vuruş rengi
        }
    }

    void Update()
    {
        // 1. Yukarı doğru hareket et
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // 2. Rengini (Alpha/Şeffafllık) yavaşça azalt
        textColor.a -= disappearSpeed * Time.deltaTime;
        textMesh.color = textColor;

        // 3. Tamamen görünmez olunca objeyi yok et
        if (textColor.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}