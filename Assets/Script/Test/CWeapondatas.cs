using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[System.Serializable]

public static class CWeaponData
{
    private class Data
    {
        public string INFO=null;
        public int SHOOT_CD=0;
        public int COST=0;
        public int SHOOT_SPEED=0;
    }
    private class Datas
    {
        public List<Data> weapondatas=new List<Data>();
    }
    private static Datas m_Weapondatas;
    public static void LoadData()
    {
        string temp = File.ReadAllText(Application.streamingAssetsPath + "\\weapondata.json");
        m_Weapondatas = JsonUtility.FromJson<Datas>(temp);
        foreach (Data item in m_Weapondatas.weapondatas)
        {
            Debug.Log(item.SHOOT_CD);
        }
    }
}



