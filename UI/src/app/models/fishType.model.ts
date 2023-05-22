export interface FishType {
  id: string;
  name: string;
  closedSeasonFromDay?: number;
  closedSeasonFromMonth?: number;
  closedSeasonToDay?: number;
  closedSeasonToMonth: number;
  minimumSize?: number;
  hasClosedSeason: boolean;
  hasMinimumSize: boolean;
  closedSeasonsInDays?: number;
}
