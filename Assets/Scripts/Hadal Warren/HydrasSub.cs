using UnityEngine;

public class HydrasSub : PlayerBase
{
    public HydrasGame game;
    public CharacterController controller;
    public Camera cam;
    public Light subLight;

    public int health = 3,
           maxHealth = 3,
           deathCount = 0,
           score = 0,
           highScore = 0;

    public float speed = 3f, turnSpeed = 3f;
    public bool collect = false, hit = false;

    // Start is called before the first frame update
    void Start()
    {
        game.UpdateHealthUI(health);
    }
    // Update is called once per frame
    void Update()
    {
        if (!game.paused)
        {
            // perspective toggle
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (cam.transform.localPosition == Vector3.zero)
                {
                    cam.transform.localPosition = new Vector3(0f, 10f, 0f);
                    cam.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                } else
                {
                    cam.transform.localPosition = Vector3.zero;
                    cam.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                }
            }
            // light toggle
            if (Input.GetKeyDown(KeyCode.Space))
            {
                subLight.enabled = !subLight.enabled;
            }
            // speed boost
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                {
                    speed *= 2;
                    turnSpeed *= 2;
                }
                if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
                {
                    speed /= 2;
                    turnSpeed /= 2;
                }
            }
            // turning
            if (Input.GetAxisRaw("Horizontal") !=0)
            {
                float horizontal = Input.GetAxisRaw("Horizontal");

                float angle = horizontal * turnSpeed;
                //game.UpdateCompassUI(angle);
                angle += transform.eulerAngles.y;
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

            }

            // moving
            if (Input.GetAxisRaw("Vertical") != 0)
            {

                float vertical = Input.GetAxisRaw("Vertical");

                Vector3 moveDirection = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward * vertical;
                controller.Move(speed * Time.deltaTime * moveDirection.normalized);
            }

        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (subLight.enabled == true)
        {
            if (other.gameObject.CompareTag("Treasure"))
            {
                other.gameObject.transform.position = Vector3.zero;
                collect = true;
            }
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<WanderingMine>().moving = false;
                other.gameObject.transform.position = Vector3.zero;
                hit = true;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (subLight.enabled == true)
        {
            if (other.gameObject.CompareTag("Treasure"))
            {
                other.gameObject.transform.position = Vector3.zero;
                collect = true;
            }
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.transform.position = Vector3.zero;
                other.gameObject.GetComponent<WanderingMine>().moving = false;
                hit = true;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Treasure") && collect == true)
        {
            collect = false;
            score += 10;

        }
        if (other.gameObject.CompareTag("Enemy") && hit == true)
        {
            hit = false;
            if (health > 1)
            {
                health--;
            }
            else
            {
                GameOver();
            }
            game.UpdateHealthUI(health);
        }
        game.UpdateScoreUI(score);
    }

    void GameOver()
    {
        controller.enabled = false;
        deathCount++;
        if (score > highScore)
        {
            highScore = score;
        }
        score = 0;
        game.SpawnPlayer();
        health = maxHealth;
        controller.enabled = true;
    }
}
