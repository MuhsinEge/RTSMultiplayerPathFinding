using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Quantum.RTS.CharacterMover;

namespace Quantum.RTS
{
    public unsafe class InputHandler : SystemMainThreadFilter<InputHandler.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            Input input = default;
            input = *f.GetPlayerInput(filter.character->teamId);
            if (input.character != EntityRef.None)
            {
                if (input.selectedGrid != -1 && input.selectedGrid != -1)
                {
                    if (f.Unsafe.TryGetPointer(input.character, out CharacterLink* characterLink))
                    {
                        characterLink->targetLine = input.selectedLine;
                        characterLink->targetGrid = input.selectedGrid;
                    }
                }
            }
        }
        public struct Filter
        {
            public EntityRef entity;
            public CharacterLink* character;
        }
    }

   
}
