using Godot;
using System;
using System.Collections.Generic;

public partial class EnemySpawner : Node
{
    class EnemyData {
        public static RandomNumberGenerator randomGenerator;
        private static List<EnemyData> list = new List<EnemyData>();
        private static float totalOdds = 0;

        private float spawnOdds;
        private PackedScene resource;

        public static void addToList(float spawnOdds, String resourcePath) {
            EnemyData newData = new EnemyData(spawnOdds, resourcePath);
            list.Add(newData);
            totalOdds += spawnOdds;
        }

        public static void SpawnRandom(Node parent, Vector2 position, CharacterBody2D player) {
            float randomOdds = randomGenerator.RandfRange(0, totalOdds);
            PackedScene randomResource = findMatchingResource(randomOdds);
            Node2D newSpawn = randomResource.Instantiate<Node2D>();
            newSpawn.Set("PlayerBody", player);
            newSpawn.Position = position;
            parent.AddChild(newSpawn);
        }

        private static PackedScene findMatchingResource(float odds) {
            int listSize = list.Count;
            float oddsCount = 0;
            for (int i = 0; i < listSize; i++) {
                EnemyData data = list[i];
                oddsCount += data.spawnOdds;
                if (oddsCount >= odds) {
                    return data.resource;
                } 
            }
            return null;
        }

        private EnemyData(float spawnOdds, String resourcePath) {
            this.spawnOdds = spawnOdds;
            this.resource = ResourceLoader.Load<PackedScene>(resourcePath);
        }


    }

    // CONSTANT
    [Export] CharacterBody2D player;
    [Export] Label waveLabel;
    [Export] PackedScene goldResource;
    float goldSpawnDuration = 1f;
    float minDistanceFromPlayer = 2000;
    RandomNumberGenerator randomGenerator = new RandomNumberGenerator();
    float spawnDuration = 0.2f;
    float waveCountMultiplier = 1.2f; // multiplier to increase the number of enemies by each wave
    float waveLabelFadeDuration = 4f;

    // Varying
    int maxWaveEnemyCount = 10;
    int waveEnemyCount;
    int waveCount = 0;
    float spawnTimer;
    float waveLabelFadeTimer;
    float goldSpawnTimer = 0;


    // Called when the node enters the scene tree for the first time.
	public override void _Ready() {
        goldResource = ResourceLoader.Load<PackedScene>("res://Scenes/gold.tscn");

        EnemyData.randomGenerator = randomGenerator;
        randomGenerator.Randomize(); // initialize seed
        spawnTimer = spawnDuration;
        waveEnemyCount = maxWaveEnemyCount;

        EnemyData.addToList(2, "res://Scenes/enemy.tscn");

        startNextWave();
	}

    // returns a random spawn position on the circumference of a circle centered at the player with radius of minDistanceFromPlayer;
    Vector2 getRandomSpawnPosition() {
        float randomAngle = randomGenerator.RandfRange(0,2*Mathf.Pi);
        Vector2 randomPosition = new Vector2(0, minDistanceFromPlayer).Rotated(randomAngle);
        Vector2 randomGlobalPosition = player.ToGlobal(randomPosition);

        return randomGlobalPosition;
    }

    Vector2 getRandomGoldPosition() {
        float randomY = randomGenerator.RandfRange(-minDistanceFromPlayer/2, minDistanceFromPlayer/2);
        Vector2 randomPosition = new Vector2(-minDistanceFromPlayer, randomY);
        Vector2 randomGlobalPosition = player.ToGlobal(randomPosition);
        return randomGlobalPosition;
    }

    void spawnEnemy() {
        Vector2 enemyPosition = getRandomSpawnPosition();
        EnemyData.SpawnRandom(Owner, enemyPosition, player);
        waveEnemyCount--;
    }

    void spawnGold() {
        Vector2 goldPosition = getRandomGoldPosition();
        Node2D newGold = goldResource.Instantiate<Node2D>();
        newGold.Position = goldPosition;
        AddChild(newGold);
    }

    void startNextWave() {
        waveCount++;
        waveLabel.Text = "Wave " + waveCount;
        waveLabel.Modulate = new Color(waveLabel.Modulate, 1);
        waveLabelFadeTimer = waveLabelFadeDuration;

        if (waveCount != 1) {
            maxWaveEnemyCount = (int) Mathf.Round(maxWaveEnemyCount*waveCountMultiplier);
            waveEnemyCount = maxWaveEnemyCount;
        }
    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
        float dt = (float) delta;

        goldSpawnTimer -= dt;
        if (goldSpawnTimer <= 0) {
            spawnGold();
            goldSpawnTimer = goldSpawnDuration;
        }

        if (waveLabelFadeTimer > 0) {
            waveLabel.Modulate = new Color(waveLabel.Modulate, Mathf.Lerp(0, 1, waveLabelFadeTimer*2/waveLabelFadeDuration));
            waveLabelFadeTimer -= dt;
            if (waveLabelFadeTimer <= 0) {
                waveLabel.Modulate = new Color(waveLabel.Modulate, 0);
            }
        }

        if (waveEnemyCount <= 0) {
            if (GetTree().GetNodesInGroup("Enemy").Count == 0) {
                startNextWave();
            }
            else {return;}
        }


        spawnTimer -= dt;
        if (spawnTimer <= 0) {
            spawnEnemy();
            spawnTimer = spawnDuration;
        }

	}
}
