﻿using Mundialito.DAL.GeneralBets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mundialito.Models
{

    public class GeneralBetViewModel
    {
        public GeneralBetViewModel(GeneralBet bet)
        {
            GeneralBetId = bet.GeneralBetId;
            WinningTeam = bet.WinningTeam.Name;
            WinningTeamId = bet.WinningTeam.TeamId;
            GoldenBootPlayer = bet.GoldBootPlayer;
            IsResolved = bet.IsResolved;
            if (IsResolved)
            {
                Points = bet.PlayerPoints.Value + bet.TeamPoints.Value;
            }
        }

        public int GeneralBetId { get; set; }

        public String WinningTeam { get; set; }

        public int  WinningTeamId { get; set; }

        public String GoldenBootPlayer { get; set; }

        public Boolean IsResolved { get; set; }

        public int Points { get; set; }
    }

    public class NewGeneralBetModel
    {
        public int TeamId { get; set; }

        public string Player { get; set; }

        public int GenrealBetId { get; set; }
    }

    public class UpdateGenralBetModel
    {
        public int TeamId { get; set; }

        public string Player { get; set; }
    }

    public class ResolveGeneralBetModel
    {
        public Boolean PlayerIsRight { get; set; }

        public Boolean TeamIsRight { get; set; }
    }
}