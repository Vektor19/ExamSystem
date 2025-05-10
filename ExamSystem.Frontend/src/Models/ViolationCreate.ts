export interface ViolationCreate {
  examUserId: string;
  violationType: string;
  description: string;
  isCritical: boolean;
}