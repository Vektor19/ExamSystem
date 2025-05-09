import { Violation } from './Violation';

export interface Participant {
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  completeStatus: string;
  isBlocked: string;
  grade: Number;
  violations: Violation[];
}
