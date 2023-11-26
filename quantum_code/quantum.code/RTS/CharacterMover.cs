using Photon.Deterministic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantum.RTS
{
    public unsafe class CharacterMover : SystemMainThreadFilter<CharacterMover.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            if (filter.character->targetLine == -1 || filter.character->targetGrid == -1)
            {
                return;
            }
            
            if(f.Unsafe.TryGetPointer(filter.entity,out Transform3D* transform) ) {
                var targetPosition = new FPVector3(filter.character->targetLine, transform->Position.Y, filter.character->targetGrid);
                var distance = FPVector3.Distance(transform->Position, targetPosition);
                if(distance < FP._0_03)
                {
                    filter.character->targetLine = -1;
                    filter.character->targetGrid = -1;
                    return;
                }
                
                var direction = targetPosition - transform->Position;
                direction = direction.Normalized;
                transform->Position += direction / f.UpdateRate * FP._3;
            }
        }

        public struct Filter
        {
            public EntityRef entity;
            public CharacterLink* character;
        }
    }
}
