import {Address} from "./address.model";

export interface Registration {
  firstName: string;
  lastName: string;
  email: string;
  role: string;
  isActive: string;
  dateOfBirth: Date;
  saNaNumber: string;
  address: Address
}
