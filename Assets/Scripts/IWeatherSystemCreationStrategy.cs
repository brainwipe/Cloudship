using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal interface IWeatherSystemCreationStrategy
{
    float Radius { get; }
	float Diameter { get; }
	float Speed { get; }
    float OverlapAmount { get; }
    bool IsClockwise { get; }

    bool Accepts(int perlinNoiseValue);
}

internal abstract class WeatherSystemCreationStrategyBase
{
	public abstract float Radius { get; }
	public float Diameter { get { return Radius * 2; } }
	public virtual float OverlapAmount { get { return 0.1f; } }
	public virtual bool IsClockwise { get { return Random.Range(0,2) > 0; } }
}

internal class Small : WeatherSystemCreationStrategyBase, IWeatherSystemCreationStrategy
{
	public bool Accepts(int perlinNoiseValue) { return perlinNoiseValue > 7; }
	public override float Radius { get { return 5; } }
	public float Speed { get { return 60; } }
}

internal class Medium : WeatherSystemCreationStrategyBase, IWeatherSystemCreationStrategy
{
	public bool Accepts(int perlinNoiseValue) { return perlinNoiseValue < 8 && perlinNoiseValue > 5; }
	public override float Radius { get { return 10; } }
	public float Speed { get { return 30; } }
}

internal class Large : WeatherSystemCreationStrategyBase, IWeatherSystemCreationStrategy
{
	public bool Accepts(int perlinNoiseValue) { return perlinNoiseValue < 6; }
	public override float Radius { get { return 20; } }
	public float Speed { get { return 10; } }
}