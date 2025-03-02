
import { UserPreference } from '@/api/__generated__/graphql';
import { useMeStore } from '@/stores/data/settings/users/me';
import moment from 'moment';

export enum DateTimeFormatMode {
  Complete = 'complete',
  Limited = 'limited',
  Static = 'static'
}

class DateTimeFormatter {
  private static getUserPreferences() {
    const meStore = useMeStore();
    const preferences = meStore.me?.preferences || [];

    return {
      elapsedTime: preferences.includes(UserPreference.ElapsedTime),
      militaryTime: preferences.includes(UserPreference.MilitaryTime),
      middleEndianDate: preferences.includes(UserPreference.MiddleEndianDate)
    };
  }

  private static convertToMoment(datetime: string): moment.Moment{
    if(!datetime) return moment();
    const utcDateTime = datetime.replace(/([+-]\d{2}:\d{2})$/, 'Z');
    const localDateTime = moment(utcDateTime).local();
  
    return localDateTime;  
  }

  private static getDateFormat(): string {
    const { middleEndianDate } = this.getUserPreferences();
    return middleEndianDate ? 'MM/DD/YYYY' : 'MMM Do YYYY';
  }

  private static getTimeFormat(): string {
    const { militaryTime } = this.getUserPreferences();
    return militaryTime ? 'HH:mm' : 'hh:mm a';
  }

  public static formatDatetime(
    datetime: string,
    mode: DateTimeFormatMode = DateTimeFormatMode.Complete
  ): string {
    if (!datetime) return '';

    const momentDatetime = this.convertToMoment(datetime);
    const preferences = this.getUserPreferences();

    if (mode === DateTimeFormatMode.Complete && preferences.elapsedTime) {
      return momentDatetime.fromNow();
    }

    const dateFormat = this.getDateFormat();
    const timeFormat = this.getTimeFormat();

    if (mode === DateTimeFormatMode.Limited) {
      return momentDatetime.format(`${dateFormat} ${timeFormat}`);
    }

    // Static mode (predefined format)
    return momentDatetime.format('MMM D, YYYY h:mm A');
  }

  public static formatElapsedTime(datetime: string): string {
    if (!datetime) return '';
    return this.convertToMoment(datetime).fromNow();
  }

  public static formatDuration(fromDatetime: string, toDatetime: string, includeSeconds: boolean = false): string {
    if (!fromDatetime || !toDatetime) return '';

    const start = this.convertToMoment(fromDatetime);
    const end = this.convertToMoment(toDatetime);
    const duration = moment.duration(end.diff(start));

    const hours = String(duration.hours()).padStart(2, '0');
    const minutes = String(duration.minutes()).padStart(2, '0');
    const seconds = String(duration.seconds()).padStart(2, '0');

    return includeSeconds ? `${hours}:${minutes}:${seconds}` : `${hours}:${minutes}`;
  }
}

export default DateTimeFormatter;
