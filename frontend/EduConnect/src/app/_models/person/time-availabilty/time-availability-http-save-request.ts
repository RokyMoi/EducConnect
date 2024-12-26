import { DayOfWeek } from '../../../../enums/day-of-week.enum';

export interface TimeAvailabilityHttpSaveRequest {
  dayOfWeek: number;
  startTime: string;
  endTime: string;
}
