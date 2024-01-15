using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    private Vector2 targetPosition;
    private Unit selectedUnit;
    static List<Zone> zones;
    private List<Zone> validMoveZones;
    private float moveSpeed = 5f;
    private float highlightInterval = 0.1f; // Highlight every 0.5 second
    private float highlightTimer = 0.0f;

    private void Awake()
    {
        selectedUnit = null;
        targetPosition = transform.position;
        if (ZoneManager.Instance == null)
        {
            Debug.LogError("ZoneManager is not initialized!");
            return;
        }

        // Initialize the static list in MoveAction
        MoveAction.zones = ZoneManager.GetAllZones();
        UnitActionsSystem.Instance.OnSelectedUnitChanged += OnSelectedUnitChanged;
    }

    private void Start()
    {
        validMoveZones = new List<Zone>();
    }
    private void Update()
    {


        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
        highlightTimer += Time.deltaTime;

        if (highlightTimer >= highlightInterval)
        {
            // Reset the timer
            highlightTimer = 0.0f;

            if (selectedUnit != null && selectedUnit.GetMoveAction() != null && selectedUnit.GetActionPoints() < 3 && selectedUnit.GetHorse())
            {
                validMoveZones = selectedUnit.GetMoveAction().GetValidZonesList();
                HighlightValidMoveZones();
            }
            else if (selectedUnit != null && selectedUnit.GetMoveAction() != null && selectedUnit.GetActionPoints() < 2 && !selectedUnit.GetHorse())
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

    public List<Zone> GetValidZonesList()
    {

        List<Zone> validZoneList = new List<Zone>();
        // makes new list of validzones
        Collider2D[] StandingZoneCollider = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.1f, 0.1f), 0);
        // creates a square based on size of a zone + 0.9xy

        foreach (var collider in StandingZoneCollider)
        {
            // checks if the square overlaps any type of zone
            Zone zone = collider.GetComponent<Zone>();
            if (zone != null && !validZoneList.Contains(zone))
            {

                // Get the position and size of the zone
                Vector2 zonePosition = zone.transform.position;
                Vector2 zoneSize = zone.GetZoneSizeModifier();

                // Calculate the enlarged box based on the zone's position and size
                float enlargedWidth = zoneSize.x;
                float enlargedHeight = zoneSize.y;

                // Perform overlap check with the enlarged box
                Collider2D[] adjustedColliders = Physics2D.OverlapBoxAll(zonePosition, new Vector2(enlargedWidth, enlargedHeight), 0);

                foreach (var adjustedCollider in adjustedColliders)
                {
                    // makes zone variable from collider and then puts the zone into list
                    Zone adjustedZone = adjustedCollider.GetComponent<Zone>();
                    if (adjustedZone != null && !validZoneList.Contains(adjustedZone) && adjustedZone != zone && !(adjustedZone.GetUnitsInZone().Count > 1 && (adjustedZone.GetZoneSizeModifier().x == 1f || adjustedZone.GetZoneSizeModifier().y == 1f)))
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
        // makes new list of validzones
        Collider2D[] StandingZoneCollider = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.1f, 0.1f), 0);
        // creates a square based on size of a zone + 0.9xy

        foreach (var collider in StandingZoneCollider)
        {
            // checks if the square overlaps any type of zone
            Zone zone = collider.GetComponent<Zone>();
            if (zone != null && !Enlargedzones.Contains(zone))
            {

                // Get the position and size of the zone
                Vector2 zonePosition = zone.transform.position;
                Vector2 zoneSize = zone.GetZoneSizeModifier();

                // Calculate the enlarged box based on the zone's position and size
                float enlargedWidth = (zoneSize.x / 2) + 10;
                float enlargedHeight = (zoneSize.y / 2) + 10;

                // Perform overlap check with the enlarged box
                Collider2D[] adjustedColliders = Physics2D.OverlapBoxAll(zonePosition, new Vector2(enlargedWidth, enlargedHeight), 0);

                foreach (var adjustedCollider in adjustedColliders)
                {
                    // makes zone variable from collider and then puts the zone into list
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

    public List<Zone> CheckForResetZones()
    {

        List<Zone> Enlargedzones = new List<Zone>();
        // makes new list of validzones
        Collider2D[] StandingZoneCollider = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.1f, 0.1f), 0);
        // creates a square based on size of a zone + 0.9xy

        foreach (var collider in StandingZoneCollider)
        {
            // checks if the square overlaps any type of zone
            Zone zone = collider.GetComponent<Zone>();
            if (zone != null && !Enlargedzones.Contains(zone))
            {

                // Get the position and size of the zone
                Vector2 zonePosition = zone.transform.position;
                Vector2 zoneSize = zone.GetZoneSizeModifier();

                // Calculate the enlarged box based on the zone's position and size
                float enlargedWidth = (zoneSize.x / 2) + 10;
                float enlargedHeight = (zoneSize.y / 2) + 10;

                // Perform overlap check with the enlarged box
                Collider2D[] adjustedColliders = Physics2D.OverlapBoxAll(zonePosition, new Vector2(enlargedWidth, enlargedHeight), 0);

                foreach (var adjustedCollider in adjustedColliders)
                {
                    // makes zone variable from collider and then puts the zone into list
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

    public bool IsValidActionWorldPosition(Vector2 worldPosition)
    {

        return true;
    }


    private void HighlightValidMoveZones()
    {
        // Remove highlight from all zones
        foreach (var zone in CheckForResetZones())
        {
            zone.ResetHighlight();
        }

        // Highlight the valid move zones
        foreach (var zone in validMoveZones)
        {
            zone.Highlight();
        }
    }
    private void OnSelectedUnitChanged(object sender, EventArgs e)
    {
        // Update the selected unit when the event is triggered
        selectedUnit = UnitActionsSystem.Instance.GetSelectedUnit();
    }
}