using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {
    [Header("References")]
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator; // YENİ: Animasyon kontrolcüsü

// --- YENİ EKLENEN KISIM ---
    [Header("Effects")]
    public GameObject damagePopupPrefab; // Prefabı buraya sürükleyeceğiz
    public Transform headTransform;      // Metnin çıkacağı yer (Kafa kemiği)
    [Header("Stats")]
    public float health = 100f;
    public float attackRange = 3f; // Saldırı mesafesi
    public float timeBetweenAttacks = 2f; // Kaç saniyede bir vursun

    private bool alreadyAttacked;

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        // Eğer inspector'dan atamayı unutursan diye otomatik bulsun
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    private void Update() {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange) {
            AttackPlayer();
        } else {
            ChasePlayer();
        }
    }

    private void ChasePlayer() {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        
        // Animasyon: Koşuyor
        animator.SetBool("IsRunning", true);
    }

    private void AttackPlayer() {
        // Dur ve hedefe dön
        agent.isStopped = true;
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));

        // Animasyon: Koşmuyor
        animator.SetBool("IsRunning", false);

        if (!alreadyAttacked) {
            // Saldırı animasyonunu tetikle
            animator.SetTrigger("Attack");

            // Burada oyuncunun canını azaltma kodu (Health System) çağrılabilir
            Debug.Log("Titan Saldırdı!");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack() {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage) {
        
        health -= damage;
        ShowDamagePopup(damage);
        var renderer = GetComponentInChildren<Renderer>();
        if(renderer != null) {
             renderer.material.color = Color.red;
             Invoke("ResetColor", 0.1f);
        }
       
        if (health <= 0) Die();
    }
    void ShowDamagePopup(float damageAmount)
    {
        if (damagePopupPrefab != null && headTransform != null)
        {
            // Prefabı kafanın biraz üstünde oluştur
            Vector3 spawnPosition = headTransform.position + Vector3.up * 2f; 
            
            GameObject popup = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity);
            
            // Oluşan popup'ın scriptine ulaşıp hasar değerini ver
            popup.GetComponent<DamagePopup>().Setup(damageAmount);

            // Metin her zaman kameraya baksın (Billboard effect)
            popup.transform.LookAt(Camera.main.transform);
            popup.transform.Rotate(0, 180, 0); // LookAt ters çevirir, düzeltelim
        }
    }

    void ResetColor() {
       var renderer = GetComponentInChildren<Renderer>();
       if (renderer != null) renderer.material.color = Color.white;
    }

    private void Die() {
       agent.enabled = false;
    animator.enabled = false;
    
    // 2. Tüm kemiklerdeki 'Is Kinematic'i kapat (Fizik başlasın)
    Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
    foreach(Rigidbody rb in rbs)
    {
        rb.isKinematic = false;
        // Opsiyonel: Ölürken hafif bir itme uygula
        rb.AddForce(Vector3.back * 5f, ForceMode.Impulse);
    }
    
    // 3. Ana scripti devre dışı bırak ama objeyi YOK ETME (Ceset kalsın)
    this.enabled = false;
    
    // 5 saniye sonra cesedi temizle
    Destroy(gameObject, 1f);
    if (GameManager.Instance != null)
    {
        // Dev yere düşerken biraz beklesin sonra ekran çıksın (2 saniye delay)
        // Invoke kullandığımız için ayrı bir fonksiyon yazmamız gerekebilir veya
        // GameManager'a direkt ulaşabiliriz ama Coroutine daha şık olurdu.
        // Hızlı çözüm için direkt çağıralım:
        GameManager.Instance.Invoke("EndGame", 2f);
        //Invoke(nameof(CallWin), 2f);
        // Not: GameManager scriptindeki EndGame fonksiyonunu Invoke ile 
        // çağırabilmek için o fonksiyonun public olması yeterli.
        // Ama Invoke string ile çalışır, daha garantisi şudur:
    }
    }
    void CallWin()
    {
    GameManager.Instance.EndGame();
    }   
    
}