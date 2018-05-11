using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BackgroundData
{
    // Scaling Constants
    double TaxRevenueScaling = .1, WaterConsumptionRatePopScaling = 50, WaterConsumptionRateTempScaling = 1,
        WaterTowerScaling = 1000, TempEventScaling = 1, MigrationEventScaling = 1
        ;

    // Gameplay Variables
    public int TaxRevenue, Population, Temperature, Fund,
        WaterConsumptionRate, WaterDistributionRate;

    // private variables use for calculation
    private int[] AmmountPulledFromSources;
    private int NumberofSources, AverageTemperature;
    private int MaxTemp = 120, MinTemp = 10, MaxPop = 20000, MinPop = 5000;

    // Upgradables and Event
    public WaterSource[] WaterSources;
    public int WaterTowers = 100, MaxWaterDistributionRate;
    string CurrentEvent;


    // constructor
    public BackgroundData(int population, int temperature, int fund, int NumberofWaterSources)
    {
        WaterSources = new WaterSource[NumberofWaterSources];
        AmmountPulledFromSources = new int[NumberofWaterSources];

        Temperature = temperature;
        AverageTemperature = Temperature;
        Fund = fund;
        SetPopulation(population);
        NumberofSources = NumberofWaterSources;
        WaterTowers = Random.Range(50, 150);
        MaxWaterDistributionRate = (int)(WaterTowers * WaterTowerScaling);
    }

    // Increment Fund
    public void IncrementFund()
    {
        Fund += TaxRevenue;
    }
    // increment population also increment TaxRevenue
    public void IncrementPopulation(int a)
    {
        Population += a;
        TaxRevenue = (int)(Population * TaxRevenueScaling);
        WaterConsumptionRate = (int)(Population * WaterConsumptionRatePopScaling * (double)Temperature / (double)AverageTemperature);

    }

    public void SetPopulation(int newpop)
    {
        Population = newpop;
        TaxRevenue = (int)(Population * TaxRevenueScaling);
        WaterConsumptionRate = (int)(Population * WaterConsumptionRatePopScaling * (double)Temperature / (double)AverageTemperature);
    }

    // Upgrading water tower, also changes WaterDistributionRate, and set AmmountPulled
    public void UpgradeWaterTowers(int spent, int ExtractMode)
    {

        WaterTowers += spent / 10;
        MaxWaterDistributionRate = (int)(WaterTowerScaling * WaterTowers);
        CalculateWaterDistributionRate(ExtractMode);

    }

    // Calculate WaterDistributionRate
    public void CalculateWaterDistributionRate(int mode)
    {
        int i = 0, last = NumberofSources, sum = 0;
        if(mode == 3)
        {
            while (i < last)
            {
                AmmountPulledFromSources[i] = 0;
                i++;
            }
            WaterDistributionRate = 0;
        }
        else if (mode == 0)
        {
            last = NumberofSources - 1;
            AmmountPulledFromSources[last] = 0;
        }
        else if (mode == 1)
        {
            while (i < NumberofSources - 1)
            {
                AmmountPulledFromSources[i] = 0;
                i++;
            }
        }
        while (i < last)
        {
            AmmountPulledFromSources[i] = WaterSources[i].GetAvailability();
            sum = sum + WaterSources[i].GetAvailability();
            i++;
        }
        WaterDistributionRate = Mathf.Min(sum, MaxWaterDistributionRate, WaterConsumptionRate);
        i = 0;
        while (i < NumberofSources)
        {
            if (AmmountPulledFromSources[i] != 0)
            {
                AmmountPulledFromSources[i] = (int)(AmmountPulledFromSources[i] * ((double)WaterDistributionRate / (double)sum));
                Debug.Log("distribution test: " + AmmountPulledFromSources[i]);
            }
            i++;
        }

    }

    //Extracting from sources
    public int ExtractWaterSources()
    {
        int i = 0, sum = 0;
        while (i < NumberofSources)
        {
            sum += WaterSources[i].DepleteReserve(AmmountPulledFromSources[i]);
            i++;
        }
        return sum;
    }

    private int AsymptoticByY_Helper(double x, double c)
    {
        double a = ((x - 1) / (x + 1) + 1);
        double b = c / 2;
        double d = a * b;
        return (int)d;
    }

    // fund automatically change base on TaxRevenue
    public void SetFund()
    {
        Fund += TaxRevenue;
    }

    public bool DepleteFund(int spent)
    {
        int holder = Fund - spent;
        if (holder >= 0)
        {
            Fund = holder;
            return true;
        }
        return false;
    }

    //Event setter
    public void SetEvent(int type, int intensity)
    {
        //rain
        if (type == 0)
        {
            int i = 0;
            CurrentEvent = "Rain";
            while (i < NumberofSources)
            {
                if (WaterSources[i].type != "Shipment")
                    WaterSources[i].RefillReserve(intensity);
                i++;
            }
        }
        //Warm Front
        if (type == 1)
        {
            int temp = Temperature + (int)(intensity * TempEventScaling);
            if (temp < MaxTemp)
                Temperature = temp;
            else
                Temperature = MaxTemp;
        }
        //Cold Front
        else if (type == 2)
        {
            //CurrentEvent = "Cold Front";
            int temp = Temperature - (int)(intensity * TempEventScaling);
            if (temp > MinTemp)
                Temperature = temp;
            else
                Temperature = MinTemp;
        }
        //Migration in
        else if (type == 3)
        {
            int pop = Population + (int)(intensity * MigrationEventScaling);
            if (pop < MaxPop)
                IncrementPopulation((int)(intensity * MigrationEventScaling));
            else
                SetPopulation(MaxPop);

        }
        //Migration out
        else if (type == 4)
        {
            int pop = Population - (int)(intensity * MigrationEventScaling);
            if (pop > MinPop)
                IncrementPopulation((int)(-1 * intensity * MigrationEventScaling));
            else
                SetPopulation(MinPop);
        }
        //Filtration breakdown
        /* else if(type == 2)
         {
             WaterDistributionRate -= 
         }*/
    }
}


