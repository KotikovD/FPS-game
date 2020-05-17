using System;
using UnityEngine;


namespace FPS_Kotikov_D.Data
{
    [Serializable]
    public struct SerializableGameObject
    {
        public string Name;
        public SerializableVector3 Pos;
        public SerializableQuaternion Rot;
        public SerializableVector3 Scale;
        public SerializablePlayer SPlayer;
        public bool IsEnable;

        public override string ToString()
        {
            return $"Name = {Name}; IsEnable = {IsEnable}; Pos = ({Pos});";
        }
    }

    [Serializable]
    public struct SerializablePlayer
    {
        public float CurrentHp;
        public SerializableWeapons[] Guns;

        public SerializablePlayer(Player player)
        {
            CurrentHp = Player.CurrentHp;

            var guns = player.GetComponentsInChildren<Gun>(true);
            Debug.Log("guns coun " + guns.Length);
            Guns = new SerializableWeapons[guns.Length];
            for (int i = 0; i < guns.Length; i++)
                Guns[i] = guns[i];

        }

        public static implicit operator SerializablePlayer(Player player)
        {
            return new SerializablePlayer(player);
        }

    }

    #region Weapoons

    [Serializable]
    public struct SerializableWeapons
    {
        public string WeaponName;
        public SerializableAmmuniton Ammunition;

        public SerializableWeapons(Gun gun)
        {
            WeaponName = gun.name;
            Ammunition = new SerializableAmmuniton(gun.CurrentAmmunition, gun.CountClips);

        }

        public static implicit operator SerializableWeapons(Gun Gun)
        {
            return new SerializableWeapons(Gun);
        }

        public override string ToString()
        {
            return $"WeaponName = {WeaponName}";
        }
    }

    [Serializable]
    public struct SerializableAmmuniton
    {
        
        public int CurrentAmmunition;
        public int CountClips;

        public SerializableAmmuniton(int currentAmmunition, int countClips)
        {
            CurrentAmmunition = currentAmmunition;
            CountClips = countClips;

        }

        public override string ToString()
        {
            return $"CurrentAmmunition = {CurrentAmmunition}, CountClips = {CountClips}";
        }
    }

    #endregion


    [Serializable]
    public struct SerializableVector3
    {
        public float X;
        public float Y;
        public float Z;

        public SerializableVector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static implicit operator Vector3(SerializableVector3 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }

        public static implicit operator SerializableVector3(Vector3 value)
        {
            return new SerializableVector3(value.x, value.y, value.z);
        }

        public override string ToString()
        {
            return $"X = {X}, Y = {Y}, Z = {Z}";
        }
    }

    [Serializable]
    public struct SerializableQuaternion
    {
        public float X;
        public float Y;
        public float Z;
        public float W;


        public SerializableQuaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }


        public static implicit operator Quaternion(SerializableQuaternion value)
        {
            return new Quaternion(value.X, value.Y, value.Z, value.W);
        }


        public static implicit operator SerializableQuaternion(Quaternion value)
        {
            return new SerializableQuaternion(value.x, value.y, value.z, value.w);
        }

        public override string ToString()
        {
            return $"X = {X}; Y = {Y}; Z = {Z}; W = {W};";
        }

    }
}