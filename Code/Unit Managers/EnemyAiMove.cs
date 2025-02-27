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
            Debug.LogError("Existuje viac než jeden EnemyAi! " + transform + " - " + Instance);
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

            // Orezanie cieľovej pozície v rámci limitov kamery
            targetPosition.x = Mathf.Clamp(targetPosition.x, CameraController.Instance.panMinimum.x + Camera.main.orthographicSize, CameraController.Instance.panLimit.x - Camera.main.orthographicSize);
            targetPosition.y = Mathf.Clamp(targetPosition.y, CameraController.Instance.panMinimum.y + Camera.main.orthographicSize / 2, CameraController.Instance.panLimit.y - Camera.main.orthographicSize / 2);

            // Plynulé pohybovanie kamery smerom k orezanej cieľovej pozícii
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPosition, Time.deltaTime * 5f);
        }
    }
    public void MakeDecisionForUnit(Unit enemyUnit)
    {
        unitToWatch = enemyUnit;
        if (enemyUnit.ReturnCurrentStandingZone() != null)
        {
            if (enemyUnit.ReturnCurrentStandingZone().ReturnAllyUnitsInZone().Count > 0) return;
        }
        previousZone = enemyUnit.ReturnCurrentStandingZone();
        // Získanie platných zón pre aktuálnu nepriateľskú jednotku
        List<Zone> validZones = enemyUnit.GetMoveAction().GetValidZonesListForEnemy();
        // Kontrola, či existujú ďalšie zóny s jednotkami na útok
        List<Zone> ZonesToCheck = enemyUnit.GetMoveAction().CheckForAlliesToAttack();
        // Kontrola, či existujú platné zóny na presun
        if (validZones.Count > 0)
        {
            enemyUnit.SetEnemyPastZoneBack();
            Zone TargetZone = null;
            Zone destinationZone = null;
            bool StayStill = false;
            enemyUnit.SetTurnMiddlePoints(-1);
            destinationZone = validZones[UnityEngine.Random.Range(0, validZones.Count)];
            switch (UnityEngine.Random.Range(0, 2)) // Máme 50/50 šancu, že jednotka sa pokúsi ísť k blízkej spojeneckej jednotke alebo dobije spojeneckú zónu
            {
                case 0:
                    foreach (Zone zone in ZonesToCheck)
                    {
                        if (zone.ReturnEnemyUnitsInZone().Count > 0)
                        {
                            // Kontrola, či je zóna, ktorú chceme navštíviť, bližšie k stredu ako aktuálna
                            Vector2 VectorToMiddle1 = new Vector2(
                            Mathf.Abs(enemyUnit.ReturnCurrentStandingZone().transform.position.x - CenterZone.transform.position.x),
                            Mathf.Abs(enemyUnit.ReturnCurrentStandingZone().transform.position.y - CenterZone.transform.position.y));
                            float totaldiff1 = VectorToMiddle1.x + VectorToMiddle1.y;
                            Vector2 VectorToMiddle2 = new Vector2(
                            Mathf.Abs(zone.transform.position.x - CenterZone.transform.position.x),
                            Mathf.Abs(zone.transform.position.y - CenterZone.transform.position.y));
                            float totaldiff2 = VectorToMiddle2.x + VectorToMiddle2.y;
                            // Ak detekujeme zónu so spojeneckou jednotkou, získame jej názov
                            if (totaldiff1 > totaldiff2)
                                TargetZone = zone;
                            Debug.Log("Ideme k spojeneckej jednotke");
                        }
                    }
                    break;
                case 1:
                    foreach (Zone zone in validZones)
                    {
                        if (zone.WhoIsUnderControl() == Zone.ControlType.allied)
                        {
                            destinationZone = zone;
                            Debug.Log("Ideme dobiť zónu");
                        }
                    }
                    break;
            }
            if (enemyUnit.GetTurnMiddlePoints() <= 0)
            {
                TargetZone = CenterZone;
                enemyUnit.SetTurnMiddlePoints(3);
                Debug.Log("Ideme na stred");
            }
            // Náhodne vyberieme cieľovú zónu, ak ani zóna s nepriateľom, ani spojencami nie je v dosahu, ak je niečo, toto sa prepíše
            if (TargetZone != null)
            {
                // Získame vzdialenosť k našej cieľovej pozícii
                Vector2 VectorToDestination = new Vector2(
                    Mathf.Abs(enemyUnit.ReturnCurrentStandingZone().transform.position.x - TargetZone.transform.position.x),
                    Mathf.Abs(enemyUnit.ReturnCurrentStandingZone().transform.position.y - TargetZone.transform.position.y));
                // Získame rozdiel v osi x a y
                float xdiff = VectorToDestination.x;
                float ydiff = VectorToDestination.y;
                float totaldiff = xdiff + ydiff;

                foreach (Zone zone in validZones)
                {
                    if (zone == TargetZone)
                    {
                        // Ak je naša cieľová zóna hneď vedľa našej aktuálnej zóny, presunieme sa tam a preskočíme slučku
                        destinationZone = zone;
                        TargetZone = null;
                        StayStill = true;
                        break;

                    }
                    else
                    {
                        // Tu sa porovnávajú pozície zón, ak je zóna bližšie k cieľu než aktuálna, presunieme sa do nej
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
                // Ak existuje zóna s nepriateľom v okolí, prepisujeme cieľovú zónu na zónu s nepriateľom
                if (zone.ReturnAllyUnitsInZone().Count > 0)
                {
                    destinationZone = zone;
                    StayStill = true;
                    // Ak je v okolí zóna s jednotkou spojenca, presunieme sa tam
                }
            }

            // Nastavenie minulých pozícií zóny na neplatné
            int index = 0;
            for (int i = 0; i < destinationZone.GetEnemyMoveLocationStatuses().Length; i++)
            {
                if (destinationZone.GetEnemyMoveLocationStatuses()[i] == false)
                {
                    destination = destinationZone.GetEnemyMoveLocations()[i]; // Získame pozíciu Vector2 zóny
                    destinationZone.SetEnemyPositionStatus(i, true); // Nastavíme, že zóna je obsadená
                    index = i;
                    break;
                }
            }

            enemyUnit.SetStandingZone(destinationZone, index);
            // Presun jednotky smerom k zvolenej zóne
            enemyUnit.SetRunningAnimation(true);
            enemyUnit.GetMoveAction().Move(destination);
            if (destination.x > enemyUnit.transform.position.x) enemyUnit.FlipUnit();
            // Presunutie do zóny
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
            else { enemyUnit.DoAction(destinationZone); }
        }
    }
    private IEnumerator DelayedSecondMove(Unit enemyUnit)
    {
        // Počkajte 2 sekundy pred druhým presunom
        yield return new WaitForSeconds(1.5f);

        // Skontrolujte, či je jednotka stále platná a nebola už presunutá dvakrát
        if (enemyUnit != null && enemyUnit.GetActionPoints() > 0)
        {
            // Náhodne vyberte ďalšiu cieľovú zónu pre druhý presun
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

                // Náhodne vyberte cieľovú zónu
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
                    Debug.Log("Presúvame sa do cieľovej zóny" + previousTargetZone.name);
                    // Všetko je vysvetlené predtým
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
                                seconddestinationZone = zone; // Vypočítame rozdiel v pozícii x+y a vyberieme najbližšiu zónu
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

                // Nastavenie minulých pozícií zóny na neplatné
                int index = 0;
                for (int i = 0; i < seconddestinationZone.GetEnemyMoveLocationStatuses().Length; i++)
                {
                    if (seconddestinationZone.GetEnemyMoveLocationStatuses()[i] == false)
                    {
                        destination = seconddestinationZone.GetEnemyMoveLocations()[i]; // Získame pozíciu Vector2 zóny
                        seconddestinationZone.SetEnemyPositionStatus(i, true); // Nastavíme, že zóna je obsadená
                        index = i;
                        break;
                    }
                }

                enemyUnit.SetStandingZone(seconddestinationZone, index);
                enemyUnit.GetMoveAction().Move(destination);
            }
        }
    }
}
