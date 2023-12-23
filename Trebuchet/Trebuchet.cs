using System.Text.RegularExpressions;

namespace Trebuchet; 

public static class Trebuchet {
    public static Calibration ToCalibration(this CalibrationString @this) {
        var calibrationNumbers = @this.GetCalibrationNumbers();
        if (calibrationNumbers.Count < 1) {
            throw new ArgumentException("Calibration string must contain at least one number.");
        }

        var digits = calibrationNumbers.Select(match => match.ParseDigit()).ToArray();
        return digits.First() * 10 + digits.Last();
    }

    private static MatchCollection GetCalibrationNumbers(this CalibrationString @this) {
        const string pattern = "[123456789]|(?:one|two|three|four|five|six|seven|eight|nine)";
        return new Regex(pattern).Matches(@this.Data.ToLower());
    }

    private static int ToDigit(this string @this) {
        return @this switch {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            _ => throw new ArgumentException("string must contain a valid number to parse")
        };
    }
    
    private static int ParseDigit(this Capture @this) => int.TryParse(@this.Value, out var digit)
        ? digit
        : @this.Value.ToDigit();
}

public record struct Calibration(int Value) {
    public int Value { get; init; } = Value;
    public static implicit operator int(Calibration value) => value.Value;
    public static implicit operator Calibration(int value) => new(value);
}

public record struct CalibrationString(string Data) {
    public string Data { get; init; } = Data;
    public static implicit operator string(CalibrationString calibrationString) => calibrationString.Data;
    public static implicit operator CalibrationString(string input) => new(input);
}