using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonGameobject<GameManager>
{
    [Header("Set objects")]
    [SerializeField] private Transform startFigure;
    [SerializeField] private Transform placementFigures;
    [SerializeField] private GameObject cubePrefab;

    [Header("Lists of cubes")]
    [SerializeField] private List<Transform> cubes = new List<Transform>();
    [SerializeField] private List<GameObject> fallingCubes = new List<GameObject>();

    [Header("Speed of prefab")]
    [SerializeField] private float speed = 2f;

    [Header("Gradient")]
    [SerializeField] private float currentGradientStep = 0f;
    [SerializeField] private Gradient gradient = new Gradient();

    [SerializeField] private Transform tempTransforms;

    private CalculateTransoftms calculateTransoftm;

    private List<Vector3> startPositions;
    private GameObject currentFigure;    
    private float currentYPosition = 0f;
    private IEnumerator movementCoroutine;
    private int lastRandom = 0;

    private Transform newPlacedTransform;
    private Transform newFallingTransform;

    #region States
    private StateMachine stateMachine;
    private MenuState menuState;    
    private GameState gameState;    
    private WaitingState waitingState;    
    private DefeatState defeatState;
    #endregion


    #region Properties
    public MenuState MenuState => menuState;
    public GameState GameState => gameState;
    public WaitingState WaitingState => waitingState;
    public DefeatState DefeatState => defeatState;
    public Vector3 targerPos => currentFigure.transform.position;
    #endregion

    public event Action OnStartMenuShowed;
    public Action OnGameStart;
    public event Action OnGameState;
    public event Action OnFigurePlaced;
    public Action OnGameResetFromBegining;
    public event Action OnGameResetFromLastPoint;
    public event Action OnGameFailed;
    public event Action OnFinalResultShowed;
    public Action OnLevelUp;

    private void Awake()
    {
        Instance = this;
        stateMachine = new StateMachine();
        menuState = new MenuState(this, stateMachine);
        gameState = new GameState(this, stateMachine);
        waitingState = new WaitingState(this, stateMachine);
        defeatState = new DefeatState(this, stateMachine);
        stateMachine.Initialize(waitingState);

        startPositions = new List<Vector3>(8);
        for (int i = 0; i < startPositions.Capacity; i++)
        {
            startPositions.Add(Vector3.zero);
        }

        calculateTransoftm = new CalculateTransoftms();
        calculateTransoftm.CreateTempTransform().SetParent(tempTransforms);

        CalculateStartPositions();
        lastRandom = UnityEngine.Random.Range(0, 4);

        GameObject temp = new GameObject();
        newPlacedTransform = temp.GetComponent<Transform>();
        GameObject temp1 = new GameObject();
        newFallingTransform = temp1.GetComponent<Transform>();

        newPlacedTransform.SetParent(tempTransforms);
        newFallingTransform.SetParent(tempTransforms);
    }

    private void OnEnable()
    {
        OnStartMenuShowed += StartMenuEffectShowed;
        OnGameResetFromBegining += ResetGameFromStartPosition;
        OnGameResetFromBegining += ResetGameStartPlay;
    }

    private void Start()
    {
        OnStartMenuShowed?.Invoke();
        gradient = ColorInterpolation.Instance.GetPalitre();
        cubes[0].GetComponent<Renderer>().material.color = gradient.Evaluate(currentGradientStep);
        currentGradientStep += 0.1f;
        cubes[1].GetComponent<Renderer>().material.color = gradient.Evaluate(currentGradientStep);
        currentGradientStep += 0.1f;
    }   

    private void Update()
    {
        stateMachine.CurrnetState.InputUpdate();
    }

    public void InstantiateFigure()
    {
        lastRandom = GetRandomIndex();
        currentFigure = GetFigurePrefab();
        Vector3 from = startPositions[lastRandom];
        Vector3 to = startPositions[lastRandom + 4];
        currentFigure.transform.position = from;
        movementCoroutine = MoveFigureRoute(currentFigure.transform, from, to, speed);
        StartCoroutine(movementCoroutine);
    }

    private void ResetGameStartPlay()
    {
        currentYPosition = 0f;
        CalculateStartPositions();

        gradient = ColorInterpolation.Instance.GetPalitre();

        cubes[0].GetComponent<Renderer>().material.color = gradient.Evaluate(currentGradientStep);
        currentGradientStep += 0.1f;
        cubes[1].GetComponent<Renderer>().material.color = gradient.Evaluate(currentGradientStep);

        stateMachine.Initialize(gameState);
    }

    public void CheckMatches()
    {
        StopCoroutine(movementCoroutine);

        calculateTransoftm.Init(cubes[cubes.Count - 1], currentFigure.transform);

        if (calculateTransoftm.IsPlayerMissed())
        {
            GameOver();
        }
        else
        {
            OnFigurePlaced?.Invoke();
            
            
            
            if(calculateTransoftm.IsPerfectPlacement())
            {
                Debug.Log("Perfect");
                currentFigure.transform.position = new Vector3(cubes[cubes.Count - 1].position.x, currentFigure.transform.position.y, cubes[cubes.Count - 1].position.z);
                cubes.Add(currentFigure.transform);
                OnLevelUp?.Invoke();
                CalculateStartPositions();
                InstantiateFigure();
                return;
            }            

            if (lastRandom > 1) // 2||3 index is west or east start position
            {
                newPlacedTransform = calculateTransoftm.CalculateXAxisPlacementPositions();
                newFallingTransform = calculateTransoftm.CalculateXAxisFallingPosition();
            }
            else
            {
                //0||1 index is north or south startposition
                newPlacedTransform = calculateTransoftm.CalculateZAxisPlacementPosition();
                newFallingTransform = calculateTransoftm.CalculateZAxisFallingPosition();
            }

            SetPlacedFigure();
            SetFallingFigure();

            cubes.Add(currentFigure.transform);
            OnLevelUp?.Invoke();
            CalculateStartPositions();
            InstantiateFigure();
        }
    }

    private void SetPlacedFigure()
    {
        currentFigure.transform.position = new Vector3(newPlacedTransform.position.x, currentFigure.transform.position.y, newPlacedTransform.position.z);
        currentFigure.transform.localScale = new Vector3(newPlacedTransform.localScale.x, 1f, newPlacedTransform.localScale.z);
        currentFigure.isStatic = true;
    }

    private void SetFallingFigure()
    {
        GameObject go = Instantiate(currentFigure, new Vector3(100f, 0f, 0f), Quaternion.identity);
        go.transform.position = new Vector3(newFallingTransform.position.x, currentFigure.transform.position.y, newFallingTransform.position.z);
        go.transform.localScale = new Vector3(newFallingTransform.localScale.x, 1f, newFallingTransform.localScale.z);

        Rigidbody rig = go.GetComponent<Rigidbody>();
        rig.isKinematic = false;
        rig.useGravity = true;
        fallingCubes.Add(go);
    }

    private void GameOver()
    {
        stateMachine.ChangeState(defeatState);
        Rigidbody rig = currentFigure.GetComponent<Rigidbody>();
        rig.isKinematic = false;
        rig.useGravity = true;
        OnGameFailed?.Invoke();
    }

    private int GetRandomIndex()
    {
        int random = UnityEngine.Random.Range(0, 4);

        if (lastRandom == random)
        {
            while (lastRandom == random)
            {
                random = UnityEngine.Random.Range(0, 4);
            }
        }

        lastRandom = random;
        return random;
    }

    private void ResetGameFromStartPosition()
    {
        for (int i = 2; i < cubes.Count - 1; i++)
        {
            Destroy(cubes[i].gameObject);
        }

        cubes.RemoveRange(2, cubes.Count - 2);

        //страховка
        int count = placementFigures.childCount;

        for (int i = count - 1; i > -1; i--)
        {
            Destroy(placementFigures.GetChild(i).gameObject);
        }

        
        for (int i = 0; i < fallingCubes.Count - 1; i++)
        {
            Destroy(fallingCubes[i]);
        }

        foreach (var item in fallingCubes)
        {
            if(item != null) Destroy(item);
        }

        fallingCubes.Clear();
    }

    private GameObject GetFigurePrefab()
    {
        GameObject figure = Instantiate(cubePrefab);
        figure.transform.SetParent(placementFigures);
        figure.GetComponent<Renderer>().material.color = gradient.Evaluate(currentGradientStep);
        currentGradientStep += 0.1f;

        if(currentGradientStep >= 0.95f)
        {
            gradient = null;
            gradient = ColorInterpolation.Instance.GetPalitre();
            currentGradientStep = 0f;
        }

        var index = cubes.Count - 1;

        figure.transform.localScale = new Vector3(cubes[index].localScale.x, 1f, cubes[index].localScale.z);
        currentYPosition++;

        return figure;
    }

    private void CalculateStartPositions()
    {
        Vector3 centerPosition = cubes[cubes.Count - 1].position;

        Vector3 northPosition = new Vector3(centerPosition.x, currentYPosition, 5f);
        Vector3 southPosition = new Vector3(centerPosition.x, currentYPosition, -5f);

        startPositions[0] = northPosition;
        startPositions[4] = southPosition;
        startPositions[1] = southPosition;
        startPositions[5] = northPosition;

        Vector3 eastPosition = new Vector3(5f, currentYPosition, centerPosition.z);
        Vector3 westPosition = new Vector3(-5f, currentYPosition, centerPosition.z);

        startPositions[2] = eastPosition;
        startPositions[6] = westPosition;
        startPositions[3] = westPosition;
        startPositions[7] = eastPosition;
    }

    private void StartMenuEffectShowed()
    {
        StartCoroutine(MoveStartPlatform(startFigure, 0.7f, 2f, Vector3.zero));
    }

    public void RemoveThisFallingFigure(GameObject obj)
    {
        fallingCubes.RemoveAt(fallingCubes.IndexOf(obj));   
    }

    private IEnumerator MoveFigureRoute(Transform go, Vector3 from, Vector3 to, float time)
    {
        float startTime = Time.time;
        float timer = startTime + time;

        while(Time.time < timer)
        {
            var u = (Time.time - startTime) / time;
            go.position = Vector3.Lerp(from, to, u);
            yield return null;
        }

        go.position = to;

        movementCoroutine = null;
        movementCoroutine = MoveFigureRoute(go.transform, to, from, speed);
        StartCoroutine(movementCoroutine);
    }

    private IEnumerator MoveStartPlatform(Transform gObj, float timeDuration, float delayBeforeStart, Vector3 position)
    {
        yield return new WaitForSeconds(delayBeforeStart);

        var startTime = Time.time;
        var timer = startTime + timeDuration;

        Vector3 startPosition = gObj.position;
        Vector3 nextPosition = Vector3.zero;

        while (Time.time < timer)
        {
            float u = (Time.time - startTime) / timeDuration;
            gObj.position = Vector3.Lerp(startPosition, nextPosition, u);
            yield return null;
        }

        gObj.position = nextPosition;

        StartCoroutine(ChangeState(menuState, 0.7f));
    }

    private IEnumerator ChangeState(State state, float delay)
    {
        yield return new WaitForSeconds(delay);
        stateMachine.ChangeState(state);
    }

    private void ReplaceStartFigure()
    {
        startFigure.position = Vector3.zero;
    }


}
