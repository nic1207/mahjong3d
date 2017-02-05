using JUMP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiceRollerSample
{
    // Custom Commands
    public class DiceRollerCommand_RollDice : JUMPCommand
    {
        public const byte RollDice_EventCode = 100;

        public int PlayerID { get { return (int)CommandData[0]; } set { CommandData[0] = value; } }
        public int RolledDiceValue { get { return (int)CommandData[1]; } set { CommandData[1] = value; } }

        // Create a command to send with this initializer
        public DiceRollerCommand_RollDice(int playerID, int rolledDiceValue) : base(new object[2], RollDice_EventCode)
        {
            PlayerID = playerID;
            RolledDiceValue = rolledDiceValue;
        }

        // Create a command when receiving it from Photon
        public DiceRollerCommand_RollDice(object[] data) : base(data, RollDice_EventCode)
        {
        }
    }

}
