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
        Drought, WarmFront, ColdFront, MigrationIn, MigrationOut, Rain;

    //model objects

    //win and lose event
    //Event WinEvent, LoseEvent;

    //data object
    public BackgroundData data;

    //gameplay mechanics
    int gamelength, turn;

	// Use this for initialization
	void Start () {
        gamelength = 20;
        turn = 0;
        data = new BackgroundData(1000, 80, 100000, 1, 1, 1, .001, 3);
        //lake
        data.WaterSources[0]= new WaterSource(100000, 100, .5, .5);
        //aquifer
        data.WaterSources[1] = new WaterSource(100000, 100, .5, .5);
        //shipments
        data.WaterSources[2] = new WaterSource(0, 0, 1, 1);

        //set initial water towers
        data.SetWaterTowers(1);

        //Reset Win Event
        WinText.text = "";

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
            
            data.IncrementPopulation(10);
            data.ExtractWaterSources();
            turn++;
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
            data.DepleteFund(spent);
            data.UpgradeWaterTowers(spent);
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
            data.SetWaterTowers(data.WaterTowers);
        }
    }

    private void WaterShipmentListener(WaterSource w, int spent)
    {
        if (data.DepleteFund(spent) == true)
        {
            w.IncSource(spent);
            data.SetWaterTowers(data.WaterTowers);
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
