using UnityEngine;
using System.Collections.Generic;
using System;

namespace FPS_Kotikov_D
{
    sealed class Radar : PocketPCUI
    {


        #region Fields

        [SerializeField] private Color _playerColor;
        [SerializeField] private Color _enemyColor;
        [SerializeField] private Color _aidKitColor;
        [SerializeField] private Color _ammunitionColor;

        [SerializeField] private Sprite _itemsSprite;
        [SerializeField] private Sprite _playerSprite;
        [SerializeField] private Sprite _enemySprite;

        Dictionary<GameObject, Type> dict = new Dictionary<GameObject, Type>();
        private bool _isGenerate = false;

        #endregion


        #region Methods

        public override void Switch(bool value)
        {
            base.Switch(value);
            if (_isGenerate) return;

            var player = FindObjectOfType<Player>();
            dict.Add(player.gameObject, player.GetType());

            var enemys = FindObjectsOfType<Bot>();
            foreach (Bot enemy in enemys)
                dict.Add(enemy.gameObject, enemy.GetType());

            var aidKits = FindObjectsOfType<AidKit>();
            foreach (AidKit aidKit in aidKits)
                dict.Add(aidKit.gameObject, aidKit.GetType());

            var amuunitions = FindObjectsOfType<AmmunitionClip>();
            foreach (AmmunitionClip ammo in amuunitions)
                dict.Add(ammo.gameObject, ammo.GetType());

            foreach (var obj in dict)
            {
                var radarItem = new GameObject { name = StringKeeper.RadarItem };
                radarItem.transform.position = obj.Key.transform.position;
                radarItem.transform.up = obj.Key.transform.forward;
                radarItem.transform.forward = obj.Key.transform.up;
                radarItem.transform.SetParent(obj.Key.transform);
                radarItem.layer = LayerMask.NameToLayer(StringKeeper.RadarLayer);
                var itemSprite = radarItem.AddComponent<SpriteRenderer>();

                if (obj.Value.Equals(player.GetType()))
                {
                    itemSprite.sprite = _playerSprite;
                    itemSprite.color = _playerColor;
                }

                if (obj.Key.gameObject.GetComponent<Bot>())
                {
                    itemSprite.sprite = _enemySprite;
                    itemSprite.color = _enemyColor;
                }

                if (obj.Key.gameObject.GetComponent<AidKit>())
                {
                    itemSprite.sprite = _itemsSprite;
                    itemSprite.color = _aidKitColor;
                    radarItem.AddComponent<RadarItemLook>();
                }

                if (obj.Key.gameObject.GetComponent<AmmunitionClip>())
                {
                    itemSprite.sprite = _itemsSprite;
                    itemSprite.color = _ammunitionColor;
                    radarItem.AddComponent<RadarItemLook>();
                }
            }
            _isGenerate = true;
        }

        #endregion


    }
}