import React from 'react';
import {render} from 'react-dom';
import {ApplicationViewModel} from "./Application/ApplicationViewModel.js";
import {ApplicationView} from "./Application/ApplicationView.js";
import {noAwait} from "./Util/Promises.js";

const viewModel = new ApplicationViewModel();

render(<ApplicationView viewModel={viewModel}/>, document.getElementById('app'));

noAwait(viewModel.load().then(() => {
    console.log('Application loaded successfully');
}));
