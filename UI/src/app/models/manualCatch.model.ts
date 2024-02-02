import {CatchDetailModel} from "./catchDetail.model";

export interface ManualCatchModel {
  catchDate: Date;
  hoursSpent: number;
  fishingLicenseId: number;
  catchDetails: CatchDetailModel[];
}
