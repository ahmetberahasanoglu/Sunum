using UnityEngine;
using TMPro; // UI için şart

public class GunSystem : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform attackPoint;
    public float shootForce = 300f;
    public float upwardForce = 10f;

    [Header("Gun Stats")]
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize = 30;    // Şarjör kapasitesi
    public int currentAmmo;          // Şarjördeki mermi
    public int storedAmmo = 60;      // Cepteki yedek mermi
    public bool allowButtonHold;
    
    bool shooting, readyToShoot, reloading;

    [Header("References")]
    public TextMeshProUGUI ammoText; // UI Text'i buraya sürükleyeceğiz
    public CameraShake camShake;
    public ParticleSystem muzzleFlash;
    public AudioClip shootSound;
    public AudioClip reloadSound;    // YENİ: Şarjör değiştirme sesi
    public AudioClip emptyClickSound; // YENİ: Mermi bitince çıkan "tık" sesi
    private AudioSource audioSource;

    // Recoil Variables (Aynen kalıyor)
    public float recoilRotationX = -5f; 
    public float recoilRotationY = 2f;  
    public float recoilRotationZ = 2f;
    public float kickBackZ = 0.2f; 
    public float kickUpY = 0.05f;  
    public float snappiness = 6f; 
    public float returnSpeed = 6f; 

    private Quaternion targetRotation;
    private Quaternion currentRotation;
    private Vector3 targetPosition;
    private Vector3 currentPosition;
    private Vector3 initialPosition;

    void Awake()
    {
        currentAmmo = magazineSize; // Oyuna dolu başla
        readyToShoot = true;
        initialPosition = transform.localPosition;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        
        UpdateAmmoUI(); // Başlangıçta UI'ı güncelle
    }

    void Update()
    {
        MyInput();
        HandleRecoil();
        
        // Mermi göstergesini sürekli güncelle (Garanti olsun)
        UpdateAmmoUI();
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse1);
        else shooting = Input.GetKeyDown(KeyCode.Mouse1);

        // Reload (R tuşu) - Mermi azsa ve cepte mermi varsa ve şuan reload yapmıyorsak
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < magazineSize && storedAmmo > 0 && !reloading)
        {
            Reload();
        }

        // Ateş Etme
        if (readyToShoot && shooting && !reloading)
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
            else
            {
                // Mermi yoksa "Tık" sesi çal
                readyToShoot = false;
                if(emptyClickSound) audioSource.PlayOneShot(emptyClickSound);
                Invoke("ResetShot", timeBetweenShooting);
            }
        }
    }

    private void Reload()
    {
        reloading = true;
        if(reloadSound) audioSource.PlayOneShot(reloadSound);
        
        // Reload animasyonu veya beklemesi
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        // Ne kadar mermiye ihtiyacımız var?
        int bulletsNeeded = magazineSize - currentAmmo;

        // Cebimizde yeterince var mı?
        if (storedAmmo >= bulletsNeeded)
        {
            storedAmmo -= bulletsNeeded;
            currentAmmo += bulletsNeeded;
        }
        else // Cebimizde az kaldıysa hepsini koy
        {
            currentAmmo += storedAmmo;
            storedAmmo = 0;
        }

        reloading = false;
        UpdateAmmoUI();
    }

    // --- DIŞARIDAN ÇAĞIRILACAK FONKSİYON ---
    public void AddAmmo(int amount)
    {
        storedAmmo += amount;
        UpdateAmmoUI();
        
        // Efekt veya ses eklenebilir
        Debug.Log("Mermi alındı: " + amount);
    }
    // ---------------------------------------

    private void Shoot()
    {
        readyToShoot = false;
        currentAmmo--; // Mermiyi azalt

        // Efektler & Ses
        if (muzzleFlash != null) muzzleFlash.Play();
        if (camShake != null) StartCoroutine(camShake.Shake(0.1f, 0.2f));
        if (shootSound != null) audioSource.PlayOneShot(shootSound);

        AddRecoil();

        // Mermi Oluşturma
        GameObject currentBullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);
        currentBullet.transform.forward = attackPoint.forward;
        Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
        rb.AddForce(attackPoint.forward * shootForce, ForceMode.Impulse);
        rb.AddForce(attackPoint.up * upwardForce, ForceMode.Impulse);

        Invoke("ResetShot", timeBetweenShooting);
        UpdateAmmoUI();
    }

    // Basit UI Güncelleme
    void UpdateAmmoUI()
    {
        if(ammoText != null)
            ammoText.text = currentAmmo + " / " + storedAmmo;
    }

    // (Recoil kodları aynı kaldığı için burayı kısa geçiyorum, senin eski kodundaki AddRecoil ve HandleRecoil buraya gelecek)
    private void AddRecoil()
    {
        targetRotation *= Quaternion.Euler(recoilRotationX, Random.Range(-recoilRotationY, recoilRotationY), Random.Range(-recoilRotationZ, recoilRotationZ));
        targetPosition -= new Vector3(0, kickUpY, kickBackZ); 
    }

    private void HandleRecoil()
    {
        targetRotation = Quaternion.Lerp(targetRotation, Quaternion.identity, Time.deltaTime * returnSpeed);
        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * snappiness);
        transform.localRotation = currentRotation;
        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, Time.deltaTime * returnSpeed);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * snappiness);
        transform.localPosition = initialPosition + currentPosition;
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }
}