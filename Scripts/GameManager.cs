using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour {
    //buttons
    public Button Endturn, WTInvestment, LKInvestment, AQInvestment, WaterShipment;

    //texts
    public Text Population, WaterConsumptionRate, WaterDistributionRate, Temperature, 
        Turn, Fund, TaxRevenue, WinText, WTLevel, LR, LK, AR, AQ, SP,
        Drought, Monsoon, WarmFront, ColdFront, MigrationIn, MigrationOut, Rain;

    //model objects

    //win and lose event
    //Event WinEvent, LoseEvent;

    //data object
    public BackgroundData data;

    //gameplay mechanics
    int gamelength, turn;
    private int RainTurnCount;
    private int RainInterval;
    private int AverageRainInterval;
    private int CurrentTempEvent = 2, CurrentTempEventTurnCount, CurrentRainEvent = 2, CurrentRainEventTurnCount, CurrentPopEvent = 2, CurrentPopEventTurnCount;

    // Use this for initialization
    void Start () {
        gamelength = 50;
        turn = 0;
        data = new BackgroundData(10000, 80, 100000, 3);
        //lake
        data.WaterSources[0]= new WaterSource("Lake",100000, 20000, .5, .5);
        //aquifer
        data.WaterSources[1] = new WaterSource("Aquifer",100000, 20000, .5, .5);
        //shipments
        data.WaterSources[2] = new WaterSource("Shipment",0, 0, 1, 1);

        //set initial water towers
        data.CalculateWaterDistributionRate();

        //Reset Win Event
        WinText.text = "";

        //Set Initial Rain Interval and TurnCount
        RainTurnCount = Random.Range(2, 4);
        RainInterval = Random.Range(3, 5);
        AverageRainInterval = RainInterval;

        //reseting event icons/text
        Rain.enabled = false;
        Drought.enabled = false;
        Monsoon.enabled = false;
        MigrationIn.enabled = false;
        MigrationOut.enabled = false;
        WarmFront.enabled = false;
        ColdFront.enabled = false;
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
            if(CurrentTempEvent == 0)
            {
                data.SetEvent(1,Random.Range(5,7));
                CurrentTempEventTurnCount--;
                WarmFront.enabled = true;
                ColdFront.enabled = false;
            }
            else if(CurrentTempEvent == 1)
            {
                data.SetEvent(2, Random.Range(5, 7));
                CurrentTempEventTurnCount--;
                WarmFront.enabled = false;
                ColdFront.enabled = true;
            }
            else
            {
                WarmFront.enabled = false;
                ColdFront.enabled = false;
            }

            if(CurrentPopEvent == 0)
            {
                data.SetEvent(3, Random.Range(5000, 10000));
                CurrentPopEventTurnCount--;
                MigrationIn.enabled = true;
                MigrationOut.enabled = false;
            }
            else if(CurrentPopEvent == 1)
            {
                data.SetEvent(4, Random.Range(5000, 10000));
                CurrentPopEventTurnCount--;
                MigrationIn.enabled = false;
                MigrationOut.enabled = true;
            }
            else
            {
                MigrationIn.enabled = false;
                MigrationOut.enabled = false;
            }

            if (CurrentRainEvent == 0)
            {
                Drought.enabled = true;
                Monsoon.enabled = false;
                CurrentRainEventTurnCount--;
            }
            else if(CurrentRainEvent ==1)
            {
                Drought.enabled = false;
                Monsoon.enabled = true;
                CurrentRainEventTurnCount--;
            }
            else
            {
                Drought.enabled = false;
                Monsoon.enabled = false;
            }

            data.IncrementPopulation(10);
            data.ExtractWaterSources();
            data.CalculateWaterDistributionRate();
            data.IncrementFund();
            turn++;
            GenerateEvents();
        }
        else if(turn == gamelength)
        {
            WinText.text = "You Win";
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
        int RainEvents = Random.Range(0,7);
        if (CurrentTempEventTurnCount == 0 && TempEvents < 2)
        {
            CurrentTempEvent = TempEvents;
            CurrentTempEventTurnCount = Random.Range(5, 8);

        }
        else
        {
            CurrentTempEvent = TempEvents;
            CurrentTempEventTurnCount = 0;
        }
        if (CurrentPopEventTurnCount == 0 && PopEvents < 2)
        {
            CurrentPopEvent = PopEvents;
            CurrentPopEventTurnCount = Random.Range(1, 2);

        }
        else
        {
            CurrentPopEvent = PopEvents;
            CurrentPopEventTurnCount = 0;
        }
        if (CurrentRainEventTurnCount == 0 && RainEvents < 2)
        {
            CurrentRainEvent = RainEvents;
            CurrentRainEventTurnCount = Random.Range(5, 8);
            if (RainEvents == 0)
            {
                RainInterval = AverageRainInterval * 2;
            }
            else
                RainInterval = AverageRainInterval / 2;

        }
        else
        {
            CurrentRainEvent = RainEvents;
            CurrentRainEventTurnCount = 0;
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
        WTLevel.text = "Water Towers Level: " + data.WaterTowers.ToString();
        LR.text = "Lake Reserve: " + data.WaterSources[0].GetReserve().ToString();
        LK.text = "Lake Availability: " + data.WaterSources[0].GetAvailability().ToString();
        AR.text = "Aquifer Reserve: " + data.WaterSources[1].GetReserve().ToString();
        AQ.text = "Aquifer Availability: " + data.WaterSources[1].GetAvailability().ToString();
        SP.text = "Storage Availability: " + data.WaterSources[2].GetReserve().ToString();
        
        

    }
}
