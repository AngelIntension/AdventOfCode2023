using System.Collections.Immutable;
using FluentAssertions;
using WaitForIt;

using static WaitForIt.BoatRace;
// ReSharper disable InconsistentNaming

namespace AdventTest; 

public class BoatRaceTest {
    [Theory]
    [InlineData(7, 0, 0)]
    [InlineData(7, 1, 6)]
    [InlineData(7, 2, 10)]
    [InlineData(7, 3, 12)]
    [InlineData(7, 4, 12)]
    [InlineData(7, 5, 10)]
    [InlineData(7, 6, 6)]
    [InlineData(7, 7, 0)]
    public static void GetTraveled_mm_ShouldComputeCorrectDistance
        (int race_ms, int charge_ms, int expected_mm) {
        var traveled_mm = GetTraveled_mm(race_ms, charge_ms);
        traveled_mm.Should().Be(expected_mm);
    }

    [Theory]
    [InlineData(7, 9, 4)]
    [InlineData(15, 40, 8)]
    [InlineData(30, 200, 9)]
    public static void GetWinningCharge_ms_ShouldReturnCorrectNumberOfChargeSpans
        (int race_ms, int record_mm, int expectedCount) {
        var race = new BoatRaceState(race_ms, record_mm);
        var winningChargeTimes_ms = race.GetWinningCharge_ms();
        winningChargeTimes_ms.Count.Should().Be(expectedCount);
    }

    [Fact]
    public static void ProductOfWinningCharge_msCounts_GivenTestData_ShouldBeExpected() {
        var races = new BoatRaceState[] {
            new(7, 9),
            new(15, 40),
            new(30, 200)
        }.ToImmutableArray();
        var productOfWinningChargeTimeCounts
            = races.Aggregate(1, (product, race) => product * race.GetWinningCharge_ms().Count);
        productOfWinningChargeTimeCounts.Should().Be(288);
    }

    [Fact]
    public static void ProductOfWinningCharge_msCounts_GivenInputData_ShouldBeExpected() {
        var races = new BoatRaceState[] {
            new(61, 430),
            new(67, 1036),
            new(75, 1307),
            new(71, 1150)
        }.ToImmutableArray();
        var productOfWinningChargeTimeCounts
            = races.Aggregate(1, (product, race) => product * race.GetWinningCharge_ms().Count);
        productOfWinningChargeTimeCounts.Should().Be(316800);
    }
    
    [Fact]
    public static void ProductOfWinningCharge_msCounts_GivenGoodKerning_ShouldBeExpected() {
        var races = new BoatRaceState[] { new(71530, 940200) }.ToImmutableArray();
        var productOfWinningChargeTimeCounts
            = races.Aggregate(1, (product, race) => product * race.GetWinningCharge_ms().Count);
        productOfWinningChargeTimeCounts.Should().Be(71503);
    }
}