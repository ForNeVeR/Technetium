import {ApplicationViewModel} from "./ApplicationViewModel.js";
import {StatusBarView} from "./StatusBarView.js";
import React from "react";
import {CalendarView} from "../Calendar/CalendarView.js";
import {MenuView} from "../Menu/MenuView.js";

export const ApplicationView = ({ viewModel }: { viewModel: ApplicationViewModel }) => {
    return [
        <MenuView/>,
        <CalendarView />,
        <StatusBarView status={viewModel.status} />
    ]
};
