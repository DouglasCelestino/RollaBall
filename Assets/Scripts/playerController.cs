using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
   private Rigidbody rb;
   private int count;
   [SerializeField] private string menu = "menu";
   private float movementX;
   private float movementY;
   public float speed = 0;
   public TextMeshProUGUI countText;
   public GameObject winTextObject;
   public GameObject deadTextObject;

   // Cronômetro
   private float timer = 0.0f;
   public TextMeshProUGUI timerText;
   public TextMeshProUGUI finalTimeText;

   private bool isGameOver = false;

   // Start is called before the first frame update
   void Start()
   {
      rb = GetComponent<Rigidbody>();
      count = 0;
      SetCountText();

      winTextObject.SetActive(false);
      deadTextObject.SetActive(false);
      finalTimeText.gameObject.SetActive(false);
   }

   void OnMove(InputValue movementValue)
   {
      Vector2 movementVector = movementValue.Get<Vector2>();
      movementX = movementVector.x;
      movementY = movementVector.y;
   }

   private void FixedUpdate()
   {
      if (!isGameOver)
      {
         Vector3 movement = new Vector3(movementX, 0.0f, movementY);
         rb.AddForce(movement * speed);
         UpdateTimer();
         Dead();
      }

      if (Input.GetKeyDown(KeyCode.R))
      {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
      }
      if (Input.GetKeyDown(KeyCode.M))
      {
         SceneManager.LoadScene(menu);
      }
   }

   void UpdateTimer()
   {
      // Incrementa o cronômetro
      timer += Time.deltaTime;
      timerText.text = "Time: " + timer.ToString("F2"); // Formata com 2 casas decimais
   }

   void Dead()
   {
      if (rb.position.y < -1f && !isGameOver)
      {
         rb.position = new Vector3(rb.position.x, -1f, rb.position.z);
         rb.velocity = Vector3.zero;
         rb.isKinematic = true;

         Debug.Log("Player has fallen off the platform!");

         deadTextObject.SetActive(true);
         EndGame(); // Finaliza o jogo quando o jogador morre
      }
   }

   void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("PickUp"))
      {
         other.gameObject.SetActive(false);
         count = count + 1;
         SetCountText();
      }
   }

   void SetCountText()
   {
      countText.text = "Count: " + count.ToString();

      if (count >= 12)
      {
         winTextObject.SetActive(true);
         EndGame(); // Finaliza o jogo quando o jogador vence
      }
   }

   // Método para finalizar o jogo e mostrar o tempo final
   void EndGame()
   {
      isGameOver = true; // Pausa o jogo
      finalTimeText.gameObject.SetActive(true); // Exibe o tempo final
      finalTimeText.text = "Final Time: " + timer.ToString("F2"); // Mostra o tempo final formatado
   }
}
