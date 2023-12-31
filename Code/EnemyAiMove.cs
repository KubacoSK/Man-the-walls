using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAiMove : MonoBehaviour
{

    public static EnemyAiMove Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one EnemyAi! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void MakeDecisionForUnit(Unit enemyUnit)
    {
        // Get valid zones for the current enemy unit
        List<Zone> validZones = enemyUnit.GetMoveAction().GetValidZonesList();
        List<Zone> ZonesToCheck = enemyUnit.GetMoveAction().CheckForAlliesToAttack();
        // Check if there are valid zones to move to
        if (validZones.Count > 0)
        {
            Vector2 PositionTowhereMove = new Vector2();
            Zone TargetZone = null;
            Zone destinationZone = null;
            bool StayStill = false;
            
            foreach (Zone zone in ZonesToCheck)
            {
                if (zone.ReturnEnemyUnitsInZone().Count > 0)
                {
                    zone.transform.position = PositionTowhereMove;
                    TargetZone = zone;
                }
            }


            // Randomly choose a destination zone
            if(TargetZone == null) destinationZone = validZones[UnityEngine.Random.Range(0, validZones.Count)];
            
            else
            {
                Vector2 VectorToDestination = enemyUnit.GetCurrentZone().transform.position - TargetZone.transform.position;
                foreach(Zone zone in validZones)
                {
                    if (Vector2.Equals(zone.transform.position, PositionTowhereMove)) 
                    {
                        TargetZone = destinationZone;
                    }
                }
            }
            foreach (Zone zone in validZones)
            {
                if (zone.ReturnAllyUnitsInZone().Count > 0)
                {
                    destinationZone = zone;
                    StayStill = true;
                    // however if there is zone with allyunit nierby it instead moves there, so it moves there
                }
            }
            Vector2 destinationposition = destinationZone.transform.position;
            List<Unit> UnitsInZone = destinationZone.GetUnitsInZone();
            float x = 0;
            float y = 0;
            foreach (Unit unitinzone in UnitsInZone)
            {
                x += 0.4f;
            }
            if (destinationZone.GetZoneSizeModifier().x == 1) y -= 0.4f;

            destinationposition.x += x;
            destinationposition.y += y;
            // Move the unit towards the chosen zone
            enemyUnit.GetMoveAction().Move(destinationposition);
            // moves to zone
            enemyUnit.DoAction(destinationZone);
            if (!StayStill) StartCoroutine(DelayedSecondMove(enemyUnit));
            else { enemyUnit.DoAction(); }


        }
        Camera.main.transform.position = enemyUnit.transform.position + new Vector3(0, 0, -10);
    }

    private IEnumerator DelayedSecondMove(Unit enemyUnit)
    {
        // Wait for 2 seconds before the second move
        yield return new WaitForSeconds(1.5f);

        // Check if the unit is still valid and has not already made two moves
        if (enemyUnit != null && enemyUnit.GetActionPoints() < 2)
        {
            // Randomly choose another destination zone for the second move
            List<Zone> validZones2 = enemyUnit.GetMoveAction().GetValidZonesList();
            if (validZones2.Count > 0)
            {
                Zone seconddestinationZone = validZones2[UnityEngine.Random.Range(0, validZones2.Count)];
                foreach (Zone zone in validZones2)
                {
                    if (zone.ReturnAllyUnitsInZone().Count > 0)
                    {
                        seconddestinationZone = zone;
                    }
                }
                Vector2 destinationposition = seconddestinationZone.transform.position;
                List<Unit> UnitsInZone2 = seconddestinationZone.GetUnitsInZone();
                float sx = 0;
                foreach (Unit unitinzone in UnitsInZone2)
                {
                    sx += 0.4f;
                }
                destinationposition.x += sx;
                // Move the unit towards the chosen zone
                enemyUnit.GetMoveAction().Move(destinationposition);
                enemyUnit.DoAction(seconddestinationZone);
            }
        }
    }
}
