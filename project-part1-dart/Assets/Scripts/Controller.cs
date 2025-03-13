using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/**
 * Done by KPP (pern) in 2025!
 */

public class Controller : MonoBehaviour
{
    // references to the board and the dart rigid bodies
    private Rigidbody dart;
    private Rigidbody board;

    // the time series with the data to export
    private readonly List<TimeSeriesData> timeSeries = new();

    // flag that indicates if the dart has been launched
    private bool isLaunched = false;

    // stores the current time for exporting the time series
    private float currentTime = 0;

    // the MoveDart-script (to stop recording the time series after the collision)
    private MoveDart moveDart;

    // Input actions
    private InputAction reloadAction;
    private InputAction launchAction;
    private InputAction saveAction;

    // vars to calc the acceleration
    private Vector3 previousDartVelocity;
    private Vector3 previousBoardVelocity;


    // Start is called before the first frame update
    void Start()
    {
        // store references to the dart and board rigid bodies
        // Note: The Board tag was added to the dart board game object in Unity
        dart = GameObject.Find("SM_Prop_Dart_01").GetComponent<Rigidbody>();
        board = GameObject.FindWithTag("Board").GetComponent<Rigidbody>();
        Debug.Assert(dart != null);
        Debug.Assert(board != null);

        // Get the MoveDart component (to stop recording the time series after the collision)
        moveDart = dart.GetComponent<MoveDart>();
        Debug.Assert(moveDart != null);

        // Initialize input actions
        var playerInput = new InputActionMap("Player");

        reloadAction = playerInput.AddAction("Reload", binding: "<Keyboard>/r");
        launchAction = playerInput.AddAction("Launch", binding: "<Keyboard>/space");
        saveAction = playerInput.AddAction("Save", binding: "<Keyboard>/o");

        reloadAction.performed += ctx => ReloadScene();
        launchAction.performed += ctx => LaunchDart();
        saveAction.performed += ctx => WriteTimeSeriesToCSV();

        playerInput.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // Input actions are handled by the Input System callbacks
    }

    // FixedUpdate is called once per frame at a defined interval (typically 20 ms)
    void FixedUpdate()
    {
        // check if the dart has been launched
        // Note: OnCollision is called after FixedUpdate, so this code should also capture the last data point correctly
        if (isLaunched && !moveDart.hitBoard)
        {
            // calculate acceleration
            Vector3 dartAcceleration = (dart.linearVelocity - previousDartVelocity) / Time.fixedDeltaTime;
            Vector3 boardAcceleration = (board.linearVelocity - previousBoardVelocity) / Time.fixedDeltaTime;

            // add current data to time series
            timeSeries.Add(new TimeSeriesData
            {
                Time = currentTime,
                DartPositionY = dart.position.y,
                DartPositionZ = dart.position.z,
                BoardPositionY = board.position.y,
                BoardPositionZ = board.position.z,

                // new fields for velocity
                DartVelocityY = dart.linearVelocity.y,
                DartVelocityZ = dart.linearVelocity.z,
                BoardVelocityY = board.linearVelocity.y,
                BoardVelocityZ = board.linearVelocity.z,

                // new fields for acceleration
                DartAccelerationY = dartAcceleration.y,
                DartAccelerationZ = dartAcceleration.z,
                BoardAccelerationY = boardAcceleration.y,
                BoardAccelerationZ = boardAcceleration.z
            });

            // store the current velocity for the next frame
            previousDartVelocity = dart.linearVelocity;
            previousBoardVelocity = board.linearVelocity;

            // increase the current time step
            currentTime += Time.fixedDeltaTime;
        }
    }

    // Reload the scene
    private void ReloadScene()
    {
        // reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // reset time series
        timeSeries.Clear();
        currentTime = 0;
    }

    // Launch the dart
    private void LaunchDart()
    {
        if (!isLaunched)
        {
            // remember that the dart has been launched
            isLaunched = true;
        }
    }

    // Write the time series to a file
    void WriteTimeSeriesToCSV()
    {
        using var streamWriter = new StreamWriter("time_series.csv");
        streamWriter.WriteLine(TimeSeriesData.Header());

        foreach (TimeSeriesData timeStep in timeSeries)
        {
            streamWriter.WriteLine(timeStep.ToString());
            streamWriter.Flush();
        }

        // log
        Debug.Log("Time series data has been saved to time_series.csv");
    }
}


// Data structure to store the time series data
public class TimeSeriesData
{
    public float Time { get; set; }
    public float DartPositionY { get; set; }
    public float DartPositionZ { get; set; }
    public float BoardPositionY { get; set; }
    public float BoardPositionZ { get; set; }

    // new fields for velocity
    public float DartVelocityY { get; set; }
    public float DartVelocityZ { get; set; }
    public float BoardVelocityY { get; set; }
    public float BoardVelocityZ { get; set; }

    // new fields for acceleration
    public float DartAccelerationY { get; set; }
    public float DartAccelerationZ { get; set; }
    public float BoardAccelerationY { get; set; }
    public float BoardAccelerationZ { get; set; }

    public override string ToString()
    {
        return $"{Time},{DartPositionY},{DartPositionZ},{BoardPositionY},{BoardPositionZ}," +
               $"{DartVelocityY},{DartVelocityZ},{BoardVelocityY},{BoardVelocityZ}," +
               $"{DartAccelerationY},{DartAccelerationZ},{BoardAccelerationY},{BoardAccelerationZ}";
    }

    public static string Header()
    {
        return "t,y_Dart,z_Dart,y_Board,z_Board," +
               "DartVelY,DartVelZ,BoardVelY,BoardVelZ," +
               "DartAccY,DartAccZ,BoardAccY,BoardAccZ";
    }
}
