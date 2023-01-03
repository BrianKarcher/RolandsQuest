using RQ.Enums;
using RQ.Model.Serialization;
using System;
using System.Collections.Generic;

namespace RQ.Serialization
{
    /// <summary>
    /// Used for serialization of sprite data to and from disk
    /// </summary>
    [Serializable]
    public class EntitySerializedData
    {
        public string Name { get; set; }
        public bool RecreateOnGameLoad { get; set; }
        // Allow for the serialization of heirarchical entites
        // A Heirarchial entity is an entity that is a child entity.  For example,
        // We can have a dragon boss.  The body can be the main entity, a head a child entity, and both arms also
        // child entities
        public string ParentEntityUniqueId { get; set; }
        public TransformData TransformData { get; set; }

        //public PhysicsData PhysicsData { get; set; }
        //public BasicPhysicsData BasicPhysicsData { get; set; }
        //public AltitudeData AlititudeData { get; set; }
        //public Dictionary<string, AnimationData> AnimationDatas { get; set; }
        //public Dictionary<string, CollisionData> CollisionDatas { get; set; }
        //public Dictionary<string, AudioData> AudioDatas { get; set; }
        //public Dictionary<string, AIData> AIDatas { get; set; }
        public Dictionary<string, object> ComponentData { get; set; }

        //public StatusPersistenceLength StatusPersistence { get; set; }

        public string UniqueId { get; set; }
        //public int CurrentState { get; set; }
        public string EntityPrefabUniqueId { get; set; }
        //public EntityClass EntityClass { get; set; }
        public EntityType EntityType { get; set; }
        
        // Used in Serialization
        //public Vector3Serializer Position { get; set; }
        //public Dictionary<string, StateInfo> StateInfos { get; set; }
        //public Dictionary<string, StateMapping> StateMappings { get; set; }
        public Dictionary<string, string> UniqueIdMappings { get; set; }

        public bool IsUsableEnabled { get; set; }
        public bool IsEnabled { get; set; }
        // The sprite position changes from the main GameObject based on offset and other factors like height
        //public Vector3Serializer SpriteLocalPosition { get; set; }

        //public bool IgnoreInput { get; set; }
        //public float IgnoreInputTimeLeft { get; set; }
        //public float IgnoreInputElapsedTime { get; set; }
        //public Vector2D Size { get; set; }

        //public Offset Offset { get; set; }
        // The position of the main GameObject
        //public Vector3Serializer ObjectPosition { get; set; }
        //public Vector3Serializer Acceleration { get; set; }
        // We handle the velocity, not the RigidBody.  This gives us direct control.
        //public Vector3Serializer Velocity { get; set; }

        //public bool IgnoreInput { get; set; }


        //public Vector2D Velocity { get; set; }
        //a normalized vector pointing in the direction the entity is heading. 
        //public Vector2D Heading { get; set; }
        //a vector perpendicular to the heading vector
        //public Vector2D Side { get; set; }
        //public float Mass { get; set; }
        //the maximum speed this entity may travel at.
        //public float MaxSpeed { get; set; }
        /// <summary>
        /// the maximum force this entity can produce to power itself 
        /// (think rockets and thrust)
        /// </summary>
        //public float MaxForce { get; set; }
        //the maximum rate (radians per second)this vehicle can rotate         
        //public float MaxTurnRate { get; set; }





        //protected bool IsAwake { get; set; }
        //[HideInInspector]
        //public Vector3Serializer RecordedPosition { get; set; }

        //public Direction FacingDirection { get; set; }

        //public Direction WalkingDirection { get; set; }
        // Used to determine the speed at which the sprite moves at
        //public float Speed { get; set; }
        //public bool IsSpeedVariable { get; set; }
        //public float SpeedVariance { get; set; }

        //public float HitPoints { get; set; }
        //public float MaxHitPoints { get; set; }

        //[HideInInspector]
        //protected bool IsInvincible { get; set; }

        //[HideInInspector]
        //public Vector3Serializer OrigSpritePosition { get; set; }
        //public bool IsAirborn;
        //public Vector2D Altitude { get; set; }
        //public Vector2D AirVelocity { get; set; }
        //public Vector2D Gravity { get; set; }
        //public Vector2D JumpVelocity { get; set; }

        //public SteeringBehaviorData SteeringBehaviorData { get; set; }

        public EntitySerializedData()
        {
            //StateInfos = new Dictionary<string, StateInfo>();
            UniqueIdMappings = new Dictionary<string, string>();
            //AudioDatas = new Dictionary<string, AudioData>();
            //AnimationDatas = new Dictionary<string, AnimationData>();
            //CollisionDatas = new Dictionary<string, CollisionData>();
            //AIDatas = new Dictionary<string, AIData>();
            ComponentData = new Dictionary<string, object>();
        }
    }
}
