import {expect, test} from 'vitest';
import {ApplicationViewModel} from "./ApplicationViewModel.js";
import {Localization} from "./Localization.js";

test('Initial state is Loading', () => {
    const model = new ApplicationViewModel();
    expect(model.status.value).toBe(Localization.StatusLoading);
});

test('State after loading is finished is Loaded', async () => {
    const model = new ApplicationViewModel();
    await model.load();
    expect(model.status.value).toBe(Localization.StatusLoaded);
});
