using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {
    //buttons
    public Button Endturn, WTInvestment, LKInvestment, AQInvestment, WaterShipment;

    //texts
    public Text Population, WaterConsumptionRate, WaterDistributionRate, Temperature, 
        Turn, Fund, TaxRevenue, WinText, LoseText, WDRMax, LR, LK, AR, AQ, SP,
        Drought, Monsoon, WarmFront, ColdFront, MigrationIn, MigrationOut, Rain, LoseCounterText;

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
    private int LoseCounter=10;

    //backup data for undo

    // Use this for initialization
    void Start () {
        gamelength = 50;
        turn = 0;
        //initializing default game variable values
        data = new BackgroundData(10000, Random.Range(60,80), 10000, 3);
        AverageTemperature = data.Temperature;
        //lake
        data.WaterSources[0]= new WaterSource("Lake",100000, 7000, 1, 1);
        //aquifer
        data.WaterSources[1] = new WaterSource("Aquifer",100000, 7000, 1, 1);
        //shipments
        data.WaterSources[2] = new WaterSource("Shipment",0, 0, 1, 10);

        //set initial water towers
        data.CalculateWaterDistributionRate();


        //Set Initial Rain Interval and TurnCount
        RainTurnCount = Random.Range(2, 4);
        RainInterval = Random.Range(3, 5);
        AverageRainInterval = RainInterval;

        //reseting event icons/text
        WinText.enabled = false;
        Rain.enabled = false;
        Drought.enabled = false;
        Monsoon.enabled = false;
        MigrationIn.enabled = false;
        MigrationOut.enabled = false;
        WarmFront.enabled = false;
        ColdFront.enabled = false;
        LoseText.enabled = false;
        LoseCounterText.text = "Lose Counter: 10";
        //buttons
        Endturn.onClick.AddListener(EndturnListener);
        WTInvestment.onClick.AddListener(delegate { WTInvestmentListener(1000); });
        LKInvestment.onClick.AddListener(delegate { ResourceInvestmentListener(data.WaterSources[0],1000); });
        AQInvestment.onClick.AddListener(delegate { ResourceInvestmentListener(data.WaterSources[1],1000); });
        WaterShipment.onClick.AddListener(delegate { WaterShipmentListener(data.WaterSources[2],1000); });
    }


    //button listeners
    private void EndturnListener()
    {
        if (turn < gamelength)
        {
            if(LoseCounter == 0)
            {
                LoseText.enabled = true;
                turn = 50;
            }
            else if (data.WaterDistributionRate < data.WaterConsumptionRate)
            {
                LoseCounter--;
                LoseCounterText.text = "Lose Counter: " + LoseCounter.ToString();
            }
            data.ExtractWaterSources();

            if (RainTurnCount == 0)
            {

                data.SetEvent(0, Random.Range(5000, 10000));
                RainTurnCount = RainInterval;
                Rain.enabled = true;
            }
            else
            {
                Rain.enabled = false;
                RainTurnCount--;
            }
            if (CurrentTempEventTurnCount != 0 && CurrentTempEvent < 2)
            {
                if (CurrentTempEvent == 0)
                {
                    data.SetEvent(1, Random.Range(5, 7));
                    CurrentTempEventTurnCount--;
                    WarmFront.enabled = true;
                    ColdFront.enabled = false;
                }
                else if (CurrentTempEvent == 1)
                {
                    data.SetEvent(2, Random.Range(5, 7));
                    CurrentTempEventTurnCount--;
                    WarmFront.enabled = false;
                    ColdFront.enabled = true;
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
                WarmFront.enabled = false;
                ColdFront.enabled = false;
            }

            if (CurrentPopEventTurnCount != 0 && CurrentPopEvent < 2)
            {
                if (CurrentPopEvent == 0)
                {
                    data.SetEvent(3, Random.Range(1000, 5000));
                    CurrentPopEventTurnCount--;
                    MigrationIn.enabled = true;
                    MigrationOut.enabled = false;
                }
                else if (CurrentPopEvent == 1)
                {
                    data.SetEvent(4, Random.Range(1000, 5000));
                    CurrentPopEventTurnCount--;
                    MigrationIn.enabled = false;
                    MigrationOut.enabled = true;
                }
            }
            else
            {
                if (CurrentPopEventTurnCount > 0)
                    CurrentPopEventTurnCount--;
                MigrationIn.enabled = false;
                MigrationOut.enabled = false;
            }

            if (CurrentRainEventTurnCount != 0 && CurrentRainEvent < 2)
            {
                if (CurrentRainEvent == 0)
                {
                    Drought.enabled = true;
                    Monsoon.enabled = false;
                    CurrentRainEventTurnCount--;
                }
                else if (CurrentRainEvent == 1)
                {
                    Drought.enabled = false;
                    Monsoon.enabled = true;
                    CurrentRainEventTurnCount--;
                }
            }
            else
            {
                if (CurrentRainEventTurnCount > 0)
                    CurrentRainEventTurnCount--;
                Drought.enabled = false;
                Monsoon.enabled = false;
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
        else if(turn == gamelength)
        {
            WinText.enabled = true;
        }

    }



    private void WTInvestmentListener(int spent)
    {
        if (data.DepleteFund(spent) == true)
        {
            data.UpgradeWaterTowers();
            /*int i= 0;
            while (data.AmmountPulledFromSources[i] != 0)
            {
                print(data.AmmountPulledFromSources[i]);
                i++;
            }*/

        }
    }

    private void ResourceInvestmentListener(WaterSource w,int spent)
    {
        if (data.DepleteFund(spent) == true)
        {
            w.Investment(spent);
            data.CalculateWaterDistributionRate();
        }
    }

    private void WaterShipmentListener(WaterSource w, int spent)
    {
        if (data.DepleteFund(spent) == true)
        {
            w.IncSource(spent);
            data.CalculateWaterDistributionRate();
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
        else if(CurrentTempEventTurnCount == 0)
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

        if(CurrentPopEvent < 2 && CurrentPopEventTurnCount == 0)
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
    void Update () {
        Population.text = "Population: "+data.Population.ToString();
        WaterConsumptionRate.text = "Consumption Rate: "+data.WaterConsumptionRate.ToString();
        WaterDistributionRate.text = "Distribution Rate: "+data.WaterDistributionRate.ToString();
        Temperature.text = "Temperature: " + data.Temperature.ToString();
        Turn.text = "Turn: " + turn.ToString();
        Fund.text = "Fund: " + data.Fund.ToString();
        TaxRevenue.text = "Tax Revenue: " + data.TaxRevenue.ToString();
        WDRMax.text = "WDRMax: " + data.MaxWaterDistributionRate.ToString();
        LR.text = "Lake Reserve: " + data.WaterSources[0].GetReserve().ToString();
        LK.text = "Lake Availability: " + data.WaterSources[0].GetAvailability().ToString();
        AR.text = "Aquifer Reserve: " + data.WaterSources[1].GetReserve().ToString();
        AQ.text = "Aquifer Availability: " + data.WaterSources[1].GetAvailability().ToString();
        SP.text = "Storage Availability: " + data.WaterSources[2].GetReserve().ToString();
        
        

    }
}
