export type Priority = null | 'STAT' | 'NORMAL' | 'ROUTINE';
export type JobStatus = null | 'Scheduled' | 'Active' | 'Underway' | 'Closed'| 'Completed' | 'Cancelled' | 'Deleted';

export interface Note {
  text?: string;
  author?: string;
  dateTime?: string;
}
export interface Person {
  initials?: string;
  img?: string;
}
interface Photo {
  file: File;
  id: string;
  url: string;
  status: 'loading' | true | false | null;
  date: Date;
}
type FormElements = 'text' | 'number' | 'checkbox' | 'radio' | 'select' | 'textarea';

interface ObjValue {
  name: string;
  value: string|number|boolean;
}
interface Properties {
    name: string; //required 
    label?: string; //optional
    type: FormElements; //required
    required: boolean;
    value: boolean| string[] | string  | number | ObjValue | ObjValue[];
}
export interface JobProcedure {
    name: string;
    properties: Properties[];
}
export interface Job {
  id: string;
  // job details
  purpose: string; //title
  contact?: string;
  externalPlacement?: boolean;
  externalPlacementBy?: string;
  orderingProvider?: string;
  // location
  facility?: string;
  room?: string;
  description?: string;
  // Patient information
  firstName?: string;
  lastName?: string;
  birthDate?: string;
  mrn?: string;
  // notes
  notes?: Note[];
  // Urgency/Schedule
  schedule?: boolean;
  date?: string;
  time?: string;
  checkOnHold?: boolean;
  checkStat?: boolean;
  onHoldMessage?: string;

  timer?: string;
  hasInfection?: boolean;
  priority?: Priority; // Stat, Normla or Routine
  status?: JobStatus; // Closed/Active/etc..
  assignees?: Person[];

  createdAt?: string;
  photos?: Photo[];
  procedures?: Procedure[];
}
