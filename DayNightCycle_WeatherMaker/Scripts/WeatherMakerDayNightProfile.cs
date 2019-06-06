﻿using DigitalRuby.WeatherMaker;
using UnityEngine;
using wizardscode.editor;
using wizardscode.validation;

namespace wizardscode.environment.weathermaker
{
    [CreateAssetMenu(fileName = "WeatherMakerDayNightCycleConfig", menuName = "Wizards Code/Day Night Cycle/Weather Maker Day Night Cycle Config")]
    public class WeatherMakerDayNightProfile : AbstractDayNightProfile
    {
        [Header("Weather Maker")]
        [Tooltip("The Weather Maker prefab to add to the scene.")]
        [Expandable(isRequired: true)]
        public PrefabSettingSO weatherMakerPrefab;

        [Tooltip("The Weather Maker Profile to use")]
        [Expandable(isRequired: true, isRequiredMessage: "Select or create a weather maker profile.")]
        public WeatherMakerDayNightCycleProfileScript weatherMakerProfile;

        [Tooltip("Camera that allows the moon and starts to shine through.")]
        [Expandable(isRequired: true)]
        public PrefabSettingSO cameraPrefab;

        [Header("Lighting")]
        [Tooltip("Camera that allows the moon and starts to shine through.")]
        [Expandable(isRequired: true)]
        public ReflectionModeSettingSO reflectionMode;

        private GameObject weatherMakerScript;
        private GameObject dayNight;
        private float daySpeed;
        private float nightSpeed;

        internal override void Initialize()
        {
            WeatherMakerScript component = GameObject.FindObjectOfType<WeatherMakerScript>();
            if ( component == null)
            {
                Debug.LogError("You don't have a WeatherMakerScript in your scene. Please see the Weather Maker Day Night Cycle plugin README for instructions.");
                return;
            } else
            {
                weatherMakerScript = component.gameObject;
            }

            WeatherMakerDayNightCycleManagerScript dayNightScript = FindObjectOfType<WeatherMakerDayNightCycleManagerScript>();
            if (dayNightScript == null)
            {
                Debug.LogError("Cannot find an object with the `WeatherMakerDayNightCycle` attached.");
            } else
            {
                dayNight = dayNightScript.gameObject;
            }            

            WeatherMakerDayNightCycleManagerScript.Instance.DayNightProfile = weatherMakerProfile;

            base.Initialize();
        }

        internal override void InitializeCamera()
        {
        }

        internal override void InitializeLighting()
        {
            RenderSettings.fog = false;
        }

        internal override void InitializeSun()
        {
        }

        internal override void InitializeTiming()
        {
            WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay = startTime;
            daySpeed = 1440 / dayCycleInMinutes;
            WeatherMakerDayNightCycleManagerScript.Instance.Speed = daySpeed;
            nightSpeed = daySpeed; // don't currently support separate day and night speeds
            WeatherMakerDayNightCycleManagerScript.Instance.Speed = nightSpeed;

            WeatherMakerDayNightCycleManagerScript.Instance.DayNightProfile = weatherMakerProfile;
            WeatherMakerDayNightCycleManagerScript.Instance.DayNightProfile.UpdateFromProfile(true);
        }

        internal override void SetTime(float timeInSeconds)
        {
            WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay = timeInSeconds;
        }

        internal override float GetTime()
        {
            return WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay;
        }

        internal override void Update()
        {
            daySpeed = 1440 / dayCycleInMinutes;
            nightSpeed = daySpeed; // don't currently support separate day and night speeds
            if (daySpeed != 1440 / dayCycleInMinutes)
            {
                weatherMakerProfile.Speed = daySpeed;
                WeatherMakerDayNightCycleManagerScript.Instance.Speed = daySpeed;
            }

            if (nightSpeed != 1440 / dayCycleInMinutes)
            {
                weatherMakerProfile.NightSpeed = nightSpeed;
                WeatherMakerDayNightCycleManagerScript.Instance.NightSpeed = nightSpeed;
            }
        }
    }
}
