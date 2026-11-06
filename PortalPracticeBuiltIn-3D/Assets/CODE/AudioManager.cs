using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource SoundSource;

    public AudioClip CheckpointClip;
    public AudioClip ShootClip;
    public AudioClip TeleportClip;
    public AudioClip TurretShotClip;
    public AudioClip CollisionClip;
    public AudioClip JumpClip;
    public AudioClip WalkClip;
    public AudioClip DoorClip;
    public AudioClip GrabClip;
    public AudioClip ButtonActivationClip;
    public AudioClip WalkingClip;


    public void PlaySounds(AudioClip clip, float volume = 1f)
    {
        SoundSource.PlayOneShot(clip, volume);
    }
}