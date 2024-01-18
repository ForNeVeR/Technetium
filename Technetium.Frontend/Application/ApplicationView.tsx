import {ApplicationViewModel} from "./ApplicationViewModel.js";
import {StatusBarView} from "./StatusBarView.js";
import React from "react";

export const ApplicationView = ({ viewModel }: { viewModel: ApplicationViewModel }) => {
    return [
        <StatusBarView status={viewModel.status} />
    ]
};
