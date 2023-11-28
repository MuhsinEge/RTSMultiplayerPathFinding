using Photon.Deterministic;
using Quantum.RTS.Filters;
using Quantum.RTS.PathFinding;
using System.Collections.Generic;
using System.IO;

namespace Quantum.RTS
{
    public unsafe class CharacterMover : SystemMainThread
    {

        private bool ShouldSkipCharacterMovement(CharacterFilter characterStruct) {

            if (characterStruct.characterLink->targetLine == -1 || characterStruct.characterLink->targetGrid == -1)
            {
                return true;
            }
            return false;
        }

        public void HandlePathFinding(Frame f,GridFilter gridStruct, CharacterFilter characterStruct)
        {
            var currentGrid = characterStruct.characterLink->currentGridRef;
            if (f.Unsafe.TryGetPointer(currentGrid, out Grid* currentGridData))
            {
                if (f.Unsafe.TryGetPointer(characterStruct.entity, out Transform3D* characterTransform))
                {
                    var path = PathFinder.FindPath(f, gridStruct.gridData, currentGridData->line, currentGridData->index, characterStruct.characterLink->targetLine, characterStruct.characterLink->targetGrid);
                    if (path == null)
                    {
                        HandleNoPathFound(f, characterStruct, characterTransform);
                    }
                    else
                    {
                        HandlePathFound(f,characterStruct,characterTransform,path,currentGridData);
                    }
                }
            }
        }

        private void HandleNoPathFound(Frame f, CharacterFilter characterStruct, Transform3D* characterTransform)
        {
            characterStruct.characterLink->targetLine = -1;
            characterStruct.characterLink->targetGrid = -1;
            if (f.Unsafe.TryGetPointer(characterStruct.characterLink->currentGridRef, out Transform3D* currentGridTransform)) // CAN BE IMPROVED
            {
                characterTransform->Position = currentGridTransform->Position;
            }
        }

        private void HandlePathFound(Frame f,CharacterFilter characterStruct, Transform3D* characterTransform, List<EntityRef> path, Grid* currentGridData)
        {
            var targetGrid = path[1];
            if (f.Unsafe.TryGetPointer(targetGrid, out Transform3D* targetGridTransform))
            {
                var targetPosition = new FPVector3(targetGridTransform->Position.X, characterTransform->Position.Y, targetGridTransform->Position.Z);
                var distance = FPVector3.Distance(characterTransform->Position, targetPosition);
                if (distance < FP._0_03)
                {
                    HandleDestinationReached(f,targetPosition,characterStruct,characterTransform,path,targetGrid,currentGridData);
                }
                else
                {
                    MoveCharacter(f,characterTransform, targetPosition, distance);
                }
               
            }
        }

        private void HandleDestinationReached(Frame f, FPVector3 targetPosition, CharacterFilter characterStruct, Transform3D* characterTransform, List<EntityRef> path, EntityRef targetGrid, Grid* currentGridData)
        {
            if (path.Count == 2 && path[0] == characterStruct.characterLink->currentGridRef)
            {
                characterStruct.characterLink->targetLine = -1;
                characterStruct.characterLink->targetGrid = -1;
            }
            currentGridData->isOccupied = false;
            f.Events.GridDataEvent(currentGridData->line, currentGridData->index);

            if (f.Unsafe.TryGetPointer(targetGrid, out Grid* targetGridData))
            {
                targetGridData->isOccupied = true;
                f.Events.GridDataEvent(targetGridData->line, targetGridData->index);
            }
            characterStruct.characterLink->currentGridRef = targetGrid;
            characterTransform->Position = targetPosition;
        }

        private void MoveCharacter(Frame f,Transform3D* characterTransform, FPVector3 targetPosition, FP distance)
        {
            var direction = targetPosition - characterTransform->Position;
            direction = direction.Normalized;
            characterTransform->Position += direction / f.UpdateRate * FP._3;
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
                    if (ShouldSkipCharacterMovement(characterStruct))
                    {
                        continue;
                    }
                    HandlePathFinding(f,gridStruct,characterStruct);
                }
            }
        }
    }
}
