using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip clickSound;    

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }
}
