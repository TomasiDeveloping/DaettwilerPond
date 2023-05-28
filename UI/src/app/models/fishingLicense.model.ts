export interface FishingLicense {
  id: string,
  userFullName: string;
  userId: string;
  createdAt: Date;
  updatedAt: Date;
  year: number;
  isPaid: boolean
  issuedBy: string;
  isActive: boolean;
  expiresOn: Date
}
