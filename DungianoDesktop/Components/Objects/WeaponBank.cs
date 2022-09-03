using System.Collections.Generic;

namespace DungianoDesktop.Components.Objects
{
    public struct WeaponInfo
    {
        public int Damage;
        public int ShootInterval;
        public string Name;
        public string TextureName;

        public WeaponInfo(int damage, int shootInterval, string name, string textureName)
        {
            Damage = damage;
            ShootInterval = shootInterval;
            Name = name;
            TextureName = textureName;
        }

        static public WeaponInfo Null()
        {
            return new WeaponInfo(0, 0, "", "");
        }
    }

    public class WeaponBank
    {
        private List<WeaponInfo> _bank;

        public WeaponBank()
        {
            _bank = new List<WeaponInfo>();
            _initWeapons();
        }

        private void _initWeapons()
        {
            _bank.Add(new WeaponInfo(10, 300, "Flute", "Objects/Weapons/Flute"));
            _bank.Add(new WeaponInfo(15, 300, "TranverseFlute", "Objects/Weapons/TranverseFlute"));
            _bank.Add(new WeaponInfo(20, 300, "Clarinet", "Objects/Weapons/Clarinet"));
            _bank.Add(new WeaponInfo(25, 300, "Bassoon", "Objects/Weapons/Basoon"));
            _bank.Add(new WeaponInfo(30, 300, "Saxophone", "Objects/Weapons/Saxophone"));
            _bank.Add(new WeaponInfo(30, 280, "Guitar", "Objects/Weapons/Guitar"));
            _bank.Add(new WeaponInfo(30, 260, "Violin", "Objects/Weapons/Violin"));
            _bank.Add(new WeaponInfo(30, 240, "Cello", "Objects/Weapons/Cello"));
            _bank.Add(new WeaponInfo(35, 240, "Trumpet", "Objects/Weapons/Trumpet"));
            _bank.Add(new WeaponInfo(35, 220, "FrenchHorn", "Objects/Weapons/French_horn"));
            _bank.Add(new WeaponInfo(40, 200, "Trombone", "Objects/Weapons/Trombone"));
            _bank.Add(new WeaponInfo(50, 200, "Tube", "Objects/Weapons/Tube"));
        }

        public WeaponInfo GetWeapon(string name)
        {
            foreach (WeaponInfo weapon in _bank)
            {
                if (weapon.Name == name)
                    return weapon;
            }

            return WeaponInfo.Null();
        }

        public WeaponInfo GetWeaponAt(int index)
        {
            if (index < _bank.Count)
                return _bank[index];
            else
                return WeaponInfo.Null();
        }
    }
}

