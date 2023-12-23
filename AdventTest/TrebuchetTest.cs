using FluentAssertions;
using Trebuchet;

namespace AdventTest; 

public class TrebuchetTest {
    [Theory]
    // ReSharper disable StringLiteralTypo
    [InlineData("1abc2", 12)]
    [InlineData("pqr3stu8vwx", 38)]
    [InlineData("a1b2c3d4e5f", 15)]
    [InlineData("treb7uchet", 77)]
    // ReSharper restore StringLiteralTypo
    public static void ToCalibration_GivenNoDigitsAsStrings_ShouldReturnCorrectCalibrationValue(
        string input, int expectedCalibrationValue) {
        var calibrationString = new CalibrationString(input);
        var expectedCalibration = new Calibration(expectedCalibrationValue);
        calibrationString.ToCalibration().Should().Be(expectedCalibration);
    }

    [Fact]
    public static void SumTrebuchetCalibrationFile() {
        var path = Path.GetFullPath(@"D:\Projects\Code\AdventOfCode2023\Trebuchet\TrebuchetCalibration.txt");
        // ReSharper disable once UnusedVariable
        var sumCalibrations = File.ReadLines(path)
                                  .Select(s => new CalibrationString(s))
                                  .Select(c => c.ToCalibration())
                                  .Sum(c => c);
        Assert.True(true);
    }

    [Theory]
    // ReSharper disable StringLiteralTypo
    [InlineData("two1nine", 29)]
    [InlineData("eightwothree", 83)]
    [InlineData("abcone2threexyz", 13)]
    [InlineData("xtwone3four", 24)]
    [InlineData("4nineeightseven2", 42)]
    [InlineData("zoneight234", 14)]
    [InlineData("7pqrstsixteen", 76)]
    // ReSharper restore StringLiteralTypo
    public static void ToCalibration_GivenDigitsAsStringsAndNumbers_ShouldReturnCorrectCalibrationValue(
        string input, int expectedCalibrationValue) {
        var calibrationString = new CalibrationString(input);
        var expectedCalibration = new Calibration(expectedCalibrationValue);
        calibrationString.ToCalibration().Should().Be(expectedCalibration);
    }
}