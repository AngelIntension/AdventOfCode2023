using System.Collections.Immutable;
// ReSharper disable InconsistentNaming

namespace WaitForIt
{
    public static class BoatRace
    {
        public static int GetTraveled_mm(int race_ms, int charge_ms) {
            if (race_ms <= 0 || charge_ms <= 0 || charge_ms >= race_ms) {
                return 0;
            }
            const int acceleration_mmPer_ms2 = 1;
            var speed_mmPer_ms = charge_ms * acceleration_mmPer_ms2;
            var travel_ms = race_ms - charge_ms;
            return speed_mmPer_ms * travel_ms;
        }

        public static ImmutableHashSet<int> GetWinningCharge_ms(this BoatRaceState @this)
            => Enumerable.Range(1, @this.Race_ms - 1)
                         .Select(charge_ms => {
                              var traveled_mm = GetTraveled_mm(@this.Race_ms, charge_ms);
                              return (traveled_mm, charge_ms); })
                         .Where(tuple => tuple.traveled_mm > @this.Record_mm)
                         .Select(tuple => tuple.charge_ms)
                         .ToImmutableHashSet();
    }

    public record struct BoatRaceState(int Race_ms, int Record_mm);
}
