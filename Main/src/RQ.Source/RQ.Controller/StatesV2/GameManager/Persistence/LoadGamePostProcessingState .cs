//using Sprites = RQ.Entity.Sprites;
using RQ.FSM.V2;
using UnityEngine;


namespace RQ.Entity.StatesV2
{
    [AddComponentMenu("RQ/States/State/Game Manager/Load Game Post Processing")]
    public class LoadGamePostProcessingState : StateBase
    {
        public override void Enter()
        {
            //GameController._instance.LoadGame("Test.dat");
        }

        public override void Exit()
        {
            //Log.Info("Exiting Load Game State");
        }

        // The reason this is in Update and not Enter is because we want a frame skip so
        // the entities get properly registered.
        public override void Update()
        {
            base.Update();
            if (!Application.isPlaying)
                return;
            if (IsComplete())
                return;

            PostProcessing();
            Complete();
        }

        /// <summary>
        /// The game has been loaded and the level refreshed.
        /// Since the game was just loaded, we want to destroy every sprite in the scene and reload them
        /// from the loaded data to make sure everything starts fresh.
        /// This process also makes debugging easier since every variable in the sprite is reset.
        /// </summary>
        public void PostProcessing()
        {
            //try
            //{
            //    GameStateController.Instance.Deserialize();
            //}
            //catch (Exception ex)
            //{
            //    Debug.LogError(ex);
            //}
            //GameDataController.Instance.LoadingGame = false;
        }
    }
}
