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

            if (grid.Next(&gridStruct))
            {
                while (characters.Next(&characterStruct))
                {
                    if (characterStruct.characterLink->targetLine == -1 || characterStruct.characterLink->targetGrid == -1)
                    {
                        continue;
                    }
                    var currentGrid = characterStruct.characterLink->currentGridRef;
                    if(f.Unsafe.TryGetPointer(currentGrid,out Grid* currentGridData))
                    {
                        if (f.Unsafe.TryGetPointer(characterStruct.entity, out Transform3D* transform))
                        {
                            var path = PathFinder.FindPath(f, gridStruct.gridData, currentGridData->line, currentGridData->index, characterStruct.characterLink->targetLine, characterStruct.characterLink->targetGrid);
                            if (path == null)
                            {
                                characterStruct.characterLink->targetLine = -1;
                                characterStruct.characterLink->targetGrid = -1;
                                if (f.Unsafe.TryGetPointer(characterStruct.characterLink->currentGridRef, out Transform3D* currentGridTransform))
                                {
                                    transform->Position = currentGridTransform->Position;
                                }
                            }
                            else
                            {
                                var targetGrid = path[1];
                                if (f.Unsafe.TryGetPointer(targetGrid, out Transform3D* targetGridTransform))
                                {
                                    var targetPosition = new FPVector3(targetGridTransform->Position.X, transform->Position.Y, targetGridTransform->Position.Z);
                                    var distance = FPVector3.Distance(transform->Position, targetPosition);
                                    if (distance < FP._0_03)
                                    {
                                        if (path.Count == 2 && path[0] == characterStruct.characterLink->currentGridRef)
                                        {
                                            characterStruct.characterLink->targetLine = -1;
                                            characterStruct.characterLink->targetGrid = -1;
                                        }
                                        currentGridData->isObstacle = false;
                                        if (f.Unsafe.TryGetPointer(targetGrid, out Grid* targetGridData))
                                        {
                                            targetGridData->isObstacle = true;
                                        }
                                        characterStruct.characterLink->currentGridRef = targetGrid;
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
    }
}
