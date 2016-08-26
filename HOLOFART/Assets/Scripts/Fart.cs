using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class Fart : MonoBehaviour
{
    private GestureRecognizer gr;
    public AudioClip[] farts;
    public GameObject fartParticleSystem;
    private bool handDetected;
    private Vector3 handPosition;

    // Use this for initialization
    void Start()
    {
        gr = new GestureRecognizer();
        gr.SetRecognizableGestures(GestureSettings.Tap);
        gr.TappedEvent += Gr_TappedEvent;
        gr.StartCapturingGestures();

        InteractionManager.SourceDetected += InteractionManager_SourceDetected;
        InteractionManager.SourceUpdated += InteractionManager_SourceUpdated;
        InteractionManager.SourceLost += InteractionManager_SourceLost;
    }

    private void Gr_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        int soundNum = Random.Range(0, farts.Length);
        GameObject soundPlayer = new GameObject("FartSound");
        AudioSource soundSource = soundPlayer.AddComponent<AudioSource>();
        soundSource.clip = farts[soundNum];
        soundSource.Play();
        Destroy(soundPlayer, soundSource.clip.length);

        if(handDetected)
        {
            GameObject fartSmoke = (GameObject)Instantiate(fartParticleSystem, handPosition, Camera.main.transform.rotation);
            soundSource.Play();
            Destroy(fartSmoke, 5);
        }
    }

    private void InteractionManager_SourceDetected(InteractionSourceState state)
    {
        handDetected = true;
    }

    private void InteractionManager_SourceUpdated(InteractionSourceState state)
    {
        state.properties.location.TryGetPosition(out handPosition);
    }

    private void InteractionManager_SourceLost(InteractionSourceState state)
    {
        handDetected = false;
    }
}