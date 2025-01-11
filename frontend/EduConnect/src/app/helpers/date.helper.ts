export default class DateHelper {
  /**
   * Converts a Date object to a "yyyy-MM-dd" string.
   * @param date - The Date object to convert.
   * @returns A string in "yyyy-MM-dd" format.
   */
  public static toDateOnlyString(date: Date): string {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    const day = date.getDate().toString().padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  public static toDateFromDateOnlyString(dateOnly: string): Date {
    const year = parseInt(dateOnly.substring(0, 4));
    const month = parseInt(dateOnly.substring(5, 7)) - 1;
    const day = parseInt(dateOnly.substring(8, 10));
    return new Date(year, month, day);
  }

  public static toDateFromUnixMillis(unixMillis: number | string): Date {
    return new Date(Number(unixMillis));
  }
}
