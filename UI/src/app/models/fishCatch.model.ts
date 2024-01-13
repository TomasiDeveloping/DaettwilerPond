import {CatchDetailModel} from "./catchDetail.model";

export interface FishCatchModel{
  id: string;
  catchDate: Date;
  hoursSpent: number;
  startFishing?: Date;
  endFishing?: Date;
  fishingLicenseId: string;
  amountFishCatch?: number;
  catchDetails?: CatchDetailModel[];
}
