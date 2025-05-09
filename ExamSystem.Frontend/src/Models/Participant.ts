import { Violation } from './Violation';

export interface Participant {
  examUserId: string;
  userId: string;
  firstName: string;
  lastName: string;
  email: string;
  completeStatus: string;
  isBlocked: string;
  grade: Number;
  violations: Violation[];
}
