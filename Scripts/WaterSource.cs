using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource{
    double AvailabilityScale, RefillScale;
    int Reserve, Availability;

    //getter for availability
    public int GetAvailability()
    {
        return Availability;
    }

    //availability increase when there is investment
    public bool Investment(int i)
    {
        int holder =(int)(Availability+ AvailabilityScale * i);
        if (holder >=Reserve)
        {
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

    //refilling reserve, base on rain
    public void RefillReserve(int rainstrength)
    {
        Reserve += (int)(rainstrength * RefillScale);
    }

    //pulling from reserve
    public int DepleteReserve(int i)
    {
        int holder= Reserve-i;
        if (holder >= 0)
        {
            Reserve = holder;
            return i;
        }
        holder = Reserve;
        Reserve = 0;
        return holder;
    }

    //constructor(reserve, availability, investmentscale, refillingscale)
    public WaterSource(int reserve, int availability, double investscale, double refillingscale)
    {
        Reserve = reserve;
        Availability = availability;
        AvailabilityScale = investscale;
        RefillScale = refillingscale;
    }
}
