using UnityEngine;

public class DayTime : MonoBehaviour
{
    [HideInInspector]
    public bool IsOffensiveMorning = true;
    [HideInInspector]
    public bool IsOffensiveNight = true;

    [Header("Light sources")]
    public Light _sun; // солнце (Direction Light)

    [Header("Game time vars")]
    public int Day = 0; //текущий день
    public int Hours = 0; // общедоступное значение часов
    public float Minutes = 0; // общедоступное значение минут
    public float TimeScale = 12;

    public int DayStarts = 8; //час, в который начинается день
    public int NightStarts = 20;
    [HideInInspector]
    public TimeUnit TimeUnit;
    [HideInInspector]
    public int DayDuration;
    [HideInInspector]
    public int NightDuration;
    [HideInInspector]
    public float DayProgress;
    [HideInInspector]
    public float NightProgress;

    [Header("Update intervals")]
    private float _newIntensity = 0;

    public delegate void TimeHandler(int day);
    public event TimeHandler DayStart;
    public event TimeHandler NightStart;

    private void Awake()
    {
        int NightAfterDay = DayStarts > NightStarts ? NightStarts + 24 : NightStarts;
        int HoursAfterDay = DayStarts > Hours ? Hours + 24 : Hours;
        TimeUnit = HoursAfterDay > NightAfterDay ? TimeUnit.Night : TimeUnit.Day;

        _newIntensity = TimeUnit == TimeUnit.Day ? 1 : 0;
        if (DayStarts < NightStarts)
        {
            DayDuration = NightStarts - DayStarts;
            NightDuration = DayStarts + 24 - NightStarts;
        }
        else
        {
            DayDuration = NightStarts + 24 - DayStarts;
            NightStarts = DayStarts - NightStarts;
        }
    }
    private void Update()
    {
        Minutes += Time.deltaTime * TimeScale; // 1 минута = 1 секунда реального времени * _timeScale)
        if (Minutes > 60)
        {
            Minutes = 0;
            Hours++;

            if (Hours == DayStarts && IsOffensiveMorning)
            {
                print("Day starting");
                _newIntensity = 1;
                TimeUnit = TimeUnit.Day;
                if (DayStart != null)
                    DayStart(Day);
            }

            if (Hours == NightStarts && IsOffensiveNight)
            {
                print("Night starting");
                _newIntensity = 0;
                TimeUnit = TimeUnit.Night;
                if (NightStart != null)
                    NightStart(Day);
            }

            if (Hours > 23)
            {
                Hours = 0;
                Day++;
            }
        }

        if (TimeUnit == TimeUnit.Day)
        {
            SetDayProgress();
        }
        else
        {
            SetNightProgress();
        }

        _sun.intensity = Mathf.Lerp(_sun.intensity, _newIntensity, Time.deltaTime / TimeScale * 6);
    }
    private void SetDayProgress()
    {
        float timeAfterDay;
        if (Hours < DayStarts)
        {
            timeAfterDay = 24 + Hours + Minutes / 60 - DayStarts;
        }
        else
        {
            timeAfterDay = Hours + Minutes / 60 - DayStarts;
        }
        float time = timeAfterDay / DayDuration;

        DayProgress = time;
        NightProgress = 1;
    }
    private void SetNightProgress()
    {
        float timeAfterNight;
        if (Hours < NightStarts)
        {
            timeAfterNight = 24 + Hours + Minutes / 60 - NightStarts;
        }
        else
        {
            timeAfterNight = Hours + Minutes / 60 - NightStarts;
        }
        float time = timeAfterNight / NightDuration;

        DayProgress = 1;
        NightProgress = time;
    }
    public void SkipDaytime()
    {
        var min = Mathf.Min(DayStarts, NightStarts);
        var max = Mathf.Max(DayStarts, NightStarts);
        if (Hours < min)
        {
            Hours = min - 1;
        }
        else if (Hours < max)
        {
            Hours = max - 1;
        }
        else
        {
            Hours = min - 1;
            Day++;
        }
        Minutes = 59;
    }
}
public enum TimeUnit
{
    Day,
    Night,
}