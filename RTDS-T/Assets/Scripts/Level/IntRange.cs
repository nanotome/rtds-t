using System;
using Random = UnityEngine.Random;

[Serializable]
public struct IntRange {

    public int m_Min;
    public int m_Max;

    public IntRange(int min, int max)
    {
        m_Min = min;
        m_Max = max;
    }

    public int RandomInt
    {
        get
        {
            return Random.Range(m_Min, m_Max);
        }
    }
}
