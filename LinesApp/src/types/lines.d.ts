export type LineStatus = null | 'In' | 'Out';
interface associatedJob{
    id: string;
}
export interface Line {
  id: string;
  name: string;
  type: string;
  room?: string;
  status?: LineStatus;
  nextChange?: string;
  hasInfection?: boolean;
  externalPlacement?: boolean;
  externalPlacementBy?: string;
  patientName?: string;
  associatedJobs?: associatedJob[];
}
