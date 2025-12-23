using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject explosionPrefab; // Patlama efekti
    public float damage = 25f;
    public bool isEnemy = false; // İlerde düşman da bize sıkarsa diye
    public float impactForce = 50f;

    private void OnCollisionEnter(Collision collision)
    {
       if (collision.gameObject.TryGetComponent<Hitbox>(out Hitbox hitbox))
        {
            hitbox.OnHit(damage);

            // --- EKSİK OLAN PARÇA BURASIYDI ---
            // Vurduğumuz uzva (kol, bacak vs.) fiziksel güç uygula
            if (collision.rigidbody != null)
            {
                // -normal diyerek çarpma yönünün tersine (yani merminin gidiş yönüne) itiyoruz
                collision.rigidbody.AddForce(-collision.contacts[0].normal * impactForce, ForceMode.Impulse);
            }
        }

        // 2. Patlama Efektini Oluştur
        if (explosionPrefab != null)
        {
            // Patlamayı tam çarpışma noktasında (Contact Point) oluştur
            Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);
        }

        // 3. Mermiyi Yok Et
        Destroy(gameObject);
    
    }
}