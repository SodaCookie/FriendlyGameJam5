using UnityEngine;
using Melodrive;
using Melodrive.Emotions;

public class PositionalEmotionExample : MonoBehaviour
{
    [Range(0.1f, 5.0f)]
    public float listenerSpeed = 1.0f;

    private MelodriveListener listener = null;
    private GameObject world;
    private EmotionalPoint[] points;
    private EmotionalPoint target = null;
    
    void Start ()
    {
        // First, get the world and the emotional points in the Scene
        world = GameObject.Find("EmotionalPoints");
        points = FindObjectsOfType<EmotionalPoint>();

        // Choose a new random target
        target = points[(int)(Random.value * points.Length)];

        // Find the listener to use later
        listener = FindObjectOfType<MelodriveListener>();
    }
	
	void Update () {
        // Rotate the world...
        world.transform.Rotate(new Vector3(0, 0.1f, 0));

        if (listener.transform.position == target.transform.position)
        {
            // Choose a new target if we got there
            target = points[(int)(Random.value * points.Length)];
        }
        else
        {
            // Or move towards our target
            float step = listenerSpeed * Time.deltaTime;
            listener.transform.position = Vector3.MoveTowards(listener.transform.position, target.transform.position, step);
        }
    }
}
