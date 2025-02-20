export type RecordStatus = null | 'Active' | 'Inactive';

interface associatedLine{
    id: string;
}
interface associatedJob{
  id: string;
}
export interface Record {
  id: string;
  mrn: string;
  firstName?: string,
  lastName?: string,
  birthDate?: string,
  status?: RecordStatus;
  lastSeen?: string;
  createdAt?: string;
  updatedAt?: string;
  hasInfection?: boolean;
  externalPlacement?: boolean;
  externalPlacementBy?: string;
  associatedJobs?: associatedJobs[];
  associatedLines?: associatedLine[];
}
