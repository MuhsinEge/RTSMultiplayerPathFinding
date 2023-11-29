using Quantum.RTS.Filters;

namespace Quantum.RTS
{
    public unsafe class InputHandler : SystemMainThreadFilter<CharacterFilter> 
    {
        public override void Update(Frame f, ref CharacterFilter filter)
        {
            Input input = default;
            var characterLink = filter.characterLink;
            input = *f.GetPlayerInput(characterLink->teamId);
            if (input.character == filter.entity)
            {
                Log.Debug("Girdi2");
                if (input.selectedGrid != -1 && input.selectedGrid != -1)
                {
                    Log.Debug("Girdi3");
                    characterLink->targetLine = input.selectedLine;
                    characterLink->targetGrid = input.selectedGrid;
                    f.Events.CharacterTargetEvent(0,characterLink->teamId, characterLink->playerIndex, characterLink->targetLine, characterLink->targetGrid);
                }
            }
        }
    }

   
}
