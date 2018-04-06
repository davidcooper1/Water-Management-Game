using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class BackgroundData
    {
        // Scaling Constants
        double TaxRevenueScaling, WaterConsumptionRatePopScaling, WaterConsumptionRateTempScaling,
            WaterTowerScaling, DroughtEventScaling = 1, FiltrationEventScaling = 1
            ;

        // Gameplay Variables
        public int TaxRevenue, Population, Temperature, Fund,
            WaterConsumptionRate, WaterDistributionRate;

        // private variables use for calculation
        private int[] AmmountPulledFromSources;

        // Upgradables and Event
        public WaterSource[] WaterSources;
        public int WaterTowers;
        string CurrentEvent;

        // constructor
        public BackgroundData(int population, int temperature, int fund, double taxrevenuescale, double WCRPopScale, double WCRTempScale, double WTScale, int NumberofWaterSources)
        {
            WaterSources = new WaterSource[NumberofWaterSources];
            AmmountPulledFromSources = new int[NumberofWaterSources];

            Temperature = temperature;
            Fund = fund;
            TaxRevenueScaling = taxrevenuescale;
            WaterConsumptionRatePopScaling = WCRPopScale;
            WaterConsumptionRateTempScaling = WCRTempScale;
            WaterTowerScaling = WTScale;
            SetPopulation(population);

        }

        // increment population also increment TaxRevenue
        public void IncrementPopulation(int a)
        {
            Population += a;
            TaxRevenue = (int)(Population * a * TaxRevenueScaling);
            WaterConsumptionRate = (int)(Population * WaterConsumptionRatePopScaling + Temperature * WaterConsumptionRateTempScaling);

        }

        public void SetPopulation(int newpop)
    {
        Population = newpop;
        TaxRevenue = (int)(Population * TaxRevenueScaling);
        WaterConsumptionRate = (int)(Population * WaterConsumptionRatePopScaling + Temperature * WaterConsumptionRateTempScaling);
    }

        // Upgrading water tower, also changes WaterDistributionRate, and set AmmountPulled
        public void UpgradeWaterTowers(int spent)
        {
            WaterTowers += (int)(spent * WaterTowerScaling);
            int i = 0, sum = 0, ammountpulledfromeach;
            while (WaterSources[i] != null)
            {
                ammountpulledfromeach = AsymptoticByY_Helper(WaterTowers, (WaterSources[i]).GetAvailability());
                AmmountPulledFromSources[i] = ammountpulledfromeach;
                i++;
                sum += ammountpulledfromeach;
            }
            WaterDistributionRate = sum;

        }

        // set water tower to fix ammount
        public void SetWaterTowers(int fix)
        {
            WaterTowers += fix;
            int i = 0, sum = 0, ammountpulledfromeach;
            while (WaterSources[i] != null)
            {
                ammountpulledfromeach = AsymptoticByY_Helper(WaterTowers, (WaterSources[i]).GetAvailability());
                AmmountPulledFromSources[i] = ammountpulledfromeach;
                i++;
                sum += ammountpulledfromeach;
            }
            WaterDistributionRate = sum;
        }

        //Extracting from sources
        public int ExtractWaterSources()
        {
            int i = 0, sum = 0;
            while (WaterSources[i] != null)
            {
                sum +=WaterSources[i].DepleteReserve(AmmountPulledFromSources[i]);
                i++;
        }
            return sum;
        }

        private int AsymptoticByY_Helper(int x, int c)
        {
            return ((x - 1) / (x + 1) + 1) * c / 2;
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
                while (WaterSources[i] != null)
                {
                    WaterSources[i].RefillReserve(intensity);
                }
            }
            //drought
            else if (type == 1)
            {
                CurrentEvent = "Drought";
                Temperature += (int)(intensity * DroughtEventScaling);
            }
            //Filtration breakdown
            /* else if(type == 2)
             {
                 WaterDistributionRate -= 
             }*/
        }

    }
