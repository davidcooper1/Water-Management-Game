using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //buttons
    public Button Endturn, WTInvestment, LKInvestment, AQInvestment, WaterShipment,
        Invest1000, Invest2000, Invest5000;

    //texts
    public Text Population, WaterConsumptionRate, WaterDistributionRate, Temperature,
        Turn, Fund, TaxRevenue, WDRMax, LK, AQ, LoseCounterText;
    public CityController City;
    public Slider LakeReserve, AquiferReserve, ShipmentReserve;
    public GameObject Drought, Monsoon, WarmFront, ColdFront, MigrationIn, MigrationOut, WinText, LoseText;
    //button and text helpers
    private int CurrentInvestment;
    //model objects

    //win and lose event
    //Event WinEvent, LoseEvent;

    //data object
    public BackgroundData data;

    //gameplay mechanics
    int gamelength, turn;
    private int RainTurnCount;
    private int RainInterval;
    private int AverageRainInterval, AverageTemperature;
    private int CurrentTempEvent = 2, CurrentTempEventTurnCount, CurrentRainEvent = 2, CurrentRainEventTurnCount, CurrentPopEvent = 2, CurrentPopEventTurnCount;
    private int LoseCounter = 10;

    //backup data for undo

    // Use this for initialization
    void Start()
    {
        gamelength = 50;
        turn = 0;
        //initializing default game variable values
        data = new BackgroundData(10000, Random.Range(60, 80), 10000, 3);
        AverageTemperature = data.Temperature;
        //lake
        data.WaterSources[0] = new WaterSource("Lake", 100000, 5000, 1, 1);
        //aquifer
        data.WaterSources[1] = new WaterSource("Aquifer", 100000, 5000, 1, 1);
        //shipments
        data.WaterSources[2] = new WaterSource("Shipment", 0, 0, 1, 10);

        //set initial water towers
        data.CalculateWaterDistributionRate();


        //Set Initial Rain Interval and TurnCount
        RainTurnCount = Random.Range(2, 4);
        RainInterval = Random.Range(3, 5);
        AverageRainInterval = RainInterval;

        //reseting event icons/text
        WinText.SetActive(false);
        // Rain.enabled = false;
        Drought.SetActive(false);
        Monsoon.SetActive(false);
        MigrationIn.SetActive(false);
        MigrationOut.SetActive(false);
        WarmFront.SetActive(false);
        ColdFront.SetActive(false);
        LoseText.SetActive(false);
        LoseCounterText.text = "Lives: 10";
        //buttons
        Endturn.onClick.AddListener(EndturnListener);
        LakeReserve.maxValue = data.WaterSources[0].GetReserve();
        AquiferReserve.maxValue = data.WaterSources[1].GetReserve();
        ShipmentReserve.maxValue = 50000;

        City.SetLakeMax(100000);
        City.SetLakeCurrent(LakeReserve.maxValue);

        WTInvestment.onClick.AddListener(delegate { SetCurrentInvestment(0); });
        LKInvestment.onClick.AddListener(delegate { SetCurrentInvestment(1); });
        AQInvestment.onClick.AddListener(delegate { SetCurrentInvestment(2); });
        Invest1000.onClick.AddListener(delegate { InvestInCurrent(1000); });
        Invest2000.onClick.AddListener(delegate { InvestInCurrent(2000); });
        Invest5000.onClick.AddListener(delegate { InvestInCurrent(5000); });
        WaterShipment.onClick.AddListener(delegate { SetCurrentInvestment(3); });
    }


    //button listeners
    private void EndturnListener()
    {
        if (turn < gamelength)
        {
            if (LoseCounter == 0)
            {
                LoseText.SetActive(true);
                turn = 50;
            }
            else if (data.WaterDistributionRate < data.WaterConsumptionRate)
            {
                LoseCounter--;
                LoseCounterText.text = "Lives: " + LoseCounter.ToString();
            }
            data.ExtractWaterSources();
            Debug.Log("Lake Reserve: " + data.WaterSources[0].GetReserve() + " Max Reserve: " + 100000);
            City.SetLakeCurrent(data.WaterSources[0].GetReserve());

            if (RainTurnCount == 0)
            {

                data.SetEvent(0, Random.Range(10, 20));
                RainTurnCount = RainInterval;
                City.SetRainEnabled(true);
            }
            else
            {
                City.SetRainEnabled(false);
                RainTurnCount--;
            }
            if (CurrentTempEventTurnCount != 0 && CurrentTempEvent < 2)
            {
                if (CurrentTempEvent == 0)
                {
                    data.SetEvent(1, Random.Range(5, 7));
                    CurrentTempEventTurnCount--;
                   WarmFront.SetActive(true);
                    ColdFront.SetActive(false);
                }
                else if (CurrentTempEvent == 1)
                {
                    data.SetEvent(2, Random.Range(5, 7));
                    CurrentTempEventTurnCount--;
                    WarmFront.SetActive(false);
                    ColdFront.SetActive(true);
                }
            }
            else
            {
                if (CurrentTempEventTurnCount > 0)
                    CurrentTempEventTurnCount--;
                if (data.Temperature < AverageTemperature)
                    data.SetEvent(1, Random.Range(3, 5));
                else
                    data.SetEvent(2, Random.Range(3, 5));
                WarmFront.SetActive(false);
                ColdFront.SetActive(false);
            }

            if (CurrentPopEventTurnCount != 0 && CurrentPopEvent < 2)
            {
                if (CurrentPopEvent == 0)
                {
                    data.SetEvent(3, Random.Range(1000, 5000));
                    CurrentPopEventTurnCount--;
                    MigrationIn.SetActive(true);
                    MigrationOut.SetActive(false);
                }
                else if (CurrentPopEvent == 1)
                {
                    data.SetEvent(4, Random.Range(1000, 5000));
                    CurrentPopEventTurnCount--;
                    MigrationIn.SetActive(false);
                    MigrationOut.SetActive(true);
                }
            }
            else
            {
                if (CurrentPopEventTurnCount > 0)
                    CurrentPopEventTurnCount--;
                MigrationIn.SetActive(false);
                MigrationOut.SetActive(false);
            }

            if (CurrentRainEventTurnCount != 0 && CurrentRainEvent < 2)
            {
                if (CurrentRainEvent == 0)
                {
                    Drought.SetActive(true);
                    Monsoon.SetActive(false);
                    CurrentRainEventTurnCount--;
                }
                else if (CurrentRainEvent == 1)
                {
                   Drought.SetActive(false);
                    Monsoon.SetActive(true);
                    CurrentRainEventTurnCount--;
                }
            }
            else
            {
                if (CurrentRainEventTurnCount > 0)
                    CurrentRainEventTurnCount--;
                Drought.SetActive(false);
                Monsoon.SetActive(false);
            }

            data.IncrementPopulation(10);
            data.CalculateWaterDistributionRate();
            data.IncrementFund();
            turn++;
            GenerateEvents();
            Debug.Log("Current Rain Event: " + CurrentRainEvent + ". Turn Count: " + CurrentRainEventTurnCount +
                "| Current Pop Event: " + CurrentPopEvent + ". Turn Count: " + CurrentPopEventTurnCount +
                "| Current Temp Event: " + CurrentTempEvent + ". Turn Count: " + CurrentTempEventTurnCount);
        }
        else if (turn == gamelength)
        {
            WinText.SetActive(true);
        }

    }


    private void SetCurrentInvestment(int c)
    {
        CurrentInvestment = c;
    }

    private void InvestInCurrent(int spent)
    {
        Debug.Log(CurrentInvestment);
        if (CurrentInvestment == 0)
        {
            if (data.DepleteFund(spent) == true)
            {
                data.UpgradeWaterTowers(spent);
            }
        }
        else if(CurrentInvestment == 1)
        {
            if (data.DepleteFund(spent) == true)
            {
                data.WaterSources[0].Investment(spent);
                data.CalculateWaterDistributionRate();
            }
        }
        else if(CurrentInvestment == 2)
        {
            if (data.DepleteFund(spent) == true)
            {
                data.WaterSources[1].Investment(spent);
                data.CalculateWaterDistributionRate();
            }
        }
        else if(CurrentInvestment == 3)
        {
            if (data.DepleteFund(spent) == true)
            {
                data.WaterSources[2].IncSource(spent);
                data.CalculateWaterDistributionRate();
            }
        }
    }

    // Event Generator
    private void GenerateEvents()
    {
        // 0 = WarmFront, 1 = ColdFront
        int TempEvents = Random.Range(0, 7);
        // 0 = Migration in, 1 = Migration out
        int PopEvents = Random.Range(0, 7);
        //0 = Drought, 1 = Monsoon
        int RainEvents = Random.Range(0, 7);
        //cooldown
        if (CurrentTempEvent < 2 && CurrentTempEventTurnCount == 0)
        {
            CurrentTempEvent = 8;
            CurrentTempEventTurnCount = Random.Range(2, 8);

        }
        else if (CurrentTempEventTurnCount == 0)
        {
            if (TempEvents < 2)
            {
                CurrentTempEvent = TempEvents;
                CurrentTempEventTurnCount = Random.Range(4, 7);

            }
            else if (TempEvents > 2)
            {
                CurrentTempEvent = TempEvents;
                CurrentTempEventTurnCount = 0;
            }

        }

        if (CurrentPopEvent < 2 && CurrentPopEventTurnCount == 0)
        {
            CurrentPopEvent = 8;
            CurrentPopEventTurnCount = Random.Range(2, 8);
        }
        else if (CurrentPopEventTurnCount == 0)
        {
            if (PopEvents < 2)
            {
                CurrentPopEvent = PopEvents;
                CurrentPopEventTurnCount = Random.Range(1, 3);

            }
            else if (PopEvents > 2)
            {
                CurrentPopEvent = PopEvents;
                CurrentPopEventTurnCount = 0;
            }
        }

        if (CurrentRainEvent < 2 && CurrentRainEventTurnCount == 0)
        {
            RainInterval = AverageRainInterval;
            CurrentRainEvent = 8;
            CurrentRainEventTurnCount = Random.Range(2, 8);
        }
        else if (CurrentRainEventTurnCount == 0)
        {
            if (RainEvents < 2)
            {
                CurrentRainEvent = RainEvents;
                CurrentRainEventTurnCount = Random.Range(4, 7);
                if (RainEvents == 0)
                {
                    RainInterval = AverageRainInterval * 2;
                }
                else
                    RainInterval = AverageRainInterval / 2;

            }
            else if (RainEvents > 2)
            {
                RainInterval = AverageRainInterval;
                CurrentRainEvent = RainEvents;
                CurrentRainEventTurnCount = 0;
            }
        }



    }

    // Update set texts; Population, WaterConsumptionRate, WaterDistributionRate, Temperature, Turn, Fund, TaxRevenue
    void Update()
    {
        Population.text = "Population: " + data.Population.ToString();
        WaterConsumptionRate.text = "Consumption Rate: " + data.WaterConsumptionRate.ToString();
        WaterDistributionRate.text = "Distribution Rate: " + data.WaterDistributionRate.ToString();
        Temperature.text = "Temperature: " + data.Temperature.ToString();
        Turn.text = "Turn: " + turn.ToString();
        Fund.text = "Fund: " + data.Fund.ToString();
        TaxRevenue.text = "Tax Revenue: " + data.TaxRevenue.ToString();
        WDRMax.text = "WDRMax: " + data.MaxWaterDistributionRate.ToString();
        LakeReserve.value = data.WaterSources[0].GetReserve();
        LK.text = "Lake Availability: " + data.WaterSources[0].GetAvailability().ToString();
        AquiferReserve.value = data.WaterSources[1].GetReserve();
        AQ.text = "Aquifer Availability: " + data.WaterSources[1].GetAvailability().ToString();
        ShipmentReserve.value = data.WaterSources[2].GetReserve();



    }
}
