using Quantum.RTS.Filters;

namespace Quantum.RTS
{
    public unsafe class InputHandler : SystemMainThreadFilter<CharacterFilter> 
    {
        public override void Update(Frame f, ref CharacterFilter filter)
        {
            Input input = default;

            input = *f.GetPlayerInput(filter.characterLink->teamId);
            if (input.character == filter.entity)
            {
                if (input.selectedGrid != -1 && input.selectedGrid != -1)
                {
                   
                       filter.characterLink->targetLine = input.selectedLine;
                       filter.characterLink->targetGrid = input.selectedGrid;
                }
            }
        }
    }

   
}
