
using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Linq;


public class Movement : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] MovementData paintMovementData;

    Vector3 lastMP;
    private float currentSpeed;
    IMovement currentMovement;
    float startY;
    private float sensitivity;

    IEnumerator Start()
    {
        yield return null;
        speed = GameManager.Instance.gameData.playerSpeed;
        currentSpeed = speed;
        startY = transform.position.y;
        float width = GameManager.Instance.gameData.roadWidth - 1f;
        currentMovement = new RunnerMovement(transform) { minX = -width / 2f, maxX = width / 2f };
        GameManager.Instance.onPhaseChange += HandlePhaseChange;
        sensitivity = GameManager.Instance.gameData.runnerSensitivity;

    }

    private void HandlePhaseChange(GameManager.GamePhase phase)
    {

        switch (phase)
        {
            case GameManager.GamePhase.Runner:
                float width = GameManager.Instance.gameData.roadWidth - 1f;
                currentMovement = new RunnerMovement(transform) { minX = -width / 2f, maxX = width / 2f };
                transform.position = new Vector3(transform.position.x, startY, transform.position.z + 16f);
                break;
            case GameManager.GamePhase.Paint:
                width = GameManager.Instance.gameData.roadWidth - 1f;
                var car = FindObjectsOfType<Car>()
                    .OrderBy(e => (transform.position - e.transform.position).sqrMagnitude)
                    .FirstOrDefault();
                currentMovement = new PainterMovement(transform)
                {
                    carBaseTf = car.transform.parent,
                    speed = paintMovementData.moveSpeed,
                    minXY = new Vector2(-width / 2f, -1.5f),
                    maxXY = new Vector2(width / 2f, width / 2f)
                };
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if (!GameManager.Instance.isPlaying) return;
        Vector3 input = MouseInput();

        currentMovement.Tick(input);
    }

    private Vector3 MouseInput()
    {
        var input = new Vector3(0, 0, currentSpeed);
        if (Input.GetMouseButtonDown(0))
        {
            lastMP = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            var delta = (Input.mousePosition - lastMP) / Time.deltaTime;
            input.x = delta.x / Screen.width;
            input.y = delta.y / Screen.width;
            lastMP = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            input.x = 0;
            input.y = 0;
        }
        return input;
    }


    public void KnockBack(float amount)
    {
        currentSpeed -= amount;
        DOTween.To(() => currentSpeed, (val) => currentSpeed = val, speed, 0.5f);
    }
    public void Slow(float amount)
    {
        currentSpeed = speed * amount;
    }
    public void Reset()
    {
        currentSpeed = speed;
    }
}
public class RunnerMovement : IMovement
{
    private Transform transform;
    public float minX;
    public float maxX;
    public float sensitivity;
    public RunnerMovement(Transform transform)
    {
        this.transform = transform;
        sensitivity = GameManager.Instance.gameData.runnerSensitivity;
    }
    public void Tick(Vector3 input)
    {
        var target = transform.position + new Vector3(input.x * sensitivity, 0, input.z) * Time.deltaTime;
        target.x = Mathf.Clamp(target.x, minX, maxX);
        transform.position = target;
    }
}
public class PainterMovement : IMovement
{
    public Transform carBaseTf;
    public Transform transform;
    public float speed;
    public Vector2 minXY;
    public Vector2 maxXY;
    
    public PainterMovement(Transform transform)
    {
        this.transform = transform;
        speed = GameManager.Instance.gameData.sprayGunSpeed;
    }
    public void Tick(Vector3 input)
    {
        // carBaseTf.rotation = Quaternion.Euler(0, -input.x * Time.deltaTime * 20, 0) * carBaseTf.rotation;
        // transform.position += new Vector3(input.x, input.y, 0);
        var target = transform.position + new Vector3(input.x, input.y, 0) * speed * Time.deltaTime;
        target.x = Mathf.Clamp(target.x, minXY.x, maxXY.x);
        target.y = Mathf.Clamp(target.y, minXY.y, maxXY.y);
        transform.position = target;
    }
}
