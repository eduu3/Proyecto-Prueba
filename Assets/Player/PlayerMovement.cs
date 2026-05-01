using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CreatureData playerData;

    float speed;
    bool canMove = false;
    bool canMoveCenter = false;

    private void Start()
    {
        speed = playerData.speed;
        GameManager.OnStart += HandleMovement;  //Cuando se lanza el evento de UIManager OnGameStart se permite el movimiento
        GameManager.OnRestartGame += HandleMovement;
        GameManager.OnGameOver += HandleMovement;
    }
    private void OnDestroy()
    {
        GameManager.OnStart -= HandleMovement;
        GameManager.OnRestartGame -= HandleMovement;
        GameManager.OnGameOver -= HandleMovement;
    }
    void Update()
    {
        transform.position = CheckBounds.CheckMarges(transform.position);

        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        Vector3 mov = new Vector3(xAxis * Time.deltaTime * speed, yAxis * Time.deltaTime * speed); 

        if (canMove) transform.Translate(mov); //Aqui se permite

        if (canMoveCenter) transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, speed * Time.deltaTime);

    }
    public void HandleMovement(GameState currentState)
    {
        if (currentState == GameState.Playing)
        {
            canMove = true; //Aqui se inicia
            canMoveCenter = false;
        } else if (currentState == GameState.GameOver)
        {
            canMove = false;
        } else if (currentState == GameState.WaitingToStart)
        {
            canMoveCenter = true;
        }
    }
}
