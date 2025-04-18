import { DayOfWeek } from '../../../../enums/day-of-week.enum';

export interface TimeAvailability {
  personAvailabilityId: string;
  dayOfWeek: DayOfWeek;
  startTime: string;
  endTime: string;
}
