import { Photo } from "./photo";

export interface User {
  id: number;
  username: string;
  knownAs: string;
  age: number;
  gender: Gender;
  DateCreated: Date;
  lastActive: Date;
  photoUrl: string;
  city: string;
  country: string;
  interests?: string;
  introduction?: string;
  lookingFor?: Gender;
  photos?: Photo[];
}

enum Gender {
  male = 0,
  female = 1,
  both = 2
}
