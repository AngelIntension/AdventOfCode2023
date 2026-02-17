# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

C# (.NET 8.0) solutions for Advent of Code 2023. Each puzzle day has its own class library project; a single xUnit test project covers all days.

## Build & Test Commands

```bash
# Build the entire solution
dotnet build AdventOfCode2023.sln

# Run all tests
dotnet test AdventTest/AdventTest.csproj

# Run a single test by fully-qualified name
dotnet test AdventTest/AdventTest.csproj --filter "FullyQualifiedName~AdventTest.TrebuchetTest.ToCalibration_GivenNoDigitsAsStrings_ShouldReturnCorrectCalibrationValue"

# Run all tests in a single test class
dotnet test AdventTest/AdventTest.csproj --filter "FullyQualifiedName~AdventTest.BoatRaceTest"
```

## Architecture & Conventions

**One project per puzzle day:** Each day is a standalone class library (`Trebuchet/`, `BagOfCubesGame/`, `GearRatio/`, `ScratchCards/`, `IfYouGiveASeedAFertilizer/`, `WaitForIt/`). The project name matches the puzzle's theme, not "Day1/Day2".

**Functional style with static extension methods:** Puzzle logic lives in static classes with extension methods on domain types. No service classes or dependency injection.

**Record structs with implicit operators:** Domain types are `record struct` wrappers (e.g., `Calibration`, `CalibrationString`) with implicit conversion operators to/from primitives. This provides type safety without allocation overhead.

**Immutable collections:** Uses `System.Collections.Immutable` throughout (`ImmutableArray<T>`, `ImmutableHashSet<T>`, `ImmutableSortedDictionary<K,V>`).

**Test style:** xUnit with FluentAssertions (`.Should().Be(...)`) and `[Theory]`/`[InlineData]` for parameterized tests. Some tests read puzzle input from text files via hardcoded paths. Test data files live in `AdventTest/`.

**Nullable reference types** and **implicit usings** are enabled across all projects.
