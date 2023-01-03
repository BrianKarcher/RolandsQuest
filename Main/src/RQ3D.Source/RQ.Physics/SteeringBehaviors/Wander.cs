using UnityEngine;

namespace RQ.Physics.SteeringBehaviors
{
    public class Wander : SteeringBehaviorBase, ISteeringBehavior
    {
        //the radius of the constraining circle for the wander behavior
        private const float WanderRad = 1.2f;
        //private const float WanderRad = .04f;
        //private const float WanderRad = 5f;
        //distance the wander circle is projected in front of the agent
        private const float WanderDist = 3.0f;
        //private const float WanderDist = .8f;
        //private const float WanderDist = 10f;
        //the maximum amount of displacement along the circle each frame
        //private const float WanderJitterPerSec = 80f;
        private const float WanderJitterPerSec = 10f;
        //private const float WanderJitterPerSec = .5f;
        //private const float WanderJitterPerSec = 1f;
        private Vector2D last_wander_pos = Vector2D.Zero();

        private Transform _transform;

        //the current position on the wander circle the agent is
        //attempting to steer towards
        private Vector2D _wanderTarget = new Vector2D(0,0);

        //explained above
        private float _wanderJitter;
        private float _wanderRadius;
        private float _wanderDistance;

        public Wander(SteeringBehaviorManager manager, Transform transform)
            : base(manager)
        {
            _transform = transform;
            _constantWeight = Constants.WanderWeight;
            _wanderDistance = WanderDist;
            _wanderJitter = WanderJitterPerSec;
            _wanderRadius = WanderRad;
            float theta = UnityEngine.Random.Range(0f, 1f) * Mathf.PI * 2;
            //create a vector to a target position on the wander circle
            _wanderTarget = new Vector2D(_wanderRadius * Mathf.Cos(theta),
                                        _wanderRadius * Mathf.Sin(theta));


        }        

        //this behavior makes the agent wander about randomly
        protected override Vector2D CalculateForce()
        {
            return SteeringBehaviorCalculations.Wander(_steeringBehaviorManager.Entity, ref _wanderTarget, _wanderJitter,
                _wanderRadius, _wanderDistance, _transform, ref last_wander_pos);
        }

        public override void OnDrawGizmos()
        {
            var entity = _steeringBehaviorManager.Entity;
            //calculate the center of the wander circle
            Vector2D m_vTCC = Transformations.PointToWorldSpace(new Vector2D(_wanderDistance /** m_pVehicle->BRadius()*/, 0),
                                     entity.GetPhysicsData().Heading,
                                                 entity.GetPhysicsData().Side,
                                                 entity.GetWorldPos());
            //draw the wander circle
            //Debug.
            Gizmos.color = new Color(0, 255, 0, .3f);
            Gizmos.DrawSphere(m_vTCC.ToVector3(entity.transform.position.z), _wanderRadius);
            //gdi->GreenPen();
            //gdi->HollowBrush();
            //gdi->Circle(m_vTCC, m_dWanderRadius * m_pVehicle->BRadius());

            //draw the wander target
            Gizmos.color = new Color(255, 0, 0, .3f);
            //gdi->RedPen();
            var targetPos = Transformations.PointToWorldSpace((_wanderTarget + new Vector2D(_wanderDistance, 0)) /** m_pVehicle->BRadius() */,
                                          entity.GetPhysicsData().Heading,
                                                 entity.GetPhysicsData().Side,
                                                 entity.GetWorldPos());
            Gizmos.DrawSphere(targetPos.ToVector3(entity.transform.position.z), 0.08f);
            //gdi->Circle(, .08f);
        }

        public float WanderJitter()
        {
            return _wanderJitter;
        }
        public float WanderDistance()
        {
            return _wanderDistance;
        }
        public float WanderRadius()
        {
            return _wanderRadius;
        }

        //public override void Serialize(SteeringBehaviorData data)
        //{
        //    data.last_wander_pos = last_wander_pos;
        //    data._wanderTarget = _wanderTarget;
        //}

        //public override void Deserialize(SteeringBehaviorData data)
        //{
        //    last_wander_pos = data.last_wander_pos;
        //    _wanderTarget = data._wanderTarget;
        //}
    }
}
