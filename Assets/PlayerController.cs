using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public float speed;
    float distToGround;
    private Rigidbody rb;
    public float timeStamp;
    private Vector3 startingPosition;
    public GameObject EntityPrefab;
    public Text countText;
    public Text winText;
    private int count;
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 1f);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        distToGround = gameObject.GetComponent<Collider>().bounds.extents.y;
        startingPosition = transform.position;
        count = 0;
        SetCountText();
        winText.text = "";
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
        if (timeStamp <= Time.time)
        {
            if (Input.GetButton("Jump") && IsGrounded())
            {
                rb.AddForce(new Vector3(0, 5000, 0));
                timeStamp = Time.time + 1;
            }
        }
        if (transform.position.y < 217)
        {
            transform.position = startingPosition;
            rb.position = startingPosition;
            rb.velocity = new Vector3(0f, 0f, 0f);
            rb.angularVelocity = new Vector3(0f, 0f, 0f);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            count = count + (other.transform.parent.name == "Kule" ? 2 : 1);
            GameObject particle = Instantiate(EntityPrefab ,other.transform);
            particle.transform.position = other.transform.position;
            particle.transform.SetParent(null);
            particle.SetActive(true);
            other.gameObject.SetActive(false);
            
            SetCountText();
        }
    }
    void SetCountText()
    {
        countText.text = "Count: " + count;
        if (count >= 10)
        {
            winText.text = "Wygrałeś!";
        }
    }
}