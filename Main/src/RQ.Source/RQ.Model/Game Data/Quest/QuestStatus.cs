using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RQ.Model.Game_Data.Quest
{
    public enum QuestStatus
    {
        Unassigned = 0,
        Active = 1,
        Success = 2,
        Failure = 3,
        Abandoned = 4
    }
}
