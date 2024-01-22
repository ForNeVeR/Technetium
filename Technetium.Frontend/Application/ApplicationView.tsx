import {ApplicationViewModel} from "./ApplicationViewModel.js";
import {StatusBarView} from "./StatusBarView.js";
import React from "react";
import {CalendarView} from "../Calendar/CalendarView.js";

export const ApplicationView = ({ viewModel }: { viewModel: ApplicationViewModel }) => {
    return [
        <CalendarView viewModel={viewModel.calendar} />,
        <StatusBarView status={viewModel.status} />
    ]
};
