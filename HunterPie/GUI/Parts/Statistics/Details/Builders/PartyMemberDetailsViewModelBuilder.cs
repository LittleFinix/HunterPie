using Avalonia.Media;
using Avalonia.Skia;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Services;
using HunterPie.Features.Statistics.Models;
using HunterPie.GUI.Parts.Statistics.Details.ViewModels;
using HunterPie.UI.Architecture.Brushes;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Painting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace HunterPie.GUI.Parts.Statistics.Details.Builders;

internal class PartyMemberDetailsViewModelBuilder
{
    public static PartyMemberDetailsViewModel? Build(
        HuntStatisticsModel quest,
        MonsterModel monster,
        PartyMemberModel player
    )
    {
        if (monster.HuntStartedAt is not { } startedAt || monster.HuntFinishedAt is not { } finishedAt)
            return null;

        double totalPartyDamage = quest.Players.SelectMany(it => it.Damages)
            .Where(it => it.DealtAt.IsBetween(startedAt, finishedAt))
            .Sum(it => it.Damage);

        float accumulatedDamage = 0;
        IEnumerable<ObservablePoint> damageFrames = player.Damages.Where(it => it.DealtAt.IsBetween(startedAt, finishedAt))
            .Select(it => (it.DealtAt, Damage: accumulatedDamage = it.Damage + accumulatedDamage))
            .Select(it =>
              {
                  double time = Math.Max(1.0, (it.DealtAt - quest.StartedAt).TotalSeconds);

                  return new ObservablePoint
                  {
                      X = time,
                      Y = it.Damage / time
                  };
              });

        var abnormalities =
            player.Abnormalities.Select(it => BuildAbnormality(quest, monster, it))
                                .FilterNull()
                                .ToObservableCollection();

        var damagePoints = new ChartValues(damageFrames);
        Color color = RandomColor();

        return new PartyMemberDetailsViewModel
        {
            Name = player.Name,
            Weapon = player.Weapon,
            Damage = accumulatedDamage,
            Damages = new LineSeries
            {
                // Title = player.Name,
                Stroke = new SolidColorPaint(color.ToSKColor())
                {
                    StrokeThickness = 2
                },
                Fill = ColorFadeGradient.FromColor(color),
                LineSmoothness = 1,
                Values = damagePoints
            },
            Contribution = accumulatedDamage / totalPartyDamage,
            Color = new SolidColorBrush(color),
            Abnormalities = abnormalities
        };
    }

    private static AbnormalityDetailsViewModel? BuildAbnormality(
        HuntStatisticsModel quest,
        MonsterModel monster,
        AbnormalityModel abnormality
    )
    {
        if (monster.HuntFinishedAt is not { } finishedAt || monster.HuntStartedAt is not { } startedAt)
            return null;

        AbnormalitySchema? schema = AbnormalityService.FindBy(quest.Game, abnormality.Id);

        if (schema is not { } abnormalityData)
            return null;

        double questTime = (finishedAt - startedAt).TotalSeconds;

        var timeFrames = abnormality.Activations
            .Where(it => (it.StartedAt >= startedAt || it.FinishedAt >= startedAt) && it.StartedAt < finishedAt)
            .ToImmutableArray();

        double upTime = timeFrames.Sum(it => (it.FinishedAt - it.StartedAt.Max(startedAt)).TotalSeconds);

        Color color = RandomColor();
        //
        // var activations = timeFrames.Select(it => new AxisSection
        // {
        //     StrokeThickness = 1,
        //     StrokeDashArray = new DoubleCollection { 4, 4 },
        //     Stroke = new SolidColorBrush(color) { Opacity = 0.60 },
        //     Fill = new SolidColorBrush(color) { Opacity = 0.35 },
        //     Value = (it.StartedAt - quest.StartedAt).TotalSeconds,
        //     SectionWidth = (it.FinishedAt - it.StartedAt).TotalSeconds,
        // }).ToList();

        return new AbnormalityDetailsViewModel
        {
            Name = abnormalityData.Name,
            Icon = abnormalityData.Icon,
            Color = ColorFadeGradient.MakeBrush(color),
            UpTime = Math.Min(1.0, upTime / questTime),
            Activations = new List<AxisSection>() // activations
        };
    }

    private static Color RandomColor()
    {
        Random rng = new();
        byte r = (byte)rng.Next(115, 256);
        byte g = (byte)rng.Next(180, 256);
        byte b = (byte)rng.Next(120, 256);
        return Color.FromRgb(r, g, b);
    }
}
