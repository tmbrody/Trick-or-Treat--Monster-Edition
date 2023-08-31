using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    const string GAMEMODEPREFS = "GameMode";
    const string VOLUMEPREFS = "Volume";
    const string SFXPREFS = "SFX";

    const string DEFAULT_GAMEMODE = "GameModeNormal";

    const float MIN_VOLUME = 0f;
    const float DEFAULT_VOLUME = 0.1f;
    const float MAX_VOLUME = 1f;

    public static void SetDefaultGameModePrefs()
    {
        PlayerPrefs.SetString(GAMEMODEPREFS, DEFAULT_GAMEMODE);
    }

    public static void SetGameModePrefs(string gameMode)
    {
        PlayerPrefs.SetString(GAMEMODEPREFS, gameMode);
    }

    public static string GetGameModePrefs()
    {
        return PlayerPrefs.GetString(GAMEMODEPREFS);
    }

    public static void SetDefaultVolumePrefs()
    {
        PlayerPrefs.SetFloat(VOLUMEPREFS, DEFAULT_VOLUME);
    }

    public static void SetVolumePrefs(float volume)
    {
        if (volume >= MIN_VOLUME && volume <= MAX_VOLUME)
        {
            PlayerPrefs.SetFloat(VOLUMEPREFS, volume);
        }
    }

    public static float GetVolumePrefs()
    {
        return PlayerPrefs.GetFloat(VOLUMEPREFS);
    }

    public static void SetDefaultSFXPrefs()
    {
        PlayerPrefs.SetFloat(SFXPREFS, DEFAULT_VOLUME);
    }

    public static void SetSFXPrefs(float sfxVolume)
    {
        if (sfxVolume >= MIN_VOLUME && sfxVolume <= MAX_VOLUME)
        {
            PlayerPrefs.SetFloat(SFXPREFS, sfxVolume);
        }
    }

    public static float GetSFXPrefs()
    {
        return PlayerPrefs.GetFloat(SFXPREFS);
    }
}
