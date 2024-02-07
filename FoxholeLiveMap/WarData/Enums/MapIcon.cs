using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoxholeSimpleAPI.WarData.Enums
{
    public enum MapIcon
    {
        StaticBase1 = 5, // Removed in Update 46
        StaticBase2 = 6, // Removed in Update 46
        StaticBase3 = 7, // Removed in Update 46
        ForwardBase1 = 8,
        ForwardBase2 = 9, // Removed in Update 50
        ForwardBase3 = 10, // Removed in Update 50
        Hospital = 11,
        VehicleFactory = 12,
        Armory = 13, // Removed in previous update
        SupplyStation = 14, // Removed in previous update
        Workshop = 15, // Removed in previous update
        ManufacturingPlant = 16, // Removed in previous update
        Refinery = 17,
        Shipyard = 18,
        TechCenter = 19, // Engineering Center in Update 37
        SalvageField = 20,
        ComponentField = 21,
        FuelField = 22,
        SulfurField = 23,
        WorldMapTent = 24,
        TravelTent = 25,
        TrainingArea = 26,
        SpecialBaseKeep = 27, // Update 14
        ObservationTower = 28, // Update 14
        Fort = 29, // Update 14
        TroopShip = 30, // Update 14
        SulfurMine = 32, // Update 16
        StorageFacility = 33, // Update 17
        Factory = 34, // Update 17
        GarrisonStation = 35, // Update 20
        AmmoFactory = 36, // Removed in previous update
        RocketSite = 37, // Update 20
        SalvageMine = 38, // Update 22
        ConstructionYard = 39, // Update 26
        ComponentMine = 40, // Update 26
        OilWell = 41, // Removed in Update 50
        RelicBase1 = 45, // Update 32
        RelicBase2 = 46, // Removed in Update 52 until further notice (use Relic Base 1)
        RelicBase3 = 47, // Removed in Update 52 until further notice (use Relic Base 1)
        MassProductionFactory = 51, // Update 35
        Seaport = 52, // Update 37
        CoastalGun = 53, // Update 37
        SoulFactory = 54, // Update 39
        TownBase1 = 56, // Update 46
        TownBase2 = 57, // Removed in Update 52 until further notice (use Town Base 1)
        TownBase3 = 58, // Removed in Update 52 until further notice (use Town Base 1)
        StormCannon = 59, // Update 47
        IntelCenter = 60, // Update 47
        CoalField = 61, // Update 50
        OilField = 62, // Update 50
        RocketTarget = 70, // Update 54
        RocketGroundZero = 71, // Update 54
        RocketSiteWithRocket = 72, // Update 54
        FacilityMineOilRig = 75 // Update 54
    }
}
