using System;
using RQ.Common.Components;
using RQ.Common.Controllers;
using RQ.Entity;
using RQ.Entity.Assets;
using RQ.Entity.Common;
using RQ.Entity.Data;
using RQ.Messaging;
using RQ.Model.Item;
using RQ.Model.UI;
using UnityEngine;

namespace RQ.Physics.Components
{
    [AddComponentMenu("RQ/Components/Entity Stats")]
    public class EntityStatsComponent : ComponentPersistence<EntityStatsComponent>
    {
        [SerializeField]
        private EntityStatsData _entityStats = new EntityStatsData();
        protected PlayerComponent _playerComponent;
        [SerializeField]
        private PlayerLevelStatsConfig _playerLevelStatsConfig;
        //[SerializeField]
        //private bool _isMainPlayer;
        private long _setEntityStatsId, _setupPlayerStatsId, _fullHealId, _healHPId, _skillUsedId, _updateHUDId, _setCurrentHPId, _addXPId, _entityDiedId,
            _updateHUDId2;

        private Action<Telegram2> _setEntityStatsDelegate, _setupPlayerStatsDelegate, _fullHealDelegate, _healHPDelegate, _skillUsedDelegate, 
            _updateHUDDelegate, _setCurrentHPDelegate, _addXPDelegate, _entityDiedDelegate;

        public override void Awake()
        {
            base.Awake();
            _setEntityStatsDelegate = (data) =>
            {
                _entityStats = (EntityStatsData) data.ExtraInfo;
            };
            _setupPlayerStatsDelegate = (data) =>
            {
                // Start of game, set it to our current stats
                if (GameDataController.Instance.Data.CurrentEntityStats == null)
                    GameDataController.Instance.Data.CurrentEntityStats = _entityStats.Clone();

                _entityStats = GameDataController.Instance.Data.CurrentEntityStats.Clone();
                UpdateHud();
            };
            _fullHealDelegate = (data) =>
            {
                _entityStats.CurrentHP = _entityStats.MaxHP;
                _entityStats.CurrentSP = _entityStats.MaxSP;
                UpdateHud();
            };
            _healHPDelegate = (data) =>
            {
                //if (this == null)
                //    Debug.LogError(name + " is null in the Messaging System");
                float.TryParse((string) data.ExtraInfo, out var amount);
                RaiseCurrentHP(amount);
                UpdateHud();
            };
            _skillUsedDelegate = (data) =>
            {
                var skill = data.ExtraInfo as SkillConfig;
                SkillUsed(skill);
            };
            _updateHUDDelegate = (data) =>
            {
                UpdateHud();
            };
            _setCurrentHPDelegate = (data) =>
            {
                if (float.TryParse((string) data.ExtraInfo, out var currentHP))
                    _entityStats.CurrentHP = currentHP;
                UpdateHud();
            };
            _addXPDelegate = (data) =>
            {
                int xp;
                if (int.TryParse((string) data.ExtraInfo, out xp))
                {
                    AddXp(xp);
                    if (CheckForLevelUp())
                        ProcessLevelUp();
                }
            };
            _entityDiedDelegate = (data) =>
            {
                SpriteBaseComponent sbc = _componentRepository as SpriteBaseComponent;
                if (sbc.EntityType == Enums.EntityType.Enemy || sbc.EntityType == Enums.EntityType.Boss)
                {
                    var damageComponent = _componentRepository.Components.GetComponent<DamageComponent>();
                    var damageInfo = damageComponent.GetDamageInfo();

                    //var mainCharacter = EntityContainer._instance.GetMainCharacter();
                    MessageDispatcher2.Instance.DispatchMsg("AddXP", 0f, this.UniqueId,
                        damageInfo.DamagedByEntity.UniqueId, _entityStats.Experience.ToString());
                }
            };
        }

        public override void Start()
        {
            base.Start();
            if (!Application.isPlaying)
                return;
            //if (_componentRepository.name.Contains("Rolan"))
            //    Debug.LogError("Entity Stats Component starting for Rolan");
            _playerComponent = _componentRepository.Components.GetComponent<PlayerComponent>();
            //if (_playerComponent != null)
            //{     
            if (this.tag == "Player")
            { 
                MessageDispatcher2.Instance.DispatchMsg("SetupPlayerStats", 0f, this.UniqueId, _componentRepository.UniqueId, null);
                //_playerComponent.SetDestroyTracker(() =>
                //    {
                //        Debug.LogError("PlayerComponent got destroyed THAT ENTITY STATS IS POINTING TO " + this.UniqueId);
                //    });
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            //if (_componentRepository.name.Contains("Rolan"))
            //    Debug.LogError("Entity Stats Component destroying for Rolan");
            if (_playerComponent != null)
                _playerComponent.SetDestroyTracker(null);
        }

        public EntityStatsData GetEntityStats()
        {
            return _entityStats;
        }

        private void SendEntityStats(string senderId)
        {
            // Send the stats to the requester
            MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
                senderId, Enums.Telegrams.SetEntityStats, _entityStats);
            //return _entityStats;
        }

        public override void StartListening()
        {
            base.StartListening();
            var unqiueid = _componentRepository.UniqueId;
            var rqname = _componentRepository.name;
            _setEntityStatsId = MessageDispatcher2.Instance.StartListening("SetEntityStats", UniqueId, _setEntityStatsDelegate);
            //_componentRepository.StartListening("SetEntityStats", this.UniqueId, );

            //if (rqname.Contains("Rolan"))
            //{               
            //    int i = 1;
            //}
            _setupPlayerStatsId =
                MessageDispatcher2.Instance.StartListening("SetupPlayerStats", _componentRepository.UniqueId,
                    _setupPlayerStatsDelegate);
            //_componentRepository.StartListening("SetupPlayerStats", this.UniqueId, );
            _fullHealId =
                MessageDispatcher2.Instance.StartListening("FullHeal", _componentRepository.UniqueId,
                    _fullHealDelegate);
            //_componentRepository.StartListening("FullHeal", this.UniqueId, );
            _healHPId =
                MessageDispatcher2.Instance.StartListening("HealHP", _componentRepository.UniqueId,
                    _healHPDelegate);
            //_componentRepository.StartListening("HealHP", this.UniqueId, );
            _skillUsedId =
                MessageDispatcher2.Instance.StartListening("SkillUsed", _componentRepository.UniqueId,
                    _skillUsedDelegate);
            //_componentRepository.StartListening("SkillUsed", this.UniqueId, );
            _updateHUDId =
                MessageDispatcher2.Instance.StartListening("UpdateHUD", _componentRepository.UniqueId,
                    _updateHUDDelegate);
            _updateHUDId2 = MessageDispatcher2.Instance.StartListening("UpdateHUD", this.UniqueId,
                _updateHUDDelegate);
            //_componentRepository.StartListening("UpdateHUD", this.UniqueId, , true);
            _setCurrentHPId =
                MessageDispatcher2.Instance.StartListening("SetCurrentHP", _componentRepository.UniqueId,
                    _setCurrentHPDelegate);
            //_componentRepository.StartListening("SetCurrentHP", this.UniqueId, );
            // This is to notify the main character to add XP
            _addXPId =
                MessageDispatcher2.Instance.StartListening("AddXP", _componentRepository.UniqueId,
                    _addXPDelegate);
            //_componentRepository.StartListening("AddXP", this.UniqueId, );
            // This is for enemies to notify the attacker to add XP for the kill
            _entityDiedId =
                MessageDispatcher2.Instance.StartListening("EntityDied", _componentRepository.UniqueId,
                    _entityDiedDelegate);
            //_componentRepository.StartListening("EntityDied", this.UniqueId, );
        }

        public override void StopListening()
        {
            base.StopListening();
            MessageDispatcher2.Instance.StopListening("SetEntityStats", _componentRepository.UniqueId, _setEntityStatsId);
            //_componentRepository.StopListening("SetEntityStats", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("SetupPlayerStats", _componentRepository.UniqueId, _setupPlayerStatsId);
            //_componentRepository.StopListening("SetupPlayerStats", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("FullHeal", _componentRepository.UniqueId, _fullHealId);
            //_componentRepository.StopListening("FullHeal", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("HealHP", _componentRepository.UniqueId, _healHPId);
            //_componentRepository.StopListening("HealHP", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("SkillUsed", _componentRepository.UniqueId, _skillUsedId);
            //_componentRepository.StopListening("SkillUsed", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("UpdateHUD", _componentRepository.UniqueId, _updateHUDId);
            MessageDispatcher2.Instance.StopListening("UpdateHUD", this.UniqueId, _updateHUDId2);
            //_componentRepository.StopListening("UpdateHUD", this.UniqueId, true);
            MessageDispatcher2.Instance.StopListening("SetCurrentHP", _componentRepository.UniqueId, _setCurrentHPId);
            //_componentRepository.StopListening("SetCurrentHP", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("AddXP", _componentRepository.UniqueId, _addXPId);
            //_componentRepository.StopListening("AddXP", this.UniqueId);
            MessageDispatcher2.Instance.StopListening("EntityDied", _componentRepository.UniqueId, _entityDiedId);
            //_componentRepository.StopListening("EntityDied", this.UniqueId);
        }

        private void SkillUsed(SkillConfig skill)
        {
            ConsumeCurrentSP(skill.Value);
            foreach (var orb in skill.Orbs)
            {
                ItemInInventoryData playerOrb;
                if (!GameDataController.Instance.Data.Inventory.Items.TryGetValue(orb.Orb.UniqueId, out playerOrb))
                    continue;
                playerOrb.Quantity -= orb.Cost;
            }
            UpdateHud();
        }

        private void RaiseCurrentHP(float amount)
        {
            _entityStats.CurrentHP += amount;
            if (_entityStats.CurrentHP > _entityStats.MaxHP)
                _entityStats.CurrentHP = _entityStats.MaxHP;
        }

        private void ConsumeCurrentSP(float amount)
        {
            _entityStats.CurrentSP -= amount;
            if (_entityStats.CurrentSP < 0)
                _entityStats.CurrentSP = 0;
        }

        private void AddXp(int xp)
        {
            if (xp == 0)
                return;
            _entityStats.Experience += xp;
            var hudTextEntryData = new HudTextEntryData()
            {
                 Color = Color.black,
                 Data = xp.ToString() + " xp",
                 Duration = 0.5f
            };
            MessageDispatcher2.Instance.DispatchMsg("AddHudText", 0f, this.UniqueId, _componentRepository.UniqueId, hudTextEntryData);

            //UpdateHud();
        }

        private bool CheckForLevelUp()
        {
            if (_playerLevelStatsConfig == null || _playerLevelStatsConfig.PlayerLevelData.Length == 0 ||
                _entityStats.Level > _playerLevelStatsConfig.PlayerLevelData.Length)
                return false;
            var nextLevelStats = _playerLevelStatsConfig.PlayerLevelData[_entityStats.Level];
            return _entityStats.Experience >= nextLevelStats.ExperienceNeeded;
        }

        private void ProcessLevelUp()
        {
            _entityStats.Level++;
            ImportLevelData();
            _entityStats.CurrentHP = _entityStats.MaxHP;
            _entityStats.CurrentSP = _entityStats.MaxSP;
            MessageDispatcher2.Instance.DispatchMsg("LevelUp", 0f, this.UniqueId, null, _entityStats);
            // Inform other components of a Level Up.  PlayerComponent reacts to this.
            //MessageDispatcher2.Instance.DispatchMsg("LevelUp", 0f, this.UniqueId, _componentRepository.UniqueId, null);
            Debug.LogWarning("Player leveled up to level " + _entityStats.Level.ToString());            
        }

        private void ImportLevelData()
        {
            var levelData = _playerLevelStatsConfig.PlayerLevelData[_entityStats.Level - 1];
            _entityStats.MaxHP = levelData.HP;
            _entityStats.MaxSP = levelData.SP;
            _entityStats.PhysicalAttackPower = levelData.PhysicalAttackPower;
            _entityStats.PhysicalDefense = levelData.PhysicalDefense;
            _entityStats.MagicalAttackPower = levelData.MPAttackPower;
            _entityStats.MagicalDefense = levelData.MagicalDefense;
        }

        public override bool HandleMessage(Telegram msg)
        {
            if (base.HandleMessage(msg))
                return true;

            switch (msg.Msg)
            {
                case Enums.Telegrams.Damaged:
                    //Debug.LogError("(EntityStats) HandleMessage.Damaged called");
                    var damageInfo = (DamageEntityInfo)msg.ExtraInfo;
                    _entityStats.CurrentHP -= damageInfo.DamageAmount;
                    var hudTextEntryData = new HudTextEntryData(-damageInfo.DamageAmount, Color.red, 0f);
                    //SendMessageToSpriteBase(0f, )
                    MessageDispatcher2.Instance.DispatchMsg("AddHudText", 0f, this.UniqueId, _componentRepository.UniqueId, hudTextEntryData);
                    UpdateHud();
                    if (_entityStats.CurrentHP <= 0)
                    {
                        MessageDispatcher2.Instance.DispatchMsg("EntityDied", 0f, this.UniqueId, _componentRepository.UniqueId, null);
                    }
                    MessageDispatcher2.Instance.DispatchMsg("EntityHPChanged", 0f, this.UniqueId, _componentRepository.UniqueId, _entityStats.CurrentHP);
                    break;
                case Enums.Telegrams.GetEntityStats:
                    // Send the stats back to the requester
                    SendEntityStats(msg.SenderId);
                    break;
            }
            return false;
        }

        private void UpdateHud()
        {
            if (_playerComponent == null)
            {
                Debug.LogError("No Player Component");
                return;
            }
            
            //var healthPct = _entityStats.CurrentHP / _entityStats.MaxHP;
            MessageDispatcher2.Instance.DispatchMsg("UpdateStatsInHud", 0f, this.UniqueId, "UI Manager", _entityStats);
            //MessageDispatcher.Instance.DispatchMsg(0f, this.UniqueId,
            //    _playerComponent.UniqueId, Enums.Telegrams.SetCurrentHealth,
            //    _entityStats);
        }

        public override void Serialize(Serialization.EntitySerializedData entitySerializedData)
        {
            base.Serialize(entitySerializedData);
            base.SerializeComponent(entitySerializedData, _entityStats);
        }

        public override void Deserialize(Serialization.EntitySerializedData entitySerializedData)
        {
            base.Deserialize(entitySerializedData);
            _entityStats = base.DeserializeComponent<EntityStatsData>(entitySerializedData);
        }
    }
}
