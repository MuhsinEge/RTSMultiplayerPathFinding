using Photon.Deterministic;
using Quantum.RTS.Filters;
using Quantum.RTS.PathFinding;
using System.Collections.Generic;
using static Quantum.RTS.InputHandler;

namespace Quantum.RTS
{
    public unsafe class CharacterMover : SystemMainThread
    {
        private const int InvalidValue = -1;

        private bool ShouldSkipCharacterMovement(CharacterLink* characterLink)
        {
            return characterLink->targetLine == InvalidValue || characterLink->targetGrid == InvalidValue;
        }

        private void MoveCharacterTowards(Frame f, Transform3D* characterTransform, FPVector3 targetPosition)
        {
            var direction = targetPosition - characterTransform->Position;
            direction = direction.Normalized;
            characterTransform->Position += direction * f.DeltaTime * 5;
        }

        private void HandleNoPathFound(Frame f, CharacterLink* characterLink, Transform3D* characterTransform, EntityRef entity)
        {
            characterLink->targetLine = InvalidValue;
            characterLink->targetGrid = InvalidValue;
            f.Events.CharacterTargetEvent(1, characterLink->teamId, characterLink->playerIndex, characterLink->targetLine, characterLink->targetGrid);

            if (f.Unsafe.TryGetPointer(characterLink->currentGridRef, out Transform3D* currentGridTransform))
            {
                var targetPosition = new FPVector3(currentGridTransform->Position.X, characterTransform->Position.Y, currentGridTransform->Position.Z);
                characterTransform->Position = targetPosition;
            }
        }

        private void HandlePathFound(Frame f, CharacterLink* characterLink, Transform3D* characterTransform, List<EntityRef> path, Grid* currentGridData)
        {
            var targetGrid = path[1];
            if (f.Unsafe.TryGetPointer(targetGrid, out Transform3D* targetGridTransform))
            {
                var targetPosition = new FPVector3(targetGridTransform->Position.X, characterTransform->Position.Y, targetGridTransform->Position.Z);
                var distance = FPVector3.Distance(characterTransform->Position, targetPosition);

                if (distance < FP._0_05)
                {
                    HandleDestinationReached(f, targetPosition, characterLink, characterTransform, path, targetGrid, currentGridData);
                }
                else
                {
                    MoveCharacterTowards(f, characterTransform, targetPosition);
                }
            }
        }

        private void HandleDestinationReached(Frame f, FPVector3 targetPosition, CharacterLink* characterLink, Transform3D* characterTransform, List<EntityRef> path, EntityRef targetGrid, Grid* currentGridData)
        {
            currentGridData->isOccupied = false;
            f.Events.GridDataEvent(currentGridData->line, currentGridData->index, false, false, -1);
            if (f.Unsafe.TryGetPointer(targetGrid, out Grid* targetGridData))
            {
                targetGridData->isOccupied = true;

                if (targetGridData->isCollectable)
                {
                    targetGridData->isCollectable = false;
                }

                f.Events.GridDataEvent(targetGridData->line, targetGridData->index, targetGridData->isOccupied, targetGridData->isCollectable,characterLink->teamId);
            }
            characterLink->currentGridRef = targetGrid;
            characterTransform->Position = targetPosition;
        }

        private void HandlePathFinding(Frame f, GridDataLink* gridData, CharacterLink* characterLink, EntityRef characterEntity)
        {
            var currentGrid = characterLink->currentGridRef;
            if (f.Unsafe.TryGetPointer(currentGrid, out Grid* currentGridData))
            {
                if (f.Unsafe.TryGetPointer(characterEntity, out Transform3D* characterTransform))
                {
                    var path = PathFinder.FindPath(f, gridData, currentGridData->line, currentGridData->index, characterLink->targetLine, characterLink->targetGrid);
                    if (path == null || path.Count <= 1)
                    {
                        HandleNoPathFound(f, characterLink, characterTransform, characterEntity);
                    }
                    else
                    {
                        HandlePathFound(f, characterLink, characterTransform, path, currentGridData);
                    }
                }
            }
        }

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
                    if (f.Unsafe.TryGetPointer(characterStruct.entity, out CharacterLink* link))
                    {
                        if (ShouldSkipCharacterMovement(link))
                        {
                            continue;
                        }
                       
                        HandlePathFinding(f, gridStruct.gridData, link, characterStruct.entity);
                    }

                }
            }
        }
    }
}
