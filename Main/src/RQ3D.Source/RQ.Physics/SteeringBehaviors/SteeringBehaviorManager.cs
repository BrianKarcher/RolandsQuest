using RQ.Model.Interfaces;
using RQ.Model.Physics;
using RQ.Physics.Components;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RQ.Physics.SteeringBehaviors
{
    [Serializable]
    public class SteeringBehaviorManager : ISteeringBehaviorManager
    {
        private Dictionary<behavior_type, ISteeringBehavior> _steeringBehaviors;
        public IPhysicsAffector SteeringPhysicsAffector { get; set; }

        //these can be used to keep track of friends, pursuers, or prey
        public PhysicsComponent TargetAgent1;
        public PhysicsComponent TargetAgent2 { get; set; }

        //the current target
        public Vector2D Target;

        public Vector3 Offset;

        public string SteeringMode;

        //a pointer to the owner of this instance
        public IBasicPhysicsComponent Entity { get; set; }

        public CollisionComponent CollisionComponent { get; set; }

        //the steering force created by the combined effect of all
        //the selected behaviors
        [SerializeField]
        private Vector2D _steeringForce;

        //how far the agent can 'see'
        public float ViewDistance { get; set; }

        //public bool IsPathFinished { get; set; }

        //private IEntityResolver _entityResolver { get; set; }

        //binary flags to indicate whether or not a behavior should be active
        //private behavior_type _flags;

        public enum summing_method
        {
            weighted_average = 0,
            prioritized = 1,
            dithered = 2
        }

        //is cell space partitioning to be used or not?
        //private bool _cellSpaceOn;

        //what type of method is used to sum any active behavior
        private summing_method _summingMethod;

        //public SteeringBehaviorManager(IBasicPhysicsComponent entity, Transform transform)
        //{

        //}

        public void Setup(IBasicPhysicsComponent entity, Transform transform)
        {
            Entity = entity;
            SteeringPhysicsAffector = entity.GetPhysicsAffector("Steering");
            //CollisionComponent = collisionComponent;
            //_entityResolver = entityResolver;
            //_entity = agent;
            //_flags = behavior_type.none;

            //_viewDistance = Constants.ViewDistance;



            TargetAgent1 = null;
            TargetAgent2 = null;


            //_cellSpaceOn = false;
            _summingMethod = summing_method.weighted_average;

            _steeringBehaviors = new Dictionary<behavior_type, ISteeringBehavior>(new behavior_typeComparer());
            // This order is very important, it determines the order that the force calculations are run
            // In general, it is ordered from most important to least important
            // @todo address this, this is a lot of class instantiation
            _steeringBehaviors.Add(behavior_type.wall_avoidance, new WallAvoidance(this));
            _steeringBehaviors.Add(behavior_type.obstacle_avoidance, new ObstacleAvoidance(this));
            _steeringBehaviors.Add(behavior_type.evade, new Evade(this));
            _steeringBehaviors.Add(behavior_type.flee, new Flee(this));
            //_steeringBehaviors.Add(behavior_type.separation, new Separation(this));
            //_steeringBehaviors.Add(behavior_type.alignment, new Alignment(this));
            //_steeringBehaviors.Add(behavior_type.cohesion, new Cohesion(this));
            _steeringBehaviors.Add(behavior_type.seek, new Seek(this));
            _steeringBehaviors.Add(behavior_type.arrive, new Arrive(this));
            _steeringBehaviors.Add(behavior_type.wander, new Wander(this, transform));
            _steeringBehaviors.Add(behavior_type.pursuit, new Pursuit(this));
            _steeringBehaviors.Add(behavior_type.offset_pursuit, new OffsetPursuit(this));
            _steeringBehaviors.Add(behavior_type.interpose, new Interpose(this));
            _steeringBehaviors.Add(behavior_type.hide, new Hide(this));
            _steeringBehaviors.Add(behavior_type.follow_path, new FollowPath(this));
            _steeringBehaviors.Add(behavior_type.radius_clamp, new RadiusClamp(this));
        }

        //public IEntityResolver GetEntityResolver()
        //{
        //    return _entityResolver;
        //}

        //public MovingEntity GetEntity()
        //{
        //    return _entity;
        //}

        //public SpriteBase GetTargetAgent1()
        //{
        //    return _targetAgent1;
        //}

        //public SpriteBase GetTargetAgent2()
        //{
        //    return _targetAgent2;
        //}

        //public void SetEntity(MovingEntity entity)
        //{
        //    _entity = entity;
        //}

        public Vector2 GetTarget()
        {
            return Target;
        }

        public Vector3 GetTarget3()
        {
            return Target.ToVector3(0f);
        }

        //public void Set

        //this function tests if a specific bit of m_iFlags is set
        public bool IsOn(behavior_type bt)
        {
            return _steeringBehaviors[bt].IsOn();
        }

        public void TurnOn(behavior_type behaviortype)
        {
            _steeringBehaviors[behaviortype].TurnOn();
            CalculateSteeringModes();
        }
        public void TurnOff(behavior_type behaviortype)
        {
            _steeringBehaviors[behaviortype].TurnOff();
            CalculateSteeringModes();
        }

        public void CalculateSteeringModes()
        {
            SteeringMode = string.Empty;
            foreach (var behavior in _steeringBehaviors)
            {
                if (behavior.Value.IsOn())
                    SteeringMode += behavior.Key + " ";
            }
        }

        public ISteeringBehavior GetSteeringBehavior(behavior_type behaviortype)
        {
            return _steeringBehaviors[behaviortype];
        }

        //----------------------- Calculate --------------------------------------
        //
        //  calculates the accumulated steering force according to the method set
        //  in m_SummingMethod
        //------------------------------------------------------------------------
        public Vector2D Calculate()
        {
            //reset the steering force
            _steeringForce.SetToZero();

            //use space partitioning to calculate the neighbours of this vehicle
            //if switched on. If not, use the standard tagging system
            //if (!isSpacePartitioningOn())
            //{
            //    //tag neighbors if any of the following 3 group behaviors are switched on
            //    if (On(separation) || On(allignment) || On(cohesion))
            //    {
            //        m_pVehicle->World()->TagVehiclesWithinViewRange(m_pVehicle, m_dViewDistance);
            //    }
            //}
            //else
            //{

            //    //calculate neighbours in cell-space if any of the following 3 group
            //    //behaviors are switched on
            //    if (On(behavior_type.separation) || On(behavior_type.allignment) || On(behavior_type.cohesion))
            //    {
            //        m_pVehicle->World()->CellSpace()->CalculateNeighbors(m_pVehicle->Pos(), m_dViewDistance);
            //    }
            //}

            switch (_summingMethod)
            {
                case summing_method.weighted_average:

                    _steeringForce = CalculateWeightedSum();
                    break;

                case summing_method.prioritized:

                    _steeringForce = CalculatePrioritized();
                    break;

                case summing_method.dithered:

                    _steeringForce = CalculateDithered();
                    break;

                default:
                    _steeringForce = new Vector2D(0, 0);
                    break;
            }//end switch

            return _steeringForce;
        }

        //---------------------- CalculatePrioritized ----------------------------
        //
        //  this method calls each active steering behavior in order of priority
        //  and acumulates their forces until the max steering force magnitude
        //  is reached, at which time the function returns the steering force 
        //  accumulated to that  point
        //------------------------------------------------------------------------
        private Vector2D CalculatePrioritized()
        {
            var force = Vector2D.Zero();

            foreach (KeyValuePair<behavior_type, ISteeringBehavior> behavior in _steeringBehaviors)
            {
                if (!behavior.Value.IsOn())
                    continue;
                force = behavior.Value.CalculatePrioritized();

                if (!AccumulateForce(ref _steeringForce, force)) return _steeringForce;
            }

            return _steeringForce;
        }

        //---------------------- CalculateDithered ----------------------------
        //
        //  this method sums up the active behaviors by assigning a probabilty
        //  of being calculated to each behavior. It then tests the first priority
        //  to see if it should be calcukated this simulation-step. If so, it
        //  calculates the steering force resulting from this behavior. If it is
        //  more than zero it returns the force. If zero, or if the behavior is
        //  skipped it continues onto the next priority, and so on.
        //
        //  NOTE: Not all of the behaviors have been implemented in this method,
        //        just a few, so you get the general idea
        //------------------------------------------------------------------------
        private Vector2D CalculateDithered()
        {
            //reset the steering force
            _steeringForce.SetToZero();

            foreach (KeyValuePair<behavior_type, ISteeringBehavior> behavior in _steeringBehaviors)
            {
                _steeringForce = behavior.Value.CalculateDithered();

                if (!_steeringForce.isZero())
                {
                    _steeringForce.Truncate(SteeringPhysicsAffector.MaxForce);
                    return _steeringForce;
                }
            }
            return Vector2D.Zero();
        }

        //---------------------- CalculateWeightedSum ----------------------------
        //
        //  this simply sums up all the active behaviors X their weights and 
        //  truncates the result to the max available steering force before 
        //  returning
        //------------------------------------------------------------------------
        private Vector2D CalculateWeightedSum()
        {
            foreach (KeyValuePair<behavior_type, ISteeringBehavior> behavior in _steeringBehaviors)
            {
                _steeringForce += behavior.Value.CalculateWeightedSum();
            }
            _steeringForce.Truncate(SteeringPhysicsAffector.MaxForce);
            return _steeringForce;
        }

        //--------------------- AccumulateForce ----------------------------------
        //
        //  This function calculates how much of its max steering force the 
        //  vehicle has left to apply and then applies that amount of the
        //  force to add.
        //------------------------------------------------------------------------
        private bool AccumulateForce(ref Vector2D RunningTot, Vector2D ForceToAdd)
        {
            //calculate how much steering force the vehicle has used so far
            float MagnitudeSoFar = RunningTot.Length();

            //calculate how much steering force remains to be used by this vehicle
            float MagnitudeRemaining = SteeringPhysicsAffector.MaxForce - MagnitudeSoFar;

            //return false if there is no more force left to use
            if (MagnitudeRemaining <= 0.0) return false;

            //calculate the magnitude of the force we want to add
            float MagnitudeToAdd = ForceToAdd.Length();

            //if the magnitude of the sum of ForceToAdd and the running total
            //does not exceed the maximum force available to this vehicle, just
            //add together. Otherwise add as much of the ForceToAdd vector is
            //possible without going over the max.
            if (MagnitudeToAdd < MagnitudeRemaining)
            {
                RunningTot += ForceToAdd;
            }

            else
            {
                //add it to the steering force
                RunningTot += (Vector2D.Vec2DNormalize(ForceToAdd) * MagnitudeRemaining);
            }

            return true;
        }

        //calculates the component of the steering force that is parallel
        //with the vehicle heading
        public float ForwardComponent()
        {
            return Vector2.Dot(Entity.GetPhysicsData().Heading, _steeringForce);
        }

        //calculates the component of the steering force that is perpendicuar
        //with the vehicle heading
        public float SideComponent()
        {
            return Vector2.Dot(Entity.GetPhysicsData().Side, _steeringForce);
            //return Entity.GetPhysicsData().Side.Dot(_steeringForce);
        }

        public Vector2D GetForce()
        {
            return _steeringForce;
        }

        public void SetSummingMethod(summing_method sm)
        {
            _summingMethod = sm;
        }

        //public SteeringBehaviorData Serialize()
        //{
        //    var data = new SteeringBehaviorData();

        //    if (TargetAgent1 != null)
        //        data.TargetAgent1 = TargetAgent1.UniqueId;
        //    else
        //        data.TargetAgent1 = string.Empty;
        //    if (TargetAgent2 != null)
        //        data.TargetAgent2 = TargetAgent2.UniqueId;
        //    else
        //        data.TargetAgent2 = string.Empty;
        //    data.Offset = Offset;

        //    //the current target
        //    data.Target = Target;

        //    var activeBehaviorTypes = new List<behavior_type>();

        //    foreach (var behavior in _steeringBehaviors)
        //    {
        //        if (behavior.Value.IsOn())
        //        {
        //            activeBehaviorTypes.Add(behavior.Key);
        //        }
        //        behavior.Value.Serialize(data);
        //    }

        //    data.ActiveBehaviors = activeBehaviorTypes;

        //    return data;
        //}

        //public void Deserialize(SteeringBehaviorData data)
        //{
        //    //var entityMap = EntityController._instance.GetEntityMap();

        //    // TODO Fix this, we have an issue with locating a remote entity with just a component GUID
        //    // Not an entity GUID
        //    //if (data.TargetAgent1 == string.Empty)
        //    //    TargetAgent1 = null;
        //    //else
        //    //    TargetAgent1 = (IKineticObject) _entityResolver.GetEntity(entityMap[data.TargetAgent1]);

        //    //if (data.TargetAgent2 == -1)
        //    //    TargetAgent2 = null;
        //    //else
        //    //    TargetAgent2 = (IKineticObject) _entityResolver.GetEntity(entityMap[data.TargetAgent2]);
        //    Target = data.Target;
        //    Offset = data.Offset;

        //    foreach (var behavior in _steeringBehaviors)
        //    {
        //        behavior.Value.Deserialize(data);
        //        behavior.Value.TurnOff();
        //    }

        //    data.ActiveBehaviors.ForEach(i => TurnOn(i));
        //}

        public IBasicPhysicsComponent GetTargetAgent1()
        {
            return TargetAgent1;
        }

        public Vector3 GetOffset()
        {
            return Offset;
        }

        public void OnDrawGizmos()
        {
            if (_steeringBehaviors == null)
                return;
            foreach (KeyValuePair<behavior_type, ISteeringBehavior> behavior in _steeringBehaviors)
            {
                if (!behavior.Value.IsOn())
                    continue;
                behavior.Value.OnDrawGizmos();
            }
        }
    }
}
