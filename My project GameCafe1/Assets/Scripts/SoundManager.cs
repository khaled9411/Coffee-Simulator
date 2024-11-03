using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";



    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private float volume = 1f;

    private int footStepNumber =0;
    private void Awake()
    {
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
        Instance = this;
    }
    private void Start()
    {
        Player.Instance.playerMovement.OnJump += Player_OnJump;
        HungerSystem.Instance.OnEatFood += HungerSystem_OnEatFood;
        MoneyManager.Instance.OnMoneyChange += MoneyManager_OnMoneyChange;
    }

    private void MoneyManager_OnMoneyChange()
    {
        PlaySound(audioClipRefsSO.buy, Player.Instance.transform.position);
    }

    private void HungerSystem_OnEatFood(float _)
    {
        PlaySound(audioClipRefsSO.eat, Player.Instance.transform.position);
    }
    private void Player_OnJump()
    {
        PlaySound(audioClipRefsSO.jump, Player.Instance.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f)
    {
        if(audioClipArray == null)
        {
            Debug.LogError("null audio");
            return;
        }
        AudioSource.PlayClipAtPoint(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volumeMultiplier * volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        if (audioClip == null)
        {
            Debug.LogError("null audio");
            return;
        }
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }
    public void PlayFootstepsSound(Vector3 position, float volumeMultiplier)
    {
        Debug.Log(footStepNumber);
        PlaySound(audioClipRefsSO.footsteps[footStepNumber], position, volumeMultiplier * volume);
        footStepNumber = (footStepNumber + 1) % audioClipRefsSO.footsteps.Length;
    }

}
