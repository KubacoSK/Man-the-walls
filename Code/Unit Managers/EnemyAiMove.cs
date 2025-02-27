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

            // nedovoli pohnut sa kamere cez hranice
            targetPosition.x = Mathf.Clamp(targetPosition.x, CameraController.Instance.panMinimum.x + Camera.main.orthographicSize, CameraController.Instance.panLimit.x - Camera.main.orthographicSize);
            targetPosition.y = Mathf.Clamp(targetPosition.y, CameraController.Instance.panMinimum.y + Camera.main.orthographicSize / 2, CameraController.Instance.panLimit.y - Camera.main.orthographicSize / 2);

            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, Time.deltaTime * 5f);
        }
    }
    public void MakeDecisionForUnit(Unit enemyUnit)
    { 
        unitToWatch = enemyUnit;
        if(enemyUnit.ReturnCurrentStandingZone() != null){
        if (enemyUnit.ReturnCurrentStandingZone().ReturnAllyUnitsInZone().Count > 0) return;
        }
        previousZone = enemyUnit.ReturnCurrentStandingZone();
        List<Zone> validZones = enemyUnit.GetMoveAction().GetValidZonesListForEnemy();
        List<Zone> ZonesToCheck = enemyUnit.GetMoveAction().CheckForAlliesToAttack();

        if (validZones.Count > 0)
        {
            enemyUnit.SetEnemyPastZoneBack();
            Zone TargetZone = null;
            Zone destinationZone = null;
            bool StayStill = false;
            enemyUnit.SetTurnMiddlePoints(-1);
            destinationZone = validZones[UnityEngine.Random.Range(0, validZones.Count)];
            switch (UnityEngine.Random.Range(0, 2)) // bud sa pokusi dobit zonu alebo sa zoskupit
            {
                case 0:
                    foreach (Zone zone in ZonesToCheck)
                    {
                        if (zone.ReturnEnemyUnitsInZone().Count > 0)
                        {
                            Vector2 VectorToMiddle1 = new Vector2(
                            Mathf.Abs(enemyUnit.ReturnCurrentStandingZone().transform.position.x - CenterZone.transform.position.x),
                            Mathf.Abs(enemyUnit.ReturnCurrentStandingZone().transform.position.y - CenterZone.transform.position.y));
                            float totaldiff1 = VectorToMiddle1.x + VectorToMiddle1.y;
                            Vector2 VectorToMiddle2 = new Vector2(
                            Mathf.Abs(zone.transform.position.x - CenterZone.transform.position.x),
                            Mathf.Abs(zone.transform.position.y - CenterZone.transform.position.y));
                            float totaldiff2 = VectorToMiddle2.x + VectorToMiddle2.y;
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
            // vyberieme nahodnu zonu, ale potom ju prepise ak
            
            
            
            if(TargetZone != null) 
            {
                Vector2 VectorToDestination = new Vector2(
                    Mathf.Abs(enemyUnit.ReturnCurrentStandingZone().transform.position.x - TargetZone.transform.position.x),
                    Mathf.Abs(enemyUnit.ReturnCurrentStandingZone().transform.position.y - TargetZone.transform.position.y));
                float xdiff = VectorToDestination.x;
                float ydiff = VectorToDestination.y;
                float totaldiff = xdiff + ydiff;
                
                foreach (Zone zone in validZones)
                {
                    if (zone == TargetZone)
                    {
                        // ak je nas ciel hned vedla, pohne jednotku tam
                        destinationZone = zone;
                        TargetZone = null;
                        StayStill = true;
                        break;
                        
                    }
                    else
                    {
                        //porovnava zonu cez ktoru sa dostane ku destinacii
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
                if (zone.ReturnAllyUnitsInZone().Count > 0)
                {
                    destinationZone = zone;
                    StayStill = true;
                    // Ak susedi so spojeneckou jednotkou, zautoci na nu
                }
            }
                                                                         
            int index = 0;
            for (int i = 0; i < destinationZone.GetEnemyMoveLocationStatuses().Length; i++)
            {
                if (destinationZone.GetEnemyMoveLocationStatuses()[i] == false)
                {
                    destination = destinationZone.GetEnemyMoveLocations()[i];                            
                    destinationZone.SetEnemyPositionStatus(i, true);                                     
                    index = i;
                    break;

                }

            } 
            
            enemyUnit.SetStandingZone(destinationZone, index);
            enemyUnit.SetRunningAnimation(true);
            enemyUnit.GetMoveAction().Move(destination);
            if (destination.x > enemyUnit.transform.position.x) enemyUnit.FlipUnit();
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
        // pockame 2 sekundy na pohyb
        yield return new WaitForSeconds(1.5f);

        // skontrolujeme ze ci sa nasa jednotka moze hybat
        if (enemyUnit != null && enemyUnit.GetActionPoints() > 0)
        {
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

                // nahodne vyberieme zonu na pohyb
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
                    // vsetko podobne ako predtym
                    Vector2 VectorToDestination = new Vector2(
                    Mathf.Abs(enemyUnit.ReturnCurrentStandingZone().transform.position.x) - Mathf.Abs(previousTargetZone.transform.position.x),
                    Mathf.Abs(enemyUnit.ReturnCurrentStandingZone().transform.position.y) - Mathf.Abs(previousTargetZone.transform.position.y));
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
                                seconddestinationZone = zone; // vypocitame x a y tak aby sme dostali najblizsiu zonu k destinacii
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
                                                                            // nastavi minulu poziciu na false
                int index = 0;
                for (int i = 0; i < seconddestinationZone.GetEnemyMoveLocationStatuses().Length; i++)
                {
                    if (seconddestinationZone.GetEnemyMoveLocationStatuses()[i] == false)
                    {
                        destination = seconddestinationZone.GetEnemyMoveLocations()[i];                             
                        seconddestinationZone.SetEnemyPositionStatus(i, true);                                     
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
