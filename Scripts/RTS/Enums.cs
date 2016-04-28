
namespace RTS 
{
	// don't forget about the arrays and dicks in GameManager that might need to be changed if you change an enum
	public enum Species {Bunnies, Deer, Sheep, Wolves, NonPlayer, Grass, Carrots}
	public enum UnitStatus {Dead, Garrisoned, Idle, Moving, Attacking, StoppedMoving, ReturningHome};
	public enum ResourceType {Gold, Wood, Unique}
	public enum PanelButtonType {BackButton, BuildMenu, BuildingMenuButton, CaravanButton, FunctionButton1, LabButton, LocalUpgradesButton, PopulationsButton, RepairButton, StatsButton, UnitsButton, StatsInfo, UnitsMenu, UnitsPopCount, UpgradeButton};
	public enum Eras {StoneAge, Classical, Renaissance, Industrial, Information};
	public enum StatsType {Health, Defense, Attack, Cost, BuildingCost, MobTrainerStats, ResourceStats, StratStats, CapitalStats, CaravanStats, RangedStats, MobileStats, UniqueStats, TimeStats, Population, CarryingCapacity, BirthRates, DeathRates, EatRates, ProtectionRates, Nutrients, Interference, NotSetYet};
	public enum AttackType {Blunt, Pierce, Crush, Incendiary, Psychological};
	public enum SpecType {Nature, Sun, Stick, Rock, Wheel, Fire, NotSelected};
	public enum WorldObjectType {WorldObject, Building, Tower, Mobtrainer, Sentry, NonRelatedUnitTrainer, SpeciesUnitTrainer, StrategicPoint, Resource, Monument, Mobile, SentryMob, Unit, Melee, Ranged, LightUnit, HeavyUnit, Siege, Empty}; 
}
