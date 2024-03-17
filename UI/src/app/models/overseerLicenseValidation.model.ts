export interface OverseerLicenseValidationModel {
  licenseId: string;
  userName: string;
  userSaNaNumber: string;
  userBirthDate: Date;
  expiryAt: Date;
  userImageUrl: string;
  isValid: boolean;
  isActive: boolean;
  errorMessage: string;
}
