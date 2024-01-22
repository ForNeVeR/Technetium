export class CalendarViewModel {

    onSelecting(range: { start: Date; end: Date }): boolean {
        console.log(range);
        return false;
    }
}
