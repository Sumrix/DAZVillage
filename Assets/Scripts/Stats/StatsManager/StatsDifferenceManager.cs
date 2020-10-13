using System;
using System.Collections;

public class StatsDifferenceManager<StatsType> :
    StatsManager<StatsType>
    where StatsType : Stats, new()
{
    private StatsManager<StatsType> _first;
    private StatsManager<StatsType> _second;

    public StatsManager<StatsType> First
    {
        get { return _first; }
        set
        {
            _first.Changed -= Stats_Changed;
            _first = value;
            _first.Changed += Stats_Changed;
            OnChanged();
        }
    }
    public StatsManager<StatsType> Second
    {
        get { return _second; }
        set
        {
            _second.Changed -= Stats_Changed;
            _second = value;
            _second.Changed += Stats_Changed;
            OnChanged();
        }
    }

    public StatsDifferenceManager()
    {
        _first = new StatsManager<StatsType>();
        _second = new StatsManager<StatsType>();
        _first.Changed += Stats_Changed;
        _second.Changed += Stats_Changed;
        Calc();
    }
    private void Stats_Changed (object sender, EventArgs e)
    {
        Calc();
    }
    public void Calc()
    {
        Result = (StatsType)(_first.Result - _second.Result);
    }
    ~StatsDifferenceManager()
    {
        if (_first != null)
        {
            _first.Changed -= Stats_Changed;
        }
        if (_second != null)
        {
            _second.Changed -= Stats_Changed;
        }
    }
}