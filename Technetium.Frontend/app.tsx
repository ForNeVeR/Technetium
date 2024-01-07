import React, {useState} from 'react';
import {render} from 'react-dom';

type State = 'Loading' | 'Loaded';

const Component = (props: {state: State}) => {
    if (props.state === 'Loading') {
        return <div>Loading…</div>;
    } else {
        return <div>Loaded</div>
    }
}

const App = () => {
    const [state, setState] = useState<State>('Loading');
    if (state == 'Loading') {
        setTimeout(() => setState('Loaded'), 1000);
    }

    return <Component state={state}/>;
};

render(<App/>, document.getElementById('app'));
