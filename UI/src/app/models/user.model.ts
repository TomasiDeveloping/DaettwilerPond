import {Address} from "./address.model";

export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  isActive: boolean;
  saNaNumber?: string;
  dateOfBirth: Date;
}

export interface UserWithAddress {
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  isActive: boolean,
  address: Address;
  role?: string;
  saNaNumber?: string;
  imageUrl?:string;
  dateOfBirth: Date;
}
