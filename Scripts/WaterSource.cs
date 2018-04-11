using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource{
    double AvailabilityScale, RefillScale;
    int Reserve, Availability, MaxAvailability;
    public string type;

    //getter and setter for availability
    public int GetAvailability()
    {
        return Availability;
    }

    //availability increase when there is investment
    public bool Investment(int i)
    {
        int holder =(int)(Availability+ AvailabilityScale * i);
        MaxAvailability = holder;
        if (holder >=Reserve)
        {
            Availability = Reserve;
            return false;
        }
        Availability = holder;
        return true;
    }

    //getter for reserve
    public int GetReserve()
    {
        return Reserve;
    }

    //incrementing reserve and availability, use for water shipments
    public void IncSource(int a)
    {
        Reserve += (int) (a*RefillScale);
        Availability += (int)(a * RefillScale);
        RefillScale = RefillScale * .8;
    }

    //refilling reserve, base on rain
    public void RefillReserve(int rainstrength)
    {
        Reserve += (int)(rainstrength * RefillScale);
        if (Reserve > MaxAvailability)
            Availability = MaxAvailability;
        else
            Availability = Reserve;
    }

    //pulling from reserve
    public int DepleteReserve(int i)
    {
        int holder= Reserve-i;
        if (holder >= 0)
        {
            Reserve = holder;
            if (Availability > Reserve)
                Availability = Reserve;
            return i;
        }
        holder = Reserve;
        Reserve = 0;
        Availability = 0;
        return holder;
    }

    //constructor(reserve, availability, investmentscale, refillingscale)
    public WaterSource(string name, int reserve, int availability, double investscale, double refillingscale)
    {
        type = name;
        Reserve = reserve;
        Availability = availability;
        MaxAvailability = availability;
        AvailabilityScale = investscale;
        RefillScale = refillingscale;
    }
}
