using UnityEngine;
using Melodrive;
using Melodrive.Triggers;

public class TriggerEmotionExample : MonoBehaviour
{
    [Range(0.1f, 5.0f)]
    public float listenerSpeed = 0.5f;

    private MelodriveListener listener = null;
    private GameObject world;
    private MelodriveTrigger[] points;
    private int t = 0;
    private MelodriveTrigger target = null;

    void Start()
    {
        // First, get the world and the emotional points in the Scene
        world = GameObject.Find("EmotionalTriggers");
        points = FindObjectsOfType<MelodriveTrigger>();

        // Choose a new random target
        target = points[t];

        // Find the listener to use later
        listener = FindObjectOfType<MelodriveListener>();
    }

    void Update()
    {
        // Rotate the world...
        world.transform.Rotate(new Vector3(0, 0.1f, 0));

        if (listener.transform.position == target.transform.position)
        {
            // Choose a new target if we got there
            t = (t + 1) % points.Length;
            target = points[t];
        }
        else
        {
            // Or move towards our target
            float step = listenerSpeed * Time.deltaTime;
            listener.transform.position = Vector3.MoveTowards(listener.transform.position, target.transform.position, step);
        }
    }
}
