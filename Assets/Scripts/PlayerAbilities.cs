using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Dash Settings")]
    public float dashForce = 60f;
    public float dashCooldown = 1.5f;
    private float dashTimer;
    public KeyCode dashKey = KeyCode.E;

    [Header("Slow Motion")]
    public float slowTimeScale = 0.3f;
    public KeyCode slowMoKey = KeyCode.Q;
    private float fixedDeltaTime; // Fizik hesaplamaları bozulmasın diye

    [Header("References")]
    public Transform orientation;
    public Transform playerCam; // Efekt için camera lazım
    public Camera camComponent; // FOV değiştirmek için
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        fixedDeltaTime = Time.fixedDeltaTime;
        if(playerCam != null) camComponent = playerCam.GetComponent<Camera>();
    }

    private void Update()
    {
        HandleDash();
        HandleSlowMo();
    }

    void HandleDash()
    {
        if (dashTimer > 0) dashTimer -= Time.deltaTime;

        if (Input.GetKeyDown(dashKey) && dashTimer <= 0)
        {
            Dash();
        }
    }

    void Dash()
    {
        dashTimer = dashCooldown;
        
        // Mevcut hızı sıfırla ki aniden yön değiştirebilelim
        rb.linearVelocity = Vector3.zero;

        // İleri doğru büyük bir güç uygula
        // W,A,S,D'ye göre atılsın istiyorsan:
        Vector3 dir = orientation.forward * Input.GetAxisRaw("Vertical") + orientation.right * Input.GetAxisRaw("Horizontal");
        
        // Eğer hiçbir şeye basmıyorsa ileri atıl
        if (dir.magnitude == 0) dir = orientation.forward;

        rb.AddForce(dir.normalized * dashForce, ForceMode.Impulse);

        // Efekt: FOV artsın
        if(camComponent) camComponent.fieldOfView = 80f; // Anlık artış
        Invoke("ResetFOV", 0.2f); // Hızlıca geri dön
    }

    void ResetFOV()
    {
        if(camComponent) camComponent.fieldOfView = 60f; // Normal FOV değeri neyse onu yaz
    }

    void HandleSlowMo()
    {
        if (Input.GetKey(slowMoKey))
        {
            Time.timeScale = slowTimeScale;
            Time.fixedDeltaTime = fixedDeltaTime * Time.timeScale; // Fizik motorunu yavaşlat
        }
        else
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = fixedDeltaTime;
        }
    }
}