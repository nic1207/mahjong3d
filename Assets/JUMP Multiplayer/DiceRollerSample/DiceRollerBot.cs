using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JUMP;

namespace DiceRollerSample
{
    public class DiceRollerBot : JUMPPlayer, IJUMPBot
    {
        public int Score = 0;

        private TimeSpan TickTimer = TimeSpan.Zero;
        private TimeSpan CommandsFrequency = TimeSpan.FromMilliseconds(1000 / 0.3);

        public IJUMPGameServerEngine Engine { get; set; }

        public void Tick(double ElapsedSeconds)
        {
            TickTimer += TimeSpan.FromSeconds(ElapsedSeconds);
            if (TickTimer > CommandsFrequency)
            {
                TickTimer = TimeSpan.Zero;

                // Every 3 seconds, roll a 3
                DiceRollerEngine engine = Engine as DiceRollerEngine;

                engine.ProcessCommand(new DiceRollerCommand_RollDice(PlayerID, 3));
            }
        }
    }
}
