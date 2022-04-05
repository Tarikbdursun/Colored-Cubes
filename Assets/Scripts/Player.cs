using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform sideMovementRoot;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;
    [SerializeField] private Cube baseCube;
    [SerializeField] private float sideMovementSensitivity;
    [SerializeField] private float sideMovementLerpSpeed;
    [SerializeField] private float forwardSpeed;
    [SerializeField] private ParticleSystem confetti;
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private TextMeshProUGUI winPointText;

    public bool end;

    private Vector2 inputDrag;
    private Vector2 previousMousePosition;
    public List<Cube> CubesList = new List<Cube>();
    private float leftLimitX => leftLimit.localPosition.x;
    private float rightLimitX => rightLimit.localPosition.x;

    private float sideMovementTarget = 0;
    private Vector2 mousePositionCM // Providing the same experience to everyone
    {
        get
        {
            Vector2 pixels = Input.mousePosition;
            var inches = pixels / Screen.dpi;
            var centimetres = inches * 2.54f; // 1 inch = 2.54 cm

            return centimetres;
        }
    }
    #endregion
    #region SINGLETON
    private static Player instance;
    public static Player Instance => instance ??= FindObjectOfType<Player>();
    #endregion
    private void Start()
    {
        pointText.text = CubesList.Count.ToString();
        PlayerReset();
    }
    void Update()
    {
        HandleInput();
        if (GameManager.Instance.isGameStarted && !end)
        {
            HandleForwardMovement();
            HandleSideMovement();
        }
    }

    private void HandleForwardMovement()
    {
        transform.position += transform.forward * Time.deltaTime * forwardSpeed;
    }

    private void HandleSideMovement()
    {
        sideMovementTarget += inputDrag.x * sideMovementSensitivity;
        sideMovementTarget = Mathf.Clamp(sideMovementTarget, leftLimitX, rightLimitX);

        var localPos = sideMovementRoot.localPosition;

        localPos.x = Mathf.Lerp(localPos.x, sideMovementTarget, Time.deltaTime * sideMovementLerpSpeed);

        sideMovementRoot.localPosition = localPos;
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = mousePositionCM;
        }
        if (Input.GetMouseButton(0))
        {
            var deltaMouse = (Vector2)mousePositionCM - previousMousePosition;
            inputDrag = deltaMouse;
            previousMousePosition = mousePositionCM;
        }
        else
        {
            inputDrag = Vector2.zero;
        }
    }

    private bool CheckCubeCount()
    {
        if (CubesList.Count == 0)
        {
            GameEnding(false);
            return true;
        }
        return false;
    }
    public void AddCube(Cube go)
    {
        go.tag = "Untagged";
        go.transform.position = CubesList[CubesList.Count - 1].transform.position + Vector3.up;
        go.transform.parent = sideMovementRoot;
        go.GetComponent<Cube>().isStacked = true;
        CubesList.Add(go);
        pointText.text = CubesList.Count.ToString();
    }
    public void RemoveCube()
    {
        CubesList[0].transform.parent = LevelManager.GetCurrentLevel().transform;
        CubesList.RemoveAt(0);
        pointText.text = CubesList.Count.ToString();
        CheckCubeCount();
    }

    public void ChangeColorType(GameObject go) 
    {
        for (int i = 0; i < CubesList.Count; i++)
        {
            CubesList[i].colorType = go.GetComponent<Gate>().colorType;
            CubesList[i].gameObject.GetComponent<Renderer>().material.color= go.GetComponent<Renderer>().material.color;
        }
    }

    public void GameEnding(bool won)
    {
        end = true;
        if (won)
        {
            confetti.Play();
            winPointText.text = CubesList.Count.ToString();
            GameManager.Instance.GameEnd(won);
        }
        else
        {
            GameManager.Instance.GameEnd(won);
        }
    }
    public void PlayerReset() 
    {
        transform.position = new Vector3(0, 1, 5);
        sideMovementRoot.localPosition = Vector3.zero;
        for (int i = 0; i < CubesList.Count; i++)
        {
            CubesList[i].transform.parent = LevelManager.GetCurrentLevel().transform;
            Destroy(CubesList[i].gameObject);
        }
        CubesList.Clear();
        var go = Instantiate(baseCube, sideMovementRoot);
        go.transform.localPosition = Vector3.zero;
        CubesList.Add(go);
        end = false;
        confetti.Stop();
        pointText.text = "0";
    }
}
