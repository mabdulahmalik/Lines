import moment from "moment";

function convertToMoment(datetime: string): moment.Moment{
  if(!datetime) return moment();
  const utcDateTime = datetime.replace(/([+-]\d{2}:\d{2})$/, 'Z');
  const localDateTime = moment(utcDateTime).local();

  return localDateTime;  
}

export function formatRelativeDate(dateString: string): string {
  return convertToMoment(dateString).fromNow();

  const date = new Date(dateString);
  const now = new Date();
  const seconds = Math.floor((now.getTime() - date.getTime()) / 1000);

  const intervals: { [key: string]: number } = {
    y: 31536000, // year
    mo: 2592000, // month
    w: 604800, // week
    d: 86400, // day
    h: 3600, // hour
    m: 60, // minute
    s: 1, // second
  };

  if(seconds < 60){
    return 'just now';
  }

  for (const interval in intervals) {
    const count = Math.floor(seconds / intervals[interval]);
    if (count >= 1) {
      return `${count}${interval} ago`;
    }
  }

  return 'just now';
}

// Function to calculate elapsed time
export function calculateElapsedTime(datetime: string, includeSeconds: boolean = false): string{
  if(!datetime) return '';

  const start = convertToMoment(datetime);
  const now = moment();
  
  const duration = moment.duration(now.diff(start));
  
  const hours = String(duration.hours()).padStart(2, '0');
  const minutes = String(duration.minutes()).padStart(2, '0');
  const seconds = String(duration.seconds()).padStart(2, '0');

  return includeSeconds ? `${hours}:${minutes}:${seconds}` : `${hours}:${minutes}`;
}

export function calculateDuration(fromDatetime: string, toDatetime: string, includeSeconds: boolean = false): string{
  if(!fromDatetime || !toDatetime) return '';

  const start = convertToMoment(fromDatetime);
  const end = convertToMoment(toDatetime)
  
  const duration = moment.duration(end.diff(start));
  
  const hours = String(duration.hours()).padStart(2, '0');
  const minutes = String(duration.minutes()).padStart(2, '0');
  const seconds = String(duration.seconds()).padStart(2, '0');

  return includeSeconds ? `${hours}:${minutes}:${seconds}` : `${hours}:${minutes}`;
}

export function formatMinutesToDuration (minutes: number | undefined | null): string {
  if (typeof minutes !== 'number' || minutes < 0) return '00:00';

  const hours = Math.floor(minutes / 60);
  const remainingMinutes = minutes % 60;

  const formattedHours = String(hours).padStart(2, '0');
  const formattedMinutes = String(remainingMinutes).padStart(2, '0');

  return `${formattedHours}:${formattedMinutes}`;
};


export function formatDatetime(datetime: string, format: string): string{
  const momentDatetime = convertToMoment(datetime);
  
  return momentDatetime.format(format);
}

export function formatDate (date: Date): string {
  const options: Intl.DateTimeFormatOptions = {
    month: 'short',
    day: 'numeric',
    year: 'numeric',
    hour: 'numeric',
    minute: 'numeric',
    hour12: true,
  };
  return new Intl.DateTimeFormat('en-US', options).format(date);
};

// Date Format By Day/Month/Year

export function formatDateByDMY(dateString: string | null | undefined): string {
  if (!dateString) {
    return '';
  }

  return formatDatetime(dateString, 'DD / MM / YYYY')
}

