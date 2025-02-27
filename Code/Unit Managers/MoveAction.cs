using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private Vector2 targetPosition;
    private Unit selectedUnit;
    private List<Zone> validMoveZones;
    private float moveSpeed = 3f;
    private float highlightInterval = 0.1f; // zvyraznenie kazde 0.1s
    private float highlightTimer = 0.0f;
    private Unit thisUnit;

    private void Awake()
    {
        selectedUnit = null;
        targetPosition = transform.position;
        if (ZoneManager.Instance == null)
        {
            Debug.LogError("ZoneManager is not initialized!");
            return;
        }
      
        UnitActionsSystem.Instance.OnSelectedUnitChanged += OnSelectedUnitChanged;
    }

    private void Start()
    {
        thisUnit = GetComponent<Unit>();
        validMoveZones = new List<Zone>();
    }
    private void Update()
    {


        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
        if(transform.position.x == targetPosition.x && transform.position.y == targetPosition.y)
        {
            thisUnit.FlipUnitBack();
            thisUnit.SetRunningAnimation(false);
        }
        highlightTimer += Time.deltaTime;

        if (highlightTimer >= highlightInterval)
        {
            // Reset the timer
            highlightTimer = 0.0f;

            if (selectedUnit != null && selectedUnit.GetMoveAction() != null && selectedUnit.GetActionPoints() > 0)
            {
                validMoveZones = selectedUnit.GetMoveAction().GetValidZonesList();
                HighlightValidMoveZones();
            }
            else
            {
                validMoveZones.Clear();
                HighlightValidMoveZones();
            }
        }
    }

    public void Move(Vector2 targetPosition)
    {
        this.targetPosition = targetPosition;

    }

    public List<Zone> GetValidZonesListForEnemy()
    {

        List<Zone> validZoneList = new List<Zone>();
        // spravi novy list pre vhodne zony
        Collider2D[] StandingZoneCollider = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.1f, 0.1f), 0);
        // zisti ze na akej zone sa jednota nachadza 

        foreach (var collider in StandingZoneCollider)
        {
           
            Zone zone = collider.GetComponent<Zone>();
            if (zone != null && !validZoneList.Contains(zone))
            {

                // zisti poziciu a velkost zony
                Vector2 zonePosition = zone.transform.position;
                Vector2 zoneSize = zone.GetZoneSizeModifier();

                // zvecsi rozsah tak aby sme vedeli zistit najblizsie zony
                float enlargedWidth = zoneSize.x;
                float enlargedHeight = zoneSize.y;


                Collider2D[] adjustedColliders = Physics2D.OverlapBoxAll(zonePosition, new Vector2(enlargedWidth, enlargedHeight), 0);

                foreach (var adjustedCollider in adjustedColliders)
                {
                    // pridame najdene zony do listu
                    Zone adjustedZone = adjustedCollider.GetComponent<Zone>();
                    if (adjustedZone != null && !validZoneList.Contains(adjustedZone) && adjustedZone != zone && Array.Exists(adjustedZone.GetEnemyMoveLocationStatuses(), value => value == false))
                    {
                        validZoneList.Add(adjustedZone);

                    }
                }

            }
        }

        return validZoneList;
    }

    public List<Zone> GetValidZonesList()
    {

        List<Zone> validZoneList = new List<Zone>();
        Collider2D[] StandingZoneCollider = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.1f, 0.1f), 0);
        // skoro to iste len pre spojenecke jednotky

        foreach (var collider in StandingZoneCollider)
        {
            Zone zone = collider.GetComponent<Zone>();
            if (zone != null && !validZoneList.Contains(zone))
            {

                Vector2 zonePosition = zone.transform.position;
                Vector2 zoneSize = zone.GetZoneSizeModifier();
                float enlargedWidth = zoneSize.x;
                float enlargedHeight = zoneSize.y;

                Collider2D[] adjustedColliders = Physics2D.OverlapBoxAll(zonePosition, new Vector2(enlargedWidth, enlargedHeight), 0);

                foreach (var adjustedCollider in adjustedColliders)
                {

                    Zone adjustedZone = adjustedCollider.GetComponent<Zone>();
                    if (adjustedZone != null && !validZoneList.Contains(adjustedZone) && adjustedZone != zone && Array.Exists(adjustedZone.GetAllyMoveLocationStatuses(), value => value == false)
                        && ((selectedUnit.CanComeToWalls() && adjustedZone.IsWallCheck()) || (selectedUnit.CanComeToWalls() && !adjustedZone.IsWallCheck()) || (!selectedUnit.CanComeToWalls() && !adjustedZone.IsWallCheck())) && !(zone.ReturnEnemyUnitsInZone().Count > 0))
                    {
                        validZoneList.Add(adjustedZone);

                    }
                }

            }
        }
        return validZoneList;
    }

    public List<Zone> CheckForAlliesToAttack()
    {

        List<Zone> Enlargedzones = new List<Zone>();
        // toto rovnakym sposobom zisti ci su nejaky spojenci v urcitej vzdialenosti na zautocenie
        Collider2D[] StandingZoneCollider = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.1f, 0.1f), 0);

        foreach (var collider in StandingZoneCollider)
        {
            Zone zone = collider.GetComponent<Zone>();
            if (zone != null && !Enlargedzones.Contains(zone))
            {

                Vector2 zonePosition = zone.transform.position;
                Vector2 zoneSize = zone.GetZoneSizeModifier();

                float enlargedWidth = (zoneSize.x / 2) + 10;
                float enlargedHeight = (zoneSize.y / 2) + 10;

                Collider2D[] adjustedColliders = Physics2D.OverlapBoxAll(zonePosition, new Vector2(enlargedWidth, enlargedHeight), 0);

                foreach (var adjustedCollider in adjustedColliders)
                {
                    Zone adjustedZone = adjustedCollider.GetComponent<Zone>();
                    if (adjustedZone != null && !Enlargedzones.Contains(adjustedZone) && adjustedZone != zone)
                    {
                        Enlargedzones.Add(adjustedZone);
                    }
                }

            }
        }

        return Enlargedzones;
    }

    private List<Zone> CheckForResetZones()
    {
        // toto sluzi na nastavenie vidietlnosti naspet
        List<Zone> Enlargedzones = new List<Zone>();

        Collider2D[] StandingZoneCollider = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.1f, 0.1f), 0);


        foreach (var collider in StandingZoneCollider)
        {

            Zone zone = collider.GetComponent<Zone>();
            if (zone != null && !Enlargedzones.Contains(zone))
            {

                Vector2 zonePosition = zone.transform.position;
                Vector2 zoneSize = zone.GetZoneSizeModifier();


                float enlargedWidth = (zoneSize.x / 2) + 10;
                float enlargedHeight = (zoneSize.y / 2) + 10;


                Collider2D[] adjustedColliders = Physics2D.OverlapBoxAll(zonePosition, new Vector2(enlargedWidth, enlargedHeight), 0);

                foreach (var adjustedCollider in adjustedColliders)
                {

                    Zone adjustedZone = adjustedCollider.GetComponent<Zone>();
                    if (adjustedZone != null && !Enlargedzones.Contains(adjustedZone))
                    {
                        Enlargedzones.Add(adjustedZone);
                    }
                }

            }
        }
        return Enlargedzones;
    }

    public bool IsValidActionWorldPosition(Vector2 worldPosition)
    {

        return true;
    }


    private void HighlightValidMoveZones()
    {

        foreach (var zone in CheckForResetZones())
        {
            zone.ResetHighlight();
        }

        foreach (var zone in validMoveZones)
        {
            zone.Highlight();
        }
    }
    private void OnSelectedUnitChanged(object sender, EventArgs e)
    {
        selectedUnit = UnitActionsSystem.Instance.GetSelectedUnit();
    }
}