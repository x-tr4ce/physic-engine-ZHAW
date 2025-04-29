using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/**
 * Done by KPP (pern) in 2025!
 */

public class Car : MonoBehaviour
{
    // the rigid body of the car
    private Rigidbody rb;

    // the rigid body of the bumpers
    private Rigidbody leftBumper;
    private Rigidbody rightBumper;

    // the Exporter script (if attached to the game object)
    private Exporter exporter;

    // the time the car was launched
    private float launchTime;

    // flag to remember if car was launched
    private bool isLaunched = false;


    // the width of the car (could also be gotten from the collider)
    private readonly float carWidth = 0.3f;

    // the width of the bumpers (could also be gotten from the collider)
    private readonly float bumperWidth = 0.1f;

    // determines the state of the bumpers
    // 0: both bumpers are fixed
    // 1: left bumper is free to move, no friction
    // 2: left bumper is free to move, with friction
    public int bumperMode = 2;

    // helper flag to automatically start the car when recording starts
    // Window > General > Recorder > Start Recording
    private readonly bool recording = true;




    // initial velocity of the car
    public float initialVelocity = 0f;

    // the length of the uncompressed spring
    public float springLength = 0.15f; // 15 cm

    // spring constant
    public float springConstant = 10f; // 10 N/m

    // friction coefficient bumper (laminare viskose D�mpfung FR=frictionCoefficient * v)
    public float frictionCoefficient = 5.2f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // get the rigid body of the car
        rb = GetComponent<Rigidbody>();

        // get the rigid body of the bumpers
        leftBumper = GameObject.Find("Bumper left").GetComponent<Rigidbody>();
        rightBumper = GameObject.Find("Bumper right").GetComponent<Rigidbody>();

        // get the Exporter script
        exporter = GetComponent<Exporter>();
        Assert.IsNotNull(exporter, "Exporter script not found");


        // confine motion of the to 1D along z-axis
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        // set motion of the bumpers
        // Your code here ...
        switch (bumperMode)
        {
            case 0:
                // fix bumper in place
                leftBumper.constraints = RigidbodyConstraints.FreezeAll;
                rightBumper.constraints = RigidbodyConstraints.FreezeAll;
                break;
            case 1:
            case 2:
                leftBumper.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
                rightBumper.constraints = RigidbodyConstraints.FreezeAll;
                break;

            default:
                Assert.Fail("Invalid bumper mode");
                break;
        }


        // Note: switch off collider in the inspector, because depending on the spring paramters, it can happen that the car touches the bumpers
        // and then the movement looks very strange and debugging is difficult (try increasing initial velocity). Without collider one sees
        // that the car penetrates the bumpers and debugging becomes easier.


        // === solver settings ===

        // Controls how often physics updates occur (default: 0.02s or 50 Hz)
        Time.fixedDeltaTime = 0.02f;

        // Determines how many times Unity refines the constraint solving per physics step(default: 6)
        Physics.defaultSolverIterations = 6;

        // Similar to above but specifically for velocity constraints (default: 1)
        Physics.defaultSolverVelocityIterations = 1;
    }



    // Update is called once per frame
    void Update()
    {
        // launch car
        if (Keyboard.current[Key.Space].wasPressedThisFrame && (recording && !isLaunched))
        {
            // remember that car was launched
            isLaunched = true;

            // remember the current time
            launchTime = Time.time;

            // set the initial velocity of the car
            rb.linearVelocity = new Vector3(0, 0, initialVelocity);

            // log
            Debug.Log("Launching the car");
        }


        // reload scene
        if (Keyboard.current[Key.R].wasPressedThisFrame)
        {
            // Reload the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    private void FixedUpdate()
    {
        // distance of the car to the bumpers
        float distToLeft = transform.position.z - leftBumper.position.z;
        float distToRight = rightBumper.position.z - transform.position.z;

        // compression of the springs
        // + (carWidth + bumperWidth) / 2f to account for the width of the car and the bumper
        float compressionLeft = Mathf.Max(0f, springLength - distToLeft + (carWidth + bumperWidth) / 2f);
        // float compressionRight = Mathf.Max(0f, springLength - distToRight + (carWidth + bumperWidth) / 2f);
        // -> not needed for exercise 2

        float forceLeft = compressionLeft * springConstant;
        // float forceRight = compressionRight * springConstant;
        // -> not needed for exercise 2

        // force of the springs
        Vector3 force = Vector3.zero;

        // === left spring
        if (compressionLeft > 0f)
        {
            force += Vector3.forward * forceLeft;

            // Apply equal and opposite force to the left bumper
            if (bumperMode == 1 || bumperMode == 2)
            {
                // auf den Wagen:
                rb.AddForce(Vector3.forward * forceLeft, ForceMode.Force);

                // auf den Prellbock:
                leftBumper.AddForce(Vector3.back * forceLeft, ForceMode.Force);
            }

        }

        // === right spring
        // if (compressionRight > 0f)
        //     force += Vector3.back * (compressionRight * springConstant);
        // -> not needed for exercise 2
§
        if (transform.position.z + (carWidth / 2) >= rightBumper.position.z - (bumperWidth / 2))
        {
            // Kollision detected
            // exercise 2: just reverse the velocity of the car
            float vAfterCollision = -1 * rb.linearVelocity.z;
            rb.linearVelocity = new Vector3(0, 0, vAfterCollision);
        }


        // apply the force to the car
        rb.AddForce(force, ForceMode.Force);

        // add friction to the left bumper
        if (bumperMode == 2)
        {
            // formula: F = -v * frictionCoefficient
            Vector3 friction = -leftBumper.linearVelocity * frictionCoefficient;
            leftBumper.AddForce(friction, ForceMode.Force);
        }



        // === left bumper

        if (bumperMode == 1 || bumperMode == 2)
        {

        }


        // === time series data

        // store time series record
        if (isLaunched)
        {
            // Your code here ... (adapt as needed)
            // TimeSeriesData timeSeriesData = new(rb, Time.time - launchTime, compressionLeft, forceLeft, compressionRight, forceRight, leftBumper.position.z, leftBumper.linearVelocity.z);
            // not needed for exercise 2

            TimeSeriesData timeSeriesData = new(rb, Time.time - launchTime, compressionLeft, forceLeft, leftBumper.position.z, leftBumper.linearVelocity.z);

            exporter.AddData(timeSeriesData);
        }

    }

    void OnGUI()
    {
        GUIStyle textStyle = new()
        {
            fontSize = 20,
            normal = { textColor = Color.black }
        };

        // keyboard shortcuts
        GUI.Label(new Rect(10, Screen.height - 20, 400, 20),
            "R ... Reload", textStyle);
    }
}
