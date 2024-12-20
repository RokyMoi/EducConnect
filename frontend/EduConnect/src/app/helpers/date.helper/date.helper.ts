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
}
