class TimeUtils {
  formatUtcToLocalIso(utcString: string): string {
    const date = new Date(utcString);

    const corrected = new Date(
      date.getTime() - date.getTimezoneOffset() * 60000
    );

    const year = corrected.getFullYear();
    const month = String(corrected.getMonth() + 1).padStart(2, "0");
    const day = String(corrected.getDate()).padStart(2, "0");
    const hours = String(corrected.getHours()).padStart(2, "0");
    const minutes = String(corrected.getMinutes()).padStart(2, "0");
    const seconds = String(corrected.getSeconds()).padStart(2, "0");

    const offsetMinutes = -date.getTimezoneOffset();
    const offsetSign = offsetMinutes >= 0 ? "+" : "-";
    const offsetHours = String(
      Math.floor(Math.abs(offsetMinutes) / 60)
    ).padStart(2, "0");
    const offsetMins = String(Math.abs(offsetMinutes % 60)).padStart(2, "0");
    const offset = `${offsetSign}${offsetHours}:${offsetMins}`;

    return `${year}-${month}-${day}T${hours}:${minutes}:${seconds}${offset}`;
  }
  toDatetimeLocalString(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    const hours = String(date.getHours()).padStart(2, "0");
    const minutes = String(date.getMinutes()).padStart(2, "0");
    return `${year}-${month}-${day}T${hours}:${minutes}`;
  }
}

export default new TimeUtils();
