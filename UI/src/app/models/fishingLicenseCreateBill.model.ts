export interface FishingLicenseCreateBill {
  userIds: string[];
  licenseYear?: number;
  emailMessage: string;
  createLicense: boolean;
}
