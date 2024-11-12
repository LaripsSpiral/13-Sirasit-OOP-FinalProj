using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEatable
{
    public float FoodAmount { get; }
    public void EatenBy(Player player, float foodAmount);
}
