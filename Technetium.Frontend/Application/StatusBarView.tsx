import React, {useEffect, useState} from "react";
import {Subject} from 'rxjs';
import {Localization} from "./Localization.js";

export const StatusBarView = ({status}: { status: Subject<string> }) => {
    const [statusString, setStatusString] = useState(Localization.StatusLoading);
    useEffect(() => {
        const subscription = status.subscribe(setStatusString);
        return () => subscription.unsubscribe();
    }, []);

    return <div>{statusString}</div>;
};
