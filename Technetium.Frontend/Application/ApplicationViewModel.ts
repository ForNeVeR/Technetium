import {BehaviorSubject} from "rxjs";
import {Localization} from "./Localization.js";

export class ApplicationViewModel {
    status = new BehaviorSubject(Localization.StatusLoading);
    async load(): Promise<void> {
        // TODO: Go to the server and load the current state
        this.status.next(Localization.StatusLoaded);
    }
}
