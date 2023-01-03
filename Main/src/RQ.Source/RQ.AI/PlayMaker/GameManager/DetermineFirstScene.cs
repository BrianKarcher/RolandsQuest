using PM = HutongGames.PlayMaker;
using HutongGames.PlayMaker;
using RQ.Animation.BasicAction.Action;
using RQ.Entity.Components;
using RQ.Physics.Components;
using UnityEngine;
using RQ.AI.Atom.GameManager;
using RQ.Common.Controllers;

namespace RQ.AI.PlayMaker
{
    [ActionCategory("RQ.GameManager")]
    [PM.Tooltip("Determine First Scene of application")]
    public class DetermineFirstScene : FsmStateAction
    {
        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when Scene Setup exists in the scene.")]
        public FsmEvent AppStart;

        [UIHint(UIHint.Variable)]
        [PM.Tooltip("Fire when Scene Setup doesn't exist in the scene.")]
        public FsmEvent AutoStart;

        //public AppStartAtom _atom;
        private IComponentRepository _entity;

        public override void OnEnter()
        {
            //var rqSM = Owner.GetComponent<PlayMakerStateMachineComponent>();
            //_entity = rqSM.GetComponentRepository();
            //_atom.Start(_entity);
            //Tick();
            //var sceneSetup = GameObject.FindObjectOfType<SceneSetup>();
            //var scene = GameDataController.Instance.AppStartScene;

            var gameConfig = GameController.Instance.GameConfig;
            if (!gameConfig.IsAutoStart)
            {
                Fsm.Event(AppStart);
                Finish();
                return;
            }

            // Auto starting
            Debug.Log($"(DetermineFirstSceneAtom) - AutoStarting, First Scene = {gameConfig.AutoStartSceneConfig.name}");
            GameDataController.Instance.NextSceneConfig = gameConfig.AutoStartSceneConfig;
            GameController.Instance.AppStart();
            GameController.Instance.BeginNewGame();
            Fsm.Event(AutoStart);

            Finish();
        }

        //public override void OnUpdate()
        //{
        //    Tick();
        //}

        //public override void OnExit()
        //{
        //    base.OnExit();
        //    _atom.End();
        //}

        //void Tick()
        //{
        //    var result = _atom.OnUpdate();
        //    if (result == RQ.AI.AtomActionResults.Success)
        //    {
        //        Finish();
        //    }
        //}
    }
}
