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
   public AudioClip pickupSFX;
   public AudioController AudioController;

   // Cronômetro
   private float timer = 0.0f;
   public TextMeshProUGUI timerText;
   public TextMeshProUGUI finalTimeText;
   public TextMeshProUGUI collectPickup;

   public TextMeshProUGUI collidedEnemyText;
   private const float pickupTime = 1.5f;
   private const float EnemyTime = 2.0f;
   private const int CollectAll = 14;

   // Start is called before the first frame update
   private bool isGameOver = false;
   void Start()
   {
      rb = GetComponent<Rigidbody>();
      count = 0;
      SetCountText();

      winTextObject.SetActive(false);
      deadTextObject.SetActive(false);
      finalTimeText.gameObject.SetActive(false);
      collectPickup.gameObject.SetActive(false);
      collidedEnemyText.gameObject.SetActive(false);
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
      timerText.text = "Time: " + timer.ToString("F1"); // Formata com 2 casas decimais
   }

   void Dead()
   {
      if (rb.position.y < -1f && !isGameOver)
      {
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
         AudioController.PlaySFX(pickupSFX);
         //minus 1.25 seconds for each pickup
         timer -= pickupTime;
         collectPickup.text = "-" + pickupTime.ToString() + "s";
         //Show the text for 1 second
         StartCoroutine(FadeTextEffect(0.0f, 0.0f, 1.0f, collectPickup));
      }
   }

   void OnCollisionEnter(Collision collision)
{
    // Verifica se o jogador colidiu com um objeto que possui a tag "Enemy"
    if (collision.gameObject.CompareTag("Enemy"))
    {
        // Aumenta o tempo em x segundos
        timer += EnemyTime;

        // Atualiza o texto do cronômetro
        timerText.text = "Time: " + timer.ToString("F1");
        
        // (Opcional) Exibe uma mensagem na tela para indicar que o tempo aumentou
        collidedEnemyText.text = "+" + EnemyTime.ToString() + "s";
        StartCoroutine(FadeTextEffect(0.0f, 0.0f, 1.5f, collidedEnemyText));
    }
}


   IEnumerator FadeTextEffect(float startAlpha, float endAlpha, float duration, TextMeshProUGUI text)
{
    float elapsedTime = 0;
    Color tempColor = text.color;
    tempColor.a = startAlpha;
    text.color = tempColor; // Começa com o alpha inicial
    text.gameObject.SetActive(true);

    // Fade in (suavemente aumentando a opacidade)
    while (elapsedTime < duration / 2)
    {
        elapsedTime += Time.deltaTime;
        tempColor.a = Mathf.Lerp(startAlpha, 1.0f, elapsedTime / (duration / 2)); // Interpola do startAlpha para 1.0
        text.color = tempColor;
        yield return null;
    }

    // Mantém o texto visível por 1 segundo
    yield return new WaitForSeconds(1f);

    elapsedTime = 0;

    // Fade out (suavemente diminuindo a opacidade)
    while (elapsedTime < duration / 2)
    {
        elapsedTime += Time.deltaTime;
        tempColor.a = Mathf.Lerp(1.0f, endAlpha, elapsedTime / (duration / 2)); // Interpola de 1.0 para o endAlpha
        text.color = tempColor;
        yield return null;
    }

    // Esconde o texto após o fade out
    text.gameObject.SetActive(false);
}


   void SetCountText()
   {
      countText.text = "Count: " + count.ToString();

      if (count >= CollectAll)
      {
         winTextObject.SetActive(true);
         EndGame(); // Finaliza o jogo quando o jogador vence
      }
   }

   // Método para finalizar o jogo e mostrar o tempo final
   void EndGame()
   {
      isGameOver = true; // Pausa o jogo
      rb.position = new Vector3(rb.position.x, -1f, rb.position.z);
      rb.velocity = Vector3.zero;
      rb.isKinematic = true;
      finalTimeText.gameObject.SetActive(true); // Exibe o tempo final
      finalTimeText.text = "Final Time: " + timer.ToString("F2"); // Mostra o tempo final formatado
   }
}
