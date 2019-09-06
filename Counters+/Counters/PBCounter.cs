﻿using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;
using CountersPlus.Config;

namespace CountersPlus.Counters
{
    public class PBCounter : Counter<PBConfigModel>
    {
        private ScoreController _scoreController;

        private GameplayModifiersModelSO gameplayModsModel;
        private GameplayModifiers gameplayMods;
        private PlayerLevelStatsData stats;

        private Color orange;
        private TMP_Text _PbTrackerText;
        private float beginningPB = 0;

        private int _maxPossibleScore = 0;
        private int highScore;

        internal override void Counter_Start()
        {
            ColorUtility.TryParseHtmlString("#FFA500", out orange);
        }

        internal override void Init(CountersData data)
        {
            _scoreController = data.ScoreController;
            PlayerDataModelSO player = data.PlayerData;
            gameplayModsModel = data.ModifiersData;
            gameplayMods = data.PlayerData.playerData.gameplayModifiers;
            IDifficultyBeatmap beatmap = data.GCSSD.difficultyBeatmap;
            stats = player.playerData.GetPlayerLevelStatsData(
                beatmap.level.levelID, beatmap.difficulty, beatmap.parentDifficultyBeatmapSet.beatmapCharacteristic);
            int maxRawScore = ScoreController.MaxRawScoreForNumberOfNotes(beatmap.beatmapData.notesCount);
            _maxPossibleScore = Mathf.RoundToInt(maxRawScore * gameplayModsModel.GetTotalMultiplier(gameplayMods));
            beginningPB = stats.highScore / (float)_maxPossibleScore;
            highScore = stats.highScore;

            Vector3 position = CountersController.DeterminePosition(gameObject, settings.Position, settings.Distance);
            TextHelper.CreateText(out _PbTrackerText, position);
            _PbTrackerText.fontSize = settings.TextSize;
            _PbTrackerText.color = Color.white;
            _PbTrackerText.alignment = TextAlignmentOptions.Center;
            
            _scoreController.scoreDidChangeEvent += UpdateScore;
            
            SetPersonalBest(beginningPB);

            if (settings.UnderScore) StartCoroutine(WaitForScoreCounter());
        }

        private IEnumerator WaitForScoreCounter()
        {
            ScoreCounter counter = CountersController.LoadedCounters.Where((GameObject x) => x?.GetComponent<ScoreCounter>() != null).FirstOrDefault()?.GetComponent<ScoreCounter>();
            if (counter == null) yield break;
            float offset = 0;
            yield return new WaitUntil(() => counter.PointsText != null);
            if (!(CountersController.settings.scoreConfig.Mode == ICounterMode.BaseGame || CountersController.settings.scoreConfig.Mode == ICounterMode.BaseWithOutPoints))
            {
                if (CountersController.settings.scoreConfig.DisplayRank)
                    offset = 3.35f;
                else
                    offset = 3.1f;
            }

            _PbTrackerText.rectTransform.SetParent(counter.PointsText.rectTransform);
            _PbTrackerText.rectTransform.localPosition = new Vector2(0, (TextHelper.ScaleFactor / 2) + (settings.TextSize / 10) + offset) * -1;
        }

        internal override void Counter_Destroy()
        {
            _scoreController.scoreDidChangeEvent -= UpdateScore;
        }
        
        public void SetPersonalBest(float pb)
        {
            //Force personal best percent to round down to decimal precision'
            try
            {
                pb = (float)Math.Round((decimal)pb * 100, settings.DecimalPrecision);
            }
            catch { pb = 0; } //yea something can go wrong here, like if you have all of the negative modifiers
            if (settings.HideFirstScore && stats.highScore == 0) _PbTrackerText.text = "PB: --";
            else _PbTrackerText.text = $"PB: {pb.ToString($"F{settings.DecimalPrecision}")}%";
        }

        public void UpdateScore(int score, int modifiedScore)
        {
            if (_maxPossibleScore != 0)
            {
                float ratio = modifiedScore / (float)_maxPossibleScore;
                if (modifiedScore > highScore)
                {
                    SetPersonalBest(ratio);
                    if (!(settings.HideFirstScore && stats.highScore == 0)) _PbTrackerText.color = Color.red;
                }
                else _PbTrackerText.color = Color.Lerp(Color.white, orange, modifiedScore / (float)highScore);
            }
        }
    }
}