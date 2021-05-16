using ME.ECS;

namespace Example.Features.Logic.Input.Markers {
    
    public struct InputDir : IMarker {

        public FPVector3 Dir;

        public InputDir(FPVector3 dir)
        {
            this.Dir = dir;
        }
    }
    
}