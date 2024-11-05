using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    public static TeamManager Instance;
    public GameObject heroPrefab;
    public List<GameObject> heroes = new List<GameObject>();
    public int maxHeroes = 9;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        // Instantiate the initial hero
        AddHeroes(1);
    }

    public void AddHeroes(int amount)
    {
        int availableSlots = maxHeroes - heroes.Count;
        int heroesToAdd = Mathf.Min(amount, availableSlots);

        for (int i = 0; i < heroesToAdd; i++)
        {
            Vector3 spawnPosition = CalculateNewHeroPosition();
            GameObject newHero = Instantiate(heroPrefab, spawnPosition, Quaternion.identity);
            heroes.Add(newHero);
        }

        UpdateHeroScales();

        // Only update positions if necessary
        if (heroesToAdd > 0)
        {
            UpdateHeroPositions();
            }
    }


    public void RemoveHero(GameObject hero)
    {
        heroes.Remove(hero);
        if (heroes.Count == 0)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            UpdateHeroScales();
            UpdateHeroPositions();
        }
    }

    public void RemoveHeroes(int amount)
    {
        int heroesToRemove = Mathf.Min(amount, heroes.Count);
        for (int i = 0; i < heroesToRemove; i++)
        {
            GameObject heroToRemove = heroes[heroes.Count - 1];
            heroes.Remove(heroToRemove);
            Destroy(heroToRemove);
        }

        if (heroes.Count == 0)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            UpdateHeroScales();
            UpdateHeroPositions();
        }
    }

    public void UpgradeWeapons()
    {
        foreach (GameObject hero in heroes)
        {
            hero.GetComponent<HeroController>().UpgradeWeapon();
        }
    }

    void UpdateHeroPositions()
    {
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(heroes.Count));
        float spacing = 1.0f; // Adjust based on desired spacing

        int index = 0;
        Vector3 teamCenter = GetTeamCenterPosition();

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                if (index >= heroes.Count)
                    break;

                float xPos = (col - (gridSize - 1) / 2.0f) * spacing;
                float yPos = (row - (gridSize - 1) / 2.0f) * spacing;

                Vector3 offset = new Vector3(xPos, yPos, 0);
                heroes[index].transform.position = teamCenter + offset;
                index++;
            }
        }
    }

    Vector3 GetTeamCenterPosition()
    {
        // Calculate the average position of all heroes
        Vector3 sumPosition = Vector3.zero;
        foreach (GameObject hero in heroes)
        {
            sumPosition += hero.transform.position;
        }
        return sumPosition / heroes.Count;
    }


    void UpdateHeroScales()
    {
        float baseScale = 0.4f;
        float scaleFactor = 1.0f;

        if (heroes.Count > 2)
        {
            scaleFactor = 0.5f; // Decrease size
        }
        if (heroes.Count > 4)
        {
            scaleFactor = 0.4f;
        }
        if (heroes.Count == maxHeroes)
        {
            scaleFactor = 0.3f; // Smallest size at max heroes
        }

        foreach (GameObject hero in heroes)
        {
            hero.transform.localScale = Vector3.one * baseScale * scaleFactor;
        }
    }

    Vector3 CalculateNewHeroPosition()
    {
        if (heroes.Count == 0)
        {
            return new Vector3(0, -4f, 0);
        }
        else
        {
            // Place new hero next to the last hero
            GameObject lastHero = heroes[heroes.Count - 1];
            Vector3 lastPosition = lastHero.transform.position;
            float spacing = 1.0f; // Adjust as needed
            Vector3 newPosition = lastPosition + new Vector3(spacing, 0, 0);
            return newPosition;
        }
    }
}
