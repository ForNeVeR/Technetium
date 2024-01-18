import React from 'react';
import {render} from 'react-dom';
import {ApplicationViewModel} from "./Application/ApplicationViewModel.js";
import {ApplicationView} from "./Application/ApplicationView.js";
import {NoAwait} from "./Util/Promises.js";

const viewModel = new ApplicationViewModel();

render(<ApplicationView viewModel={viewModel}/>, document.getElementById('app'));

NoAwait(viewModel.load().then(() => {
    console.log('Application loaded successfully');
}));
