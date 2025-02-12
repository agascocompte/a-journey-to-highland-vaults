using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movement_speed = 6f;
    public float shootDelay = 0.5f;
    public AudioClip moveClip;

    private AudioSource audio;
    private bool moving = false;

    Vector3 movement;
    Vector3 turn;
    Animator anim;
    Rigidbody playerRigidbody;
    bool shooting = false;
    float shootingTime = 0f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        CheckShooting();
        Move(h, v);
        Turning(h, v);
        Animating(h, v);
        
    }

    void Move(float h, float v)
    {
        if ((h != 0 || v != 0) && !shooting)
        {
            moving = true;
            movement.Set(h, 0f, v);

            movement = movement.normalized * movement_speed * Time.deltaTime;

            playerRigidbody.MovePosition(transform.position + movement);

            if (!audio.isPlaying)
            {
                audio.PlayOneShot(moveClip, 0.7f);
            }            
        }
        else
        {
            if (moving == true)
            {
                audio.Stop();
            }
            moving = false;
        }
    }


    void Turning(float h, float v)
    {
        if ((h != 0 || v != 0) && !shooting)
        {
            turn.Set(h, 0f, v);
            
            Quaternion newRotation = Quaternion.LookRotation(turn);

            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating(float h, float v)
    {
        bool running = (h != 0 || v != 0) && !shooting;
        anim.SetBool("IsRunning", running);
    }

    void CheckShooting()
    {
        if (shooting)
        {
            shootingTime += Time.deltaTime;
            if (shootingTime >= shootDelay)
            {
                shooting = false;
            }
        }
    }

    public void Shoot(Quaternion newRotation)
    {
        // Set the player's rotation to this new rotation.            
        playerRigidbody.MoveRotation(newRotation);
        shootingTime = 0f;
        shooting = true;
    }
}
