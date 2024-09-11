using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
   // Rigidbody of the player.
   private Rigidbody rb;

   // Variable to keep track of collected "PickUp" objects.
   private int count;

   [SerializeField] private string menu = "menu";

   // Movement along X and Y axes.
   private float movementX;
   private float movementY;

   // Speed at which the player moves.
   public float speed = 0;

   // UI text component to display count of "PickUp" objects collected.
   public TextMeshProUGUI countText;

   public GameObject winTextObject;

   public GameObject deadTextObject;

   // Start is called before the first frame update.
   void Start()
   {
      // Get and store the Rigidbody component attached to the player.
      rb = GetComponent<Rigidbody>();

      // Initialize count to zero.
      count = 0;

      // Update the count display.
      SetCountText();

      winTextObject.SetActive(false);
      deadTextObject.SetActive(false);
   }

   // This function is called when a move input is detected.
   void OnMove(InputValue movementValue)
   {
      // Convert the input value into a Vector2 for movement.
      Vector2 movementVector = movementValue.Get<Vector2>();

      // Store the X and Y components of the movement.
      movementX = movementVector.x;
      movementY = movementVector.y;
   }

   // FixedUpdate is called once per fixed frame-rate frame.
   private void FixedUpdate()
   {
      // Create a 3D movement vector using the X and Y inputs.
      Vector3 movement = new Vector3(movementX, 0.0f, movementY);

      // Apply force to the Rigidbody to move the player.
      rb.AddForce(movement * speed);

      // Check if the player has "died" (fallen off the platform)
      Dead();

      if(Input.GetKeyDown(KeyCode.R))
      {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      }
      if(Input.GetKeyDown(KeyCode.M))
      {
         SceneManager.LoadScene(menu);
      }
}

   // Function to check if the player has fallen off the platform and handle death
   void Dead()
   {
      if (rb.position.y < -1f)
      {
         // Set the position.y to -1f forever 
         rb.position = new Vector3(rb.position.x, -1f, rb.position.z);

         // Stop the player's movement and disable physics
         rb.velocity = Vector3.zero;   // Stop any current movement
         rb.isKinematic = true;        // Disable further physics
         
         // Print a debug log to track the death event
         Debug.Log("Player has fallen off the platform!");

         deadTextObject.SetActive(true);
      }
   }

   void OnTriggerEnter(Collider other)
   {
      // Check if the object the player collided with has the "PickUp" tag.
      if (other.gameObject.CompareTag("PickUp"))
      {
         // Deactivate the collided object (making it disappear).
         other.gameObject.SetActive(false);

         // Increment the count of "PickUp" objects collected.
         count = count + 1;

         // Update the count display.
         SetCountText();
      }
   }

   // Function to update the displayed count of "PickUp" objects collected.
   void SetCountText()
   {
      // Update the count text with the current count.
      countText.text = "Count: " + count.ToString();

      if (count >= 12)
      {
         winTextObject.SetActive(false);
      }
   }
}
