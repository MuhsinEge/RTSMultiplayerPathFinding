using Photon.Deterministic;
using Quantum.RTS.Filters;
using Quantum.RTS.PathFinding;
using static Quantum.RTS.InputHandler;

namespace Quantum.RTS
{
    public unsafe class CharacterMover : SystemMainThread
    {
        public override void Update(Frame f)
        {
            var grid = f.Unsafe.FilterStruct<GridFilter>();
            var gridStruct = default(GridFilter);

            var characters = f.Unsafe.FilterStruct<CharacterFilter>();
            var characterStruct = default(CharacterFilter);

          //  Log.Debug("Girdi 1");
            if (grid.Next(&gridStruct))
            {
               // Log.Debug("Girdi 2");
                while (characters.Next(&characterStruct))
                {
                   // Log.Debug("Girdi 3");
                    if (characterStruct.characterLink->targetLine == -1 || characterStruct.characterLink->targetGrid == -1)
                    {
                        continue;
                    }
                    PathFinder.FindPath(f, gridStruct.gridData, 0, 8, characterStruct.characterLink->targetLine, characterStruct.characterLink->targetGrid);
                    if (f.Unsafe.TryGetPointer(characterStruct.entity, out Transform3D* transform))
                    {
                        var targetGrid = gridStruct.gridData->gridLayout[characterStruct.characterLink->targetLine].grids[characterStruct.characterLink->targetGrid];
                        if (f.Unsafe.TryGetPointer(targetGrid, out Transform3D* targetGridTransform))
                        {
                            var targetPosition = new FPVector3(targetGridTransform->Position.X, transform->Position.Y, targetGridTransform->Position.Z);
                            var distance = FPVector3.Distance(transform->Position, targetPosition);
                            if (distance < FP._0_03)
                            {
                                characterStruct.characterLink->targetLine = -1;
                                characterStruct.characterLink->targetGrid = -1;
                                transform->Position = targetPosition;
                                continue;
                            }
                            var direction = targetPosition - transform->Position;
                            direction = direction.Normalized;
                            transform->Position += direction / f.UpdateRate * FP._3;
                        }
                    }
                }
            }
        }
    }
}
