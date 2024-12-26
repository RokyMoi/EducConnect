export default class TimeHelper {
  public static getTimeDifferenceInMinutes(
    time1: string,
    time2: string
  ): number {
    const [hours1, minutes1] = time1.split(':').map(Number);
    const [hours2, minutes2] = time2.split(':').map(Number);
    console.log('Hours1: ' + hours1);
    console.log('Minutes1: ' + minutes1);
    console.log('Hours2: ' + hours2);
    console.log('Minutes2: ' + minutes2);

    const totalMinutes1 = hours1 * 60 + minutes1;
    const totalMinutes2 = hours2 * 60 + minutes2;
    console.log('Total minutes 1: ' + totalMinutes1);
    console.log('Total minutes 2: ' + totalMinutes2);
    const differenceInMinutes = Math.abs(totalMinutes2 - totalMinutes1);
    console.log('Difference in minutes: ' + differenceInMinutes);
    return differenceInMinutes;
  }
}
