using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BackgroundData
{
    // Scaling Constants
    double TaxRevenueScaling = 1, WaterConsumptionRatePopScaling = 1, WaterConsumptionRateTempScaling = 1,
        WaterTowerScaling = 750, TempEventScaling = 1, MigrationEventScaling = 1
        ;

    // Gameplay Variables
    public int TaxRevenue, Population, Temperature = 70, Fund,
        WaterConsumptionRate, WaterDistributionRate;

    // private variables use for calculation
    private int[] AmmountPulledFromSources;
    private int NumberofSources;
    private int MaxTemp=120, MinTemp=20, MaxPop = 20000, MinPop = 5000;

    // Upgradables and Event
    public WaterSource[] WaterSources;
    public int WaterTowers = 1;
    string CurrentEvent;

    // constructor
    public BackgroundData(int population, int temperature, int fund, int NumberofWaterSources)
    {
        WaterSources = new WaterSource[NumberofWaterSources];
        AmmountPulledFromSources = new int[NumberofWaterSources];

        Temperature = temperature;
        Fund = fund;
        SetPopulation(population);
        NumberofSources = NumberofWaterSources;
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
        WaterConsumptionRate = (int)(Population * WaterConsumptionRatePopScaling + Temperature * WaterConsumptionRateTempScaling);

    }

    public void SetPopulation(int newpop)
    {
        Population = newpop;
        TaxRevenue = (int)(Population * TaxRevenueScaling);
        WaterConsumptionRate = (int)(Population * WaterConsumptionRatePopScaling + Temperature * WaterConsumptionRateTempScaling);
    }

    // Upgrading water tower, also changes WaterDistributionRate, and set AmmountPulled
    public void UpgradeWaterTowers()
    {

        WaterTowers++;
        CalculateWaterDistributionRate();

    }

    // Calculate WaterDistributionRate
    public void CalculateWaterDistributionRate()
    {
        int i = 0, dis = 0, sum = 0;
        while (i < NumberofSources)
        {
            sum = sum + WaterSources[i].GetAvailability();
            i++;
        }
        i = 0;
        if(sum <= WaterConsumptionRate && sum <= WaterTowers*WaterTowerScaling)
        {
            WaterDistributionRate = sum;
            while (i < NumberofSources)
            {
                AmmountPulledFromSources[i] = WaterSources[i].GetAvailability();
                i++;
            }
        }
        else if (sum <= WaterConsumptionRate && sum >= WaterTowers * WaterTowerScaling
              || sum >= WaterConsumptionRate && sum >= WaterTowers * WaterTowerScaling && WaterConsumptionRate >= WaterTowers * WaterTowerScaling)
        {
            WaterDistributionRate = (int)(WaterTowers * WaterTowerScaling);
            while (i < NumberofSources)
            {
                AmmountPulledFromSources[i] = WaterSources[i].GetAvailability() * WaterDistributionRate / sum;
                i++;
            }
        }
        else if(sum >= WaterConsumptionRate && sum <= WaterTowers * WaterTowerScaling
            ||  sum >= WaterConsumptionRate && sum >= WaterTowers * WaterTowerScaling && WaterConsumptionRate <= WaterTowers * WaterTowerScaling)
        {
            WaterDistributionRate = WaterConsumptionRate;
            while(i < NumberofSources)
            {
                AmmountPulledFromSources[i] = WaterSources[i].GetAvailability()*WaterDistributionRate/sum;
                i++;
            }
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
        if (holder >= 0) {
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
                while (i<NumberofSources)
                {
                    if(WaterSources[i].type!="Shipment")
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
                IncrementPopulation((int)(-1*intensity * MigrationEventScaling));
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

    
