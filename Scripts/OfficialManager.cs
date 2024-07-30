using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OfficialType
{
    TaoJinNiang,
    KeLuoSi,
    Urbian,
    Kakarot
}

public class OfficialManager : MonoBehaviour
{
    public static OfficialManager Instance;
    public List<OfficialBase> officials = new List<OfficialBase>();
    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetOfficialForType(OfficialType type)
    {
        switch (type)
        {
            case OfficialType.TaoJinNiang:
                return GameManager.Instance.GameConf.TaoJinNiang;
            case OfficialType.KeLuoSi:
                return GameManager.Instance.GameConf.KeLuoSi;
            case OfficialType.Urbian:
                return GameManager.Instance.GameConf.Urbian;
            case OfficialType.Kakarot:
                return GameManager.Instance.GameConf.Kakarot;
            default:
                break;
        }
        return null;
    }

    public void AddOfficial(OfficialBase official)
    {
        officials.Add(official);
    }

    public void RemoveOfficial(OfficialBase official)
    {
        officials.Remove(official);
    }
}
