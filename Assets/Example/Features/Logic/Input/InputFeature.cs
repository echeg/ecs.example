using ME.ECS;
using UnityEngine;

namespace Example.Features.Logic {

    using Components; using Modules; using Systems; using Features; using Markers;
    using Input.Components; using Input.Modules; using Input.Systems; using Input.Markers;
    
    namespace Input.Components {}
    namespace Input.Modules {}
    namespace Input.Systems {}
    namespace Input.Markers {}
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class InputFeature : Feature
    {

        public bool EnableRandomInput;
        [Range(0, 100)]
        public int PercentRandomMove;
        [Range(0, 100)]
        public int PercentRandomFire;
        
        private RPCId movePlayerRpcId;
        private RPCId fireRpcId;
        
        protected override void OnConstruct() {

            this.AddModule<KeyboardInputModule>();
            this.AddModule<RandomInputModule>();
            this.AddSystem<MarkerToRPCSystem>();

            var net = this.world.GetModule<NetworkModule>();
            net.RegisterObject(this, 2);
            this.movePlayerRpcId = net.RegisterRPC(new System.Action<int, InputDir>(this.MovePlayer_RPC).Method);
            this.fireRpcId = net.RegisterRPC(new System.Action<int, InputFire>(this.Fire_RPC).Method);

        }

        protected override void OnDeconstruct() {
            
        }

        public void SendDir(InputDir dir) {
            
            var net = this.world.GetModule<NetworkModule>();
            var playersFeature = this.world.GetFeature<PlayersFeature>();
            net.RPC(this, this.movePlayerRpcId, playersFeature.GetActivePlayerId(), dir);
            
        }

        public void SendFire(InputFire fire) {
            
            var net = this.world.GetModule<NetworkModule>();
            var playersFeature = this.world.GetFeature<PlayersFeature>();
            net.RPC(this, this.fireRpcId, playersFeature.GetActivePlayerId(), fire);

        }

        private void MovePlayer_RPC(int actorId, InputDir dir) {

            var playersFeature = this.world.GetFeature<PlayersFeature>();
            var playerEntity = playersFeature.GetEntityByActorId(actorId);

            playerEntity.SetData(new Example.Features.PlayerMovement.Components.MoveAction() {
                dir = dir.Dir
            }, ComponentLifetime.NotifyAllSystems);

        }

        private void Fire_RPC(int actorId, InputFire fire) {
            
            var playersFeature = this.world.GetFeature<PlayersFeature>();
            var playerEntity = playersFeature.GetEntityByActorId(actorId);
            
            playerEntity.SetData(new Example.Features.PlayerFire.Components.FireAction() {
            }, ComponentLifetime.NotifyAllSystems);

        }

    }

}