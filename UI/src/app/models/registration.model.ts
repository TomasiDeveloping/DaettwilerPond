import {Address} from "./address.model";

export interface Registration {
  firstName: string;
  lastName: string;
  email: string;
  role: string;
  isActive: string;
  address: Address
}
