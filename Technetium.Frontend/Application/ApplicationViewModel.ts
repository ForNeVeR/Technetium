import {BehaviorSubject} from "rxjs";
import {Localization} from "./Localization.js";
import {CalendarViewModel} from "../Calendar/CalendarViewModel.js";

export class ApplicationViewModel {
    status = new BehaviorSubject(Localization.StatusLoading);
    calendar = new CalendarViewModel();
    async load(): Promise<void> {
        // TODO[#23]: Go to the server and load the current state
        this.status.next(Localization.StatusLoaded);
    }
}
