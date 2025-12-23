using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 30; // Kaç mermi versin?
    public float rotateSpeed = 50f;
    public Transform iconTransform; // Üstündeki ikon (Quad)

    void Update()
    {
        // 1. Kutuyu kendi etrafında döndür
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

        // 2. İkonu her zaman oyuncuya (kameraya) baktır - Billboard Effect
        if (Camera.main != null && iconTransform != null)
        {
            iconTransform.LookAt(Camera.main.transform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Çarpan şey oyuncu mu? (Tag kontrolü yapabilirsin veya script kontrolü)
        if (other.CompareTag("Player")) 
        {
            // Oyuncunun silahını bulmaya çalış
            // Silah genellikle Player'ın çocuk objelerindedir (InChildren)
            GameObject obj= GameObject.Find("Pistol_G");
            GunSystem gun = obj.GetComponent<GunSystem>();

            if (gun != null)
            {
                gun.AddAmmo(ammoAmount); // Mermiyi ekle
                
                // Efekt sesi çalınabilir (AudioSource.PlayClipAtPoint)
                
                Destroy(gameObject); // Kutuyu yok et
            }
        }
    }
}