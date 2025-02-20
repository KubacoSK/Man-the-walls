using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class EnemyAiMove : MonoBehaviour
{

    public static EnemyAiMove Instance { get; private set; }
    private Zone previousZone = null;
    private Zone previousTargetZone = null;
    private Unit unitToWatch;

    [SerializeField] private Zone CenterZone;
    Vector2 destination;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one EnemyAi! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        unitToWatch = null;
    }
    private void Update()
    {
        if (!TurnSystem.Instance.IsPlayerTurn() && unitToWatch != null)
        {
            Vector3 targetPosition = unitToWatch.transform.position + new Vector3(0, 0, -10);
            Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetPosition, Time.deltaTime * 100f);
        }
    }
    public void MakeDecisionForUnit(Unit enemyUnit)
    { 
        unitToWatch = enemyUnit;
        if(enemyUnit.ReturnCurrentStandingZone() != null){
        if (enemyUnit.ReturnCurrentStandingZone().ReturnAllyUnitsInZone().Count > 0) return;
        }
        // Get valid zones for the current enemy unit
        List<Zone> validZones = enemyUnit.GetMoveAction().GetValidZonesListForEnemy();
        //we check if there are any further zones away with units in them
        List<Zone> ZonesToCheck = enemyUnit.GetMoveAction().CheckForAlliesToAttack();
        // Check if there are valid zones to move to
        if (validZones.Count > 0)
        {
            enemyUnit.SetEnemyPastZoneBack();
            Zone TargetZone = null;
            Zone destinationZone = null;
            bool StayStill = false;
            enemyUnit.SetTurnMiddlePoints(-1);
            destinationZone = validZones[UnityEngine.Random.Range(0, validZones.Count)];
            switch (UnityEngine.Random.Range(0, 2)) // we getting 50/50 chance that unit tries to go to the nierby allied unit or it goes to conquer allied zones
            {
                case 0:
                    foreach (Zone zone in ZonesToCheck)
                    {
                        if (zone.ReturnEnemyUnitsInZone().Count > 0)
                        {
                            // we check if the the unit we want to move towards is closer to middle than current
                            Vector2 VectorToMiddle1 = new Vector2(
                            Mathf.Abs(enemyUnit.GetCurrentZone().transform.position.x - CenterZone.transform.position.x),
                            Mathf.Abs(enemyUnit.GetCurrentZone().transform.position.y - CenterZone.transform.position.y));
                            float totaldiff1 = VectorToMiddle1.x + VectorToMiddle1.y;
                            Vector2 VectorToMiddle2 = new Vector2(
                            Mathf.Abs(zone.transform.position.x - CenterZone.transform.position.x),
                            Mathf.Abs(zone.transform.position.y - CenterZone.transform.position.y));
                            float totaldiff2 = VectorToMiddle2.x + VectorToMiddle2.y;
                            // if we detect zone with allied unit inside it we will get its name
                            if (totaldiff1 > totaldiff2)
                                TargetZone = zone;
                            Debug.Log("going to the allied unit");
                        }
                    }
                    break;
                case 1:
                    foreach (Zone zone in validZones)
                    {
                        if (zone.WhoIsUnderControl() == Zone.ControlType.allied)
                        {
                            destinationZone = zone;
                            Debug.Log("Going to conquer zone");
                        }
                    }
                    break;
            }
            if (enemyUnit.GetTurnMiddlePoints() <= 0)
            {
                TargetZone = CenterZone;
                enemyUnit.SetTurnMiddlePoints(3);
                Debug.Log("Going to the middle");
            }
            // Randomly choose a destination zone if neither zone with enemy nor ally is within distance, if there is, this gets rewritten
            
            
            
            if(TargetZone != null) 
            {
                // we get distance to our target position
                Vector2 VectorToDestination = new Vector2(
                    Mathf.Abs(enemyUnit.GetCurrentZone().transform.position.x - TargetZone.transform.position.x),
                    Mathf.Abs(enemyUnit.GetCurrentZone().transform.position.y - TargetZone.transform.position.y));
                //we get difference in x and y axis
                float xdiff = VectorToDestination.x;
                float ydiff = VectorToDestination.y;
                float totaldiff = xdiff + ydiff;
                
                foreach (Zone zone in validZones)
                {
                    if (zone == TargetZone)
                    {
                        // if our target zone is right beside our current zone we move there and skip our loop
                        destinationZone = zone;
                        TargetZone = null;
                        StayStill = true;
                        break;
                        
                    }
                    else
                    {
                        // here it compares positions of zones and if zone is closer to the target than current zone is, it moves to it
                        if (totaldiff >= ((Math.Abs(zone.transform.position.x - TargetZone.transform.position.x)) +
                                     (Math.Abs(zone.transform.position.y - TargetZone.transform.position.y))))
                        {
                            xdiff = Math.Abs(zone.transform.position.x - TargetZone.transform.position.x);
                            ydiff = Math.Abs(zone.transform.position.y - TargetZone.transform.position.y);
                            totaldiff = xdiff + ydiff;
                            destinationZone = zone;
                            previousTargetZone = TargetZone;
                        }
                    }
                }
            }
            foreach (Zone zone in validZones)
            {
                // if there is zone with enemy nierby it overrides destination zone to the one with enemy in it
                if (zone.ReturnAllyUnitsInZone().Count > 0)
                {
                    destinationZone = zone;
                    StayStill = true;
                    // however if there is zone with allyunit nierby it instead moves there, so it moves there
                }
            }
                                                                         // sets past zone position to false
            int index = 0;
            for (int i = 0; i < destinationZone.GetEnemyMoveLocationStatuses().Length; i++)
            {
                if (destinationZone.GetEnemyMoveLocationStatuses()[i] == false)
                {
                    destination = destinationZone.GetEnemyMoveLocations()[i];                             // gets Vector2 location of the zone
                    destinationZone.SetEnemyPositionStatus(i, true);                                     // Sets that zone has unit on the index
                    index = i;
                    break;

                }

            } 
            previousZone = destinationZone;
            Debug.Log(previousZone + "this is prevoius zone");
            enemyUnit.SetStandingZone(destinationZone, index);
            // Move the unit towards the chosen zone
            enemyUnit.SetRunningAnimation(true);
            enemyUnit.GetMoveAction().Move(destination);
            if (destination.x > enemyUnit.transform.position.x) enemyUnit.FlipUnit();
            // moves to zone
            enemyUnit.DoAction(destinationZone);
           
            if (destinationZone.ReturnAllyUnitsInZone().Count > 0)
            {
                destinationZone.ChangeControlToNeutral();
            }
            else
            {
                
                destinationZone.ChangeControlToEnemy();
            }
            if (!StayStill) StartCoroutine(DelayedSecondMove(enemyUnit));
            else { enemyUnit.DoAction(destinationZone);  }


        }
        
    }
    private IEnumerator DelayedSecondMove(Unit enemyUnit)
    {
        // Wait for 2 seconds before the second move
        yield return new WaitForSeconds(1.5f);

        // Check if the unit is still valid and has not already made two moves
        if (enemyUnit != null && enemyUnit.GetActionPoints() > 0)
        {
            // Randomly choose another destination zone for the second move
            List<Zone> validZones2 = enemyUnit.GetMoveAction().GetValidZonesListForEnemy();
            validZones2.RemoveAll(zone => zone.IsWallCheck());
            if (validZones2.Count > 0)
            {
                foreach (Zone zone in validZones2)
                {
                    Debug.Log(zone);
                }
                validZones2.Remove(previousZone);
                Zone seconddestinationZone = null;
                enemyUnit.SetEnemyPastZoneBack();    

                // Randomly choose a destination zone
                seconddestinationZone = validZones2[UnityEngine.Random.Range(0, validZones2.Count)];
                foreach (Zone zone in validZones2)
                {
                    if (zone.WhoIsUnderControl() == Zone.ControlType.allied)
                    {
                        seconddestinationZone = zone;
                    }
                }

                if (previousTargetZone != null)
                {
                    Debug.Log("Moving to target zone" + previousTargetZone.name);
                    // everything is explained before
                    Vector2 VectorToDestination = new Vector2(
                    Mathf.Abs(enemyUnit.GetCurrentZone().transform.position.x) - Mathf.Abs(previousTargetZone.transform.position.x),
                    Mathf.Abs(enemyUnit.GetCurrentZone().transform.position.y) - Mathf.Abs(previousTargetZone.transform.position.y));
                    float xdiff = Math.Abs(VectorToDestination.x);
                    float ydiff = Math.Abs(VectorToDestination.y);
                    foreach (Zone zone in validZones2)
                    {
                        if (zone == previousTargetZone)
                        {
                            seconddestinationZone = zone;
                            break;
                        }
                        else
                        {
                            if (xdiff >= (Math.Abs(Mathf.Abs(zone.transform.position.x) - Mathf.Abs(previousTargetZone.transform.position.x))) &&
                                ydiff >= (Math.Abs(Mathf.Abs(zone.transform.position.y) - Mathf.Abs(previousTargetZone.transform.position.y))))
                            {
                                seconddestinationZone = zone; // we calculate the difference of position x+y and choose closest zone
                            }
                        }
                    }
                }
                
                foreach (Zone zone in validZones2)
                {
                    if (zone.ReturnAllyUnitsInZone().Count > 0)
                    {
                        seconddestinationZone = zone;
                    }
                }
                                                                            // sets past zone position to false
                int index = 0;
                for (int i = 0; i < seconddestinationZone.GetEnemyMoveLocationStatuses().Length; i++)
                {
                    if (seconddestinationZone.GetEnemyMoveLocationStatuses()[i] == false)
                    {
                        destination = seconddestinationZone.GetEnemyMoveLocations()[i];                             // gets Vector2 location of the zone
                        seconddestinationZone.SetEnemyPositionStatus(i, true);                                     // Sets that zone has unit on the index
                        index = i;
                        break;

                    }

                }
                enemyUnit.SetStandingZone(seconddestinationZone, index);
                // Move the unit towards the chosen zone
                enemyUnit.SetRunningAnimation(true);
                enemyUnit.GetMoveAction().Move(destination);
                if (destination.x > enemyUnit.transform.position.x) enemyUnit.FlipUnit();


                if (seconddestinationZone.ReturnAllyUnitsInZone().Count > 0)
                {
                    seconddestinationZone.ChangeControlToNeutral();
                }
                else
                {
                    seconddestinationZone.ChangeControlToEnemy();
                }
                enemyUnit.DoAction(seconddestinationZone);
                
            }
        }
    }
}
