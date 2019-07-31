﻿using System.Collections.Generic;

namespace CountersPlus.Config
{
    internal class ConfigDefaults
    {
        internal static Dictionary<string, ConfigModel> Defaults = new Dictionary<string, ConfigModel>()
        {
            { "Missed", new MissedConfigModel(){ Enabled = true, Position = ICounterPositions.BelowCombo, Distance = 0, CustomMissTextIntegration = false } },
            { "Notes", new NoteConfigModel() { Enabled = false, Position = ICounterPositions.BelowCombo, Distance = 1, DecimalPrecision = 2, ShowPercentage = false} },
            { "Progress", new ProgressConfigModel() { Enabled = true, Position = ICounterPositions.BelowEnergy, Distance = 0, Mode = ICounterMode.Original, ProgressTimeLeft = false, IncludeRing = false } },
            { "Score", new ScoreConfigModel() { Enabled = true, Position = ICounterPositions.BelowMultiplier, Distance = 0, DecimalPrecision = 2, DisplayRank = true, Mode = ICounterMode.Original } },
            { "Speed", new SpeedConfigModel() { Enabled = false, Position = ICounterPositions.BelowMultiplier, Distance = 1, DecimalPrecision = 2, Mode = ICounterMode.Average } },
            { "Cut", new CutConfigModel() { Enabled = false, Position = ICounterPositions.AboveHighway, Distance = 0, SeparateSaberCounts = false} },
            { "Spinometer", new SpinometerConfigModel() { Enabled = false, Position = ICounterPositions.AboveMultiplier, Distance = 0, Mode = ICounterMode.Highest } },
            { "Personal Best", new PBConfigModel() { Enabled = true, Position = ICounterPositions.BelowMultiplier, Distance = 1, DecimalPrecision = 2, TextSize = 2, UnderScore = true, HideFirstScore = false } },
            { "Notes Left", new NotesLeftConfigModel() { Enabled = false, Position = ICounterPositions.AboveHighway, Distance = -1, LabelAboveCount = false, } },
            { "Fail", new FailConfigModel() { Enabled = false, Position = ICounterPositions.AboveCombo, Distance = 0, ShowRestartsInstead = false, } },
        };

        internal static MainConfigModel MainDefaults { get
            {
                return new MainConfigModel
                {
                    Enabled = true,
                    AdvancedCounterInfo = true,
                    HideCombo = false,
                    HideMultiplier = false,
                    ComboOffset = 0.2f,
                    MultiplierOffset = 0.4f,
                };
            } }
    }
}
